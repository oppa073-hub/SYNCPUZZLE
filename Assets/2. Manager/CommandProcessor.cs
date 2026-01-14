using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    public static CommandProcessor Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    [SerializeField] playerController player;

    private void OnEnable() => player.OnInteract += Execute;
    private void OnDisable() => player.OnInteract -= Execute;

    private void Execute(ICommand cmd) => cmd.Execute();


}
