using UnityEngine;

public class SyncButton : MonoBehaviour, IInteractable
{
    [SerializeField] string buttonId = "A";
    [SerializeField] SpriteRenderer indicator;
    [SerializeField] int puzzleId = 2;
    Color color;
    bool isSolved = false;
    private void Start()
    {
         color = indicator.color;
        PuzzleManager.Instance.RegisterSyncButton(this);
    }
    public string ButtonId => buttonId;
    public void Interact(playerController player)
    {
        if (!isSolved)
        {
            if (!player.photonView.IsMine) return;
            //PuzzleManager.Instance.OnSyncButtonPressed(this, player);
            PuzzleManager.Instance.RequestPress(
                puzzleId,
                action: buttonId == "A" ? 0 : 1,
                value: 1
            );

        }
     
    }
    public void ResetVisual() 
    {
        if (indicator != null) indicator.color = color;
    }
    public void SetSolved(bool solved)
    {
        if (solved)
        {
            isSolved = true;
            indicator.color = Color.gray;
        }
    }
    public void SetPressedVisual()
    {
        if (indicator) indicator.color = Color.gray;
    }
}
