using UnityEngine;

public class SyncButton : MonoBehaviour, IInteractable
{
    [SerializeField] string buttonId = "A";
    [SerializeField] SpriteRenderer indicator;
    Color color;
    bool isSolved = false;
    private void Start()
    {
         color = indicator.color;
    }
    public string ButtonId => buttonId;
    public void Interact(playerController player)
    {
        if (!isSolved)
        {
            PuzzleManager.Instance.OnSyncButtonPressed(this, player);

            if (indicator != null)
            {

                indicator.color = Color.gray;
            }
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
}
