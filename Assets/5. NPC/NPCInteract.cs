using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] DialogueData dialogueData;
    [SerializeField] NPCTalkUI talkUi;
    [SerializeField] private AudioClip NpcSfx;
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
            AudioManager.instance.PlaySFX(NpcSfx);
            talkUi.StartDialogue(dialogueData.npcName, dialogueData.linesDefault);
        }
        else
        {
            AudioManager.instance.PlaySFX(NpcSfx);
            talkUi.Next();
        }
    }
}
