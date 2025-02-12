using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Make sure this is included for TextMesh Pro
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueJson;

    [Header("UI Elements")]
    public GameObject dialoguePanel; // The dialogue UI panel
    public TextMeshProUGUI npcnametext;  // Corrected the casing here
    public TextMeshProUGUI dialoguetext; // Corrected the casing here
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;

    private List<DialogueNode> dialogueNodes;
    private DialogueNode currentNode;

    void Start()
    {
        LoadDialogue();
        StartDialogue();
    }

    void LoadDialogue()
    {
        if (dialogueJson == null)
        {
            return;
        }

        DialogueContainer dialogueData = JsonUtility.FromJson<DialogueContainer>(dialogueJson.text);

        if (dialogueData == null || dialogueData.start == null || dialogueData.start.Count == 0)
        {
            return;
        }

        dialogueNodes = dialogueData.start;
    }

    void StartDialogue()
    {
        if (dialogueNodes.Count > 0)
        {
            ShowNode(dialogueNodes[0]); // Start at the first node
        }
    }

    void ShowNode(DialogueNode node)
    {
        currentNode = node;
        dialoguetext.text = node.text; // Update dialogue text
        npcnametext.text = node.name;  // Update NPC name

        // ❌ Clear previous buttons (to avoid duplicate choices)
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // ✅ Generate choice buttons for the new dialogue node
        foreach (var choice in node.choices)
        {
            GameObject newButton = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;

            // ✅ Ensure button calls ChooseOption() with the correct index
            int choiceIndex = node.choices.IndexOf(choice);
            newButton.GetComponent<Button>().onClick.AddListener(() => ChooseOption(choiceIndex));
        }
    }

    public void ChooseOption(int choiceIndex)
    {
        if (currentNode == null || choiceIndex < 0 || choiceIndex >= currentNode.choices.Count)
        {
            Debug.LogError("Invalid choice!");
            return;
        }

        int nextId = currentNode.choices[choiceIndex].next;

        // If there is no valid next ID, end the dialogue
        if (nextId == 0)  // You can also check if it's -1 or any "end" indicator
        {
            dialoguePanel.SetActive(false); // Hide dialogue UI
            return;
        }

        // Find the next node by its ID
        DialogueNode nextNode = dialogueNodes.Find(node => node.ID == nextId);

        if (nextNode != null)
        {
            ShowNode(nextNode);
        }
        else
        {
            dialoguePanel.SetActive(false); // Hide dialogue UI
        }
    }
}
