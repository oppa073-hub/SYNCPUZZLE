using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MirrorController : MonoBehaviour
{
    public int mirrorIndex;
    public Transform pivot;
    public float stepAngle = 90f;
    private void Start()
    {
        PuzzleManager.Instance.RegisterMirror(mirrorIndex, this);
    }
    public void ApplyStep(int step)
    {
        float angle = step * stepAngle;

        pivot.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void PlayRotateFx()//사운드
    {

    }
}
