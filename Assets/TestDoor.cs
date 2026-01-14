using UnityEngine;
using System;

public class TestDoor : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject door;

    public void Interact(playerController player)
    {
        PuzzleManager.Instance.OnButtonPressed(this, player);
        if (door != null) door.SetActive(false);
        else Debug.LogWarning("door가 연결 안 됨");
    }

}
