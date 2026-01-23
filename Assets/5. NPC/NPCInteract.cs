using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] DialogueData dialogueData;
    [SerializeField] NPCTalkUI talkUi;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            talkUi.EndDialogue();
        }
    }
    public void TryTalk()
    {
        if (!talkUi.IsOpen)
        {
            talkUi.StartDialogue(dialogueData.npcName, dialogueData.linesDefault);
        }
        else
        {
            talkUi.Next();
        }
    }
}
