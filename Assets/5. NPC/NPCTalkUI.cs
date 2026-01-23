using TMPro;
using UnityEngine;

public class NPCTalkUI : MonoBehaviour
{
    [SerializeField] GameObject NpcPanel;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text bodyText;
    string currentName;
    string[] currentLines;
    int lineIndex;
    bool isOpen;
    public bool IsOpen => isOpen;
    private void Start()
    {
        NpcPanel.SetActive(false);
    }

    public void StartDialogue(string npcName, string[] lines)
    {
        NpcPanel.SetActive(true);
        currentLines = lines;
        lineIndex = 0;
        nameText.text = npcName;
        isOpen = true;
        ShowLine();
    }
    public void ShowLine()
    {
        bodyText.text = currentLines[lineIndex];
    }
    public void Next()
    {
        if (!isOpen) return;
        lineIndex++;
        if (lineIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }
        ShowLine();
    }
    public void EndDialogue()
    {
        NpcPanel.SetActive(false);
        isOpen = false;
    }
}
