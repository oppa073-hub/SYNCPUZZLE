using UnityEngine;

public class MirrorPuzzleExitBtn : MonoBehaviour
{
    [SerializeField] private AudioClip exitSfx;
    public void OnClickMirrorPuzzleExitBtn()
    {
        AudioManager.instance.PlaySFX(exitSfx);
        PuzzleModeManager.Instance.ExitPuzzleModel();
        UIManager.Instance.CloseMirrorPuzzleHint();
    }
}
