using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance; // to give dialogActivator

    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;

    public string[] dialogLines;

    public int currentLine;
    private bool justStarted;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        // dialogText.text = dialogLines[currentLine]; for testing
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1")) //button release
            {
                if (!justStarted) //dont skip first message (start button down, next buttonUp)
                {

                    currentLine++;

                    if(currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                    else // close when finihsed
                    {
                        checkIfName();
                        dialogText.text = dialogLines[currentLine]; 
                    }
                }
                else
                {
                    justStarted = false;
                }

            }
        }
    }

    public void showDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;
        currentLine = 0;

        checkIfName();

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        nameBox.SetActive(isPerson);

        justStarted = true;

    }

    public void checkIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }
}
