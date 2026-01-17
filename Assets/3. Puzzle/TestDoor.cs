using UnityEngine;
using System;

public class TestDoor : MonoBehaviour, IInteractable
{
    [SerializeField] int puzzleId = 1;
    public void Interact(playerController player)
    {
        if (!player.photonView.IsMine) return;
        //PuzzleManager.Instance.OnButtonPressed(this, player);
        PuzzleManager.Instance.RequestPress(puzzleId, 0, 1);

    }

}
