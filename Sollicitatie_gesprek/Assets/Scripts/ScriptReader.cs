using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public TextAsset InkJSONAsset;
    private Story StoryScript;

    public TMP_Text dialogueBox;
    public TMP_Text nameTag;

    void Start()
    {
        loadStory();
    }

    void Update() // Capitalized 'Update' here
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 'Space' should be capitalized
        {
            DisplayNextLine();
        }
    }

    void loadStory()
    {
        StoryScript = new Story(InkJSONAsset.text);
    }

    public void DisplayNextLine()
    {
        if (StoryScript.canContinue)
        {
            string text = StoryScript.Continue();
            text = text?.Trim();
            dialogueBox.text = text;
        }
        else
        {
            dialogueBox.text = "End of dialogue"; // Changed placeholder text
        }
    }
}
