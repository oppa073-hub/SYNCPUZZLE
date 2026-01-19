
using UnityEngine;

public class SwitchMatrix : MonoBehaviour, IInteractable
{
    [SerializeField] int leverNum = 1;
    [SerializeField] SpriteRenderer indicator;
    [SerializeField] int puzzleId = 4;
    Color color;
    public void Interact(playerController player)
    {
        if (!player.photonView.IsMine) return;
        PuzzleManager.Instance.RequestPress(puzzleId,leverNum - 1, 1);
    }
    private void Start()
    {
        color = indicator.color;
        PuzzleManager.Instance.RegisterLever(leverNum - 1, this);
    }
    public void ResetVisual()
    {
        if (indicator != null) indicator.color = color;
    }
    public void SetSolved(bool solved)
    {
        if (solved)
        {
            indicator.color = Color.gray;
        }
    }
    public void SetPressedVisual()
    {
        if (indicator) indicator.color = Color.gray;
    }
}
