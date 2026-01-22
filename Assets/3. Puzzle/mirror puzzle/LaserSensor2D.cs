using UnityEngine;

public class LaserSensor2D : MonoBehaviour
{
    public int sensorIndex;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color offColor = Color.gray;
    [SerializeField] private Color onColor = Color.green;
    int puzzleId = 5;
    private bool litThisFrame;
    private bool isLit;
    private bool wasLit;
    private void Awake()
    {
        SetVisual(false);
    }

    private void LateUpdate()
    {
        isLit = litThisFrame; // 프레임 종료 시점에 최종 상태 확정
        if (isLit == true && wasLit == false)
        {
            PuzzleManager.Instance.RequestPress(puzzleId, 1, sensorIndex);
        }
        SetVisual(isLit);
        wasLit = isLit;
        litThisFrame = false;
    }

    public void MarkLitThisFrame()
    {
        litThisFrame = true;

    }
    private void SetVisual(bool on)
    {
        if (sr)
        {
            sr.color = on ? onColor : offColor;
        }
     
    }
}
