using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{

    [SerializeField] playerController player;
    
    private void OnEnable() => player.OnInteract += Execute;
    private void OnDisable() => player.OnInteract -= Execute;

    private void Execute(ICommand cmd)
    {
        if (player != null && !player.photonView.IsMine) return;
        cmd.Execute();
    }

}
