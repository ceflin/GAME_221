using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomName : MonoBehaviour {

    public int maxLength = 20;

    public InputField textField;

    public string[] adjectives = new string[]
    {
        "Blue", "Green", "Yellow", "Red", "Orange", "Cyan", "Magenta", "Brown", "Black", "Taupe", "Chartreuse", "Ebony", "Ivory", 
        "Action", "Accidental", "Awesome", "Arid", "Acquired",
        "Buzzing", "Bombing", "Blasted", "Binary", "Bankrupt",
        "Cold", "Cuddly", "Chilled", "Curled", "Close",
        "Deadly", "Dawning", "Dear", "Dunked", "Dumb",
        "Eroding", "Evil", "Extra", "Enormous", "Egregious",
        "Frank", "Furry", "Flaming", "Flipped", "Functional",
        "Greedy", "Ghostly", "Gambling", "Gripping", "Growing",
        "Honest", "Hateful", "Hip", "Heavy", "Hunting",
        "Icy", "Insane", "Idiotic", "Implied", "Incandescent",
        "Jealous", "Just", "Jumping", "Jovial", "Junked",
        "Kicking", "Killer", "Kissing", "Knifing", "Knavish",
        "Lemon", "Lucid", "Lazy", "Lesser", "Locked",
        "Main", "Mean", "Moaning", "Mint", "Muscular",
        "Neon", "Natural", "Numbered", "Needed", "Navy",
        "Open", "Occult", "Opulent", "Ordinary", "Overdone",
        "Proud", "Peeled", "Plum", "Piping", "Posturing",
        "Quaint", "Quiet", "Quirky", "Queued", "Quailing",
        "Reluctant", "Rose", "Rhyming", "Reversed", "Raging",
        "Silly", "Stupid", "Strong", "Stone", "Schooled",
        "Teal", "Tame", "Timid", "Towering", "Thundering",
        "Usual", "Undone", "Upset", "Uploaded", "Unsung",
        "Vibrant", "Vicious", "Vain", "Vapid", "Violent",
        "Worthy", "Waiting", "Wimpy", "Walloping", "Windy",
        "Xanthous", "Xeric", "Xenophobic", "Xenial", "Xylotomous",
        "Young", "Yielding", "Yucky", "Yawning", "Yearly",
        "Zealous", "Zesty", "Zany", "Zippy", "Zygotic"
    };

    public string[] nouns = new string[]
    {
        "Alley", "Aardvark", "Ammo", "Acrobat", "Abacus", "Asteroid", "Axe",
        "Ball", "Brawler", "Bully", "Bellows", "Blaster", "Beam", "Brick",
        "Cat", "Clutch", "Clip", "Cowboy", "Cow", "Cradle", "Crease",
        "Demon", "Destroyer", "Deacon", "Dimple", "Drifter", "Darkness", "Device",
        "Eraser", "Eulogy", "Emitter", "Emptiness", "Ease", "Essence", "Evidence",
        "Fault", "Flame", "Function", "Foot", "Fist", "Freezer", "Folly",
        "Gauntlet", "Greed", "Glow", "Guest", "Gang", "Gun", "Gaffe",
        "Hitcher", "Hacker", "Hunter", "Hangman", "Heaviness", "Hall", "Harp",
        "Igloo", "Icicle", "Idiot", "Insanity", "Isolation", "Ignorance", "Itch",
        "Justice", "Junk", "Joke", "Jester", "Jawbone",
        "Killer", "Kart", "Keeper", "King", "Kiss", "Knife",
        "Leopard", "Lion", "Lemur", "Looper", "Log", "Lunch", "Luck",
        "Moment", "Music", "Money", "Moon", "Micron", "Magnum", 
        "Net", "Nerd", "Night", "Noodle", "Narwahl",
        "Owl", "Occupant", "Outsider", "Octave",
        "Person", "Prisoner", "Prince", "Pile", "Private",
        "Queen", "Queer", 
        "Returner", "Riser", "Rice", "Reloader",
        "Student", "Sun", "Star", "Sky", "Slave", "Snack",
        "Tree", "Trumpet", "Tirade", "Teapot", "Turtle",
        "Umpire", "Underdog", "Uvula", 
        "Victor", "Victim", "Viola", "Violin", "Vanguard",
        "Winner", "Weather", "Writing", "Wasp", "Witherer", "Wisher", "Witch", "Warlock", "Warrior",
        "Yesterday", "Youth", "Yacht", "Yeller", "Yielder",
        "Zealot", "Zeal", "Zebra"
    };

	// Use this for initialization
	void Start () {
        SetRandomName();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public string GetRandomName()
    {
        for (int tries = 20; tries > 0; tries--)
        {
            string candidate = adjectives[Random.Range(0, adjectives.Length)] + nouns[Random.Range(0, nouns.Length)];
            if (candidate.Length <= maxLength) return candidate;
        }
        return "Player";
    }

    public void SetRandomName()
    {
        string randomName = GetRandomName();

        textField.text = randomName;
    }
    
}
