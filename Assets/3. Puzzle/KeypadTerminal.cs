using UnityEngine;
using UnityEngine.UI;

public class KeypadTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] int puzzleId = 3;
    public void Interact(playerController player)
    {
        if (!player.photonView.IsMine) return;
        UIManager.Instance.OpenKeyPad(puzzleId);
    }

}
