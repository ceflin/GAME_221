using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour {
    
    private Vector3[] points;
    private List<VectorSegment> segs = new List<VectorSegment>();
    private float totalLength = 0;

    public List<VectorSegment> Segments { get { return segs; } }

	// Use this for initialization
	void Start () {

        points = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            print("Adding " + transform.GetChild(i).name);
            points[i] = transform.GetChild(i).position;
            points[i].y = transform.position.y;
        }

        for (int i = 0; i < points.Length - 1; i++)
        {
            segs.Add(new VectorSegment(points[i], points[i + 1]));
        }
        segs.Add(new VectorSegment(points[points.Length - 1], points[0]));

        foreach (VectorSegment seg in segs) totalLength += seg.length;
        print("Path length is " + totalLength.ToString());
    }

    // Update is called once per frame
    void Update () {
	
	}

    private Vector3 GetClosestPathPosition(Vector3 p, out int segIndex)
    {
        float minDSquared = -1;
        Vector3 closest = p;
        segIndex = -1;
        for (int i = 0; i < segs.Count; i++)
        {
            VectorSegment s = segs[i];
            Vector3 candidate = s.ClosestPoint(p);
            float dSquared = (p - candidate).sqrMagnitude;
            if (minDSquared < 0 || dSquared < minDSquared)
            {
                segIndex = i;
                closest = candidate;
                minDSquared = dSquared;
            }
        }
        return closest;
    }

    public Vector3 ProjectDownPath(Vector3 point, float distance)
    {
        int si = -1;
        point.y = 0;
        Vector3 loc = GetClosestPathPosition(point, out si);
        distance %= totalLength;
        while (distance > 0)
        {
            //print("Projecting down segment " + si.ToString());
            float segLength = segs[si].length - (segs[si].p1 - loc).magnitude;
            if (segLength > distance)
            {
                loc += segs[si].direction * distance;
                distance = 0;
            }
            else
            {
                distance -= segLength;
                si = (si == segs.Count - 1) ? 0 : si + 1;
                loc = segs[si].p1;
            }
        }
        return loc;

    }
    
}

public class VectorSegment: LineSegment
{
    public Vector3 direction;
    public float length;

    public VectorSegment(Vector3 start, Vector3 end)
        :base(start, end)
    {
        direction = (end - start).normalized;
        length = (end - start).magnitude;

    }
}

public class LineSegment : Line
{
    public Vector3 p1;
    public Vector3 p2;

    private const float e2 = 0.001f;

    public LineSegment(Vector3 one, Vector3 two)
        :base(one, two)
    {
        one.y = 0;
        two.y = 0;
        this.p1 = one;
        this.p2 = two;
    }

    public override Vector3 ClosestPoint(Vector3 point)
    {
        point.y = 0;
        Vector3 rot = new Vector3(p2.z - p1.z, 0, p1.x - p2.x);
        Vector3 cross = point + rot;
        LineSegment perp = new LineSegment(point, cross);
        Vector3 i;
        if (!base.Intersection(perp, out i))
            throw new System.Exception("No intersection");
        
        if (Contains(i)) return i;

        return (i - p1).sqrMagnitude < (i - p2).sqrMagnitude ? p1 : p2;
    }

    public bool Intersection(LineSegment other, out Vector3 intersection)
    {
        if (!base.Intersection(other as Line, out intersection))
        {
            return false;
        }

        return this.Contains(intersection) && other.Contains(intersection);
    }

    private bool Contains(Vector3 p)
    {
        Vector3 c = base.ClosestPoint(p);
        float distance = (p - c).sqrMagnitude;
        return distance <= e2 && Vector3.Dot(p1 - p, p2 - p) <= 0;
    }

}

public class Line
{
    public float a, b, c;

    public Line(Vector3 p1, Vector3 p2)
    {
        a = p1.z - p2.z;
        b = p2.x - p1.x;
        c = a * p1.x + b * p1.z;

        if (a == 0 && b == 0)
            throw new System.ArgumentException("P1=P2, invalid");
    }

    public virtual bool Intersection(Line other, out Vector3 intersection)
    {
        intersection = Vector3.zero;

        float d = other.a, e = other.b, f = other.c;
        float ce = c * e, bf = b * f, af = a * f, cd = c * d;
        float den = a * e - b * d;

        if (den == 0) return false;

        intersection = new Vector3((ce - bf) / den, 0, (af - cd) / den);
        return true;

    }

    public virtual Vector3 ClosestPoint(Vector3 point)
    {
        Vector3 cross = point + new Vector3(-a, 0, b);  // Weird but right
        Line perp = new Line(point, cross);
        Vector3 i;
        if (!Intersection(perp, out i))
        {
            throw new System.Exception("No closest point. Awesome.");
        }
        return i;

    }
}
