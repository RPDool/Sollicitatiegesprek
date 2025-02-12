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
            // Exit the method if dialogueJson is null to prevent null reference errors
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
        dialoguetext.text = node.text;
        npcnametext.text = node.name;

        // Clear previous buttons
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Generate choice buttons for the new dialogue node
        foreach (var choice in node.choices)
        {
            GameObject newButton = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;

            // Ensure button calls ChooseOption() with the correct index
            int choiceIndex = node.choices.IndexOf(choice);
            newButton.GetComponent<Button>().onClick.AddListener(() => ChooseOption(choiceIndex));
        }
    }

    public void ChooseOption(int choiceIndex)
    {
        // Check if the current node is null or if the choice index is out of bounds
        if (currentNode == null || choiceIndex < 0 || choiceIndex >= currentNode.choices.Count)
        {
            Debug.LogError("Invalid choice!"); // Log an error message
            return; // Exit the method
        }

        // Get the ID of the next dialogue node based on the chosen option
        int nextId = currentNode.choices[choiceIndex].next;

        // If the next ID is 0 (or another end indicator), end the dialogue
        if (nextId == 0)
        {
            dialoguePanel.SetActive(false); // Hide the dialogue UI
            return; // Exit the method
        }

        // Find the next dialogue node by its ID
        DialogueNode nextNode = dialogueNodes.Find(node => node.ID == nextId);

        // If the next node is found, show it
        if (nextNode != null)
        {
            ShowNode(nextNode);
        }
        else
        {
            dialoguePanel.SetActive(false); // Hide the dialogue UI if the node is not found
        }
    }
}
