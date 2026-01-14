using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

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
    public void OnButtonPressed(TestDoor button, playerController player)
    {
        Debug.Log("버튼 눌림");
        // 여기서 규칙 판단하기
    }
}
