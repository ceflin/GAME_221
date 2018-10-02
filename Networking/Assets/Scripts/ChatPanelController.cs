using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanelController : MonoBehaviour
{
    public Text lblMessages;
    public InputField txtMessage;
    public Button btnSend;

    public int numOfLines = 5;

    private List<string> displayLines = new List<string>();

    private CustomNetworkControl myNetworkControl;
    private ChatController myChat;

    // Use this for initialization
    void Start()
    {
        lblMessages.text = string.Empty;

        txtMessage.ActivateInputField();
        txtMessage.Select();

        GameObject networkController = GameObject.FindGameObjectWithTag("NetworkController");
        if (networkController != null)
        {
            myNetworkControl = networkController.GetComponent<CustomNetworkControl>();
        }

        myChat = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<ChatController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (myNetworkControl != null)
        {
            displayLines.Clear();
            for (int i = myChat.messages.Count - 1; i >= 0 && displayLines.Count < numOfLines; i--)
            {
                displayLines.Add(myChat.messages[i]);
            }

            displayLines.Reverse();
            RefreshMessageDisplay();
        }
    }

    public void btnSend_Click()
    {
        string message = txtMessage.text.Trim();
        if (message.Length > 0)
        {
            if (myNetworkControl == null)
            {
                displayLines.Add(message);
                if (displayLines.Count > numOfLines)
                {
                    displayLines.RemoveAt(0);
                }

                RefreshMessageDisplay();
            }
            else
            {
                //myNetworkControl.sendChatMessage(message);
            }
            
            
            

            txtMessage.text = string.Empty;
        }

        txtMessage.ActivateInputField();
    }

    public void RefreshMessageDisplay()
    {
        lblMessages.text = string.Empty;
        for (int i = 0; i < displayLines.Count; i++)
        {
            lblMessages.text += displayLines[i];
            if (i < displayLines.Count - 1) lblMessages.text += "\n";
        }
    }

    public void txtMessage_EndEdit()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //print("Enter hit withing txtMessage");
            btnSend_Click();
        }
    }
}
