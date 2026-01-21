using UnityEngine;

public class MirrorPuzzleExitBtn : MonoBehaviour
{
    public void OnClickMirrorPuzzleExitBtn()
    {
        PuzzleModeManager.Instance.ExitPuzzleModel();
        UIManager.Instance.CloseMirrorPuzzleHint();
    }
}
