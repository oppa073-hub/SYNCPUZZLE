using System.Collections;
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

    [Header("Sync Puzzle Settings")]
    [SerializeField] private float syncWindow = 1.0f; // 몇 초 안에 같이 눌러야 성공?
    [SerializeField] private GameObject door;         // 성공 시 열 문(비활성화)

    private float lastPressTimeA = -999f;
    private float lastPressTimeB = -999f;

    private bool aPressed;
    private bool bPressed;

    private SyncButton buttonARef;
    private SyncButton buttonBRef;

    private Coroutine timeoutCo;

    public void OnSyncButtonPressed(SyncButton button, playerController player)
    {
  

        if (button.ButtonId == "A") buttonARef = button;
       else if (button.ButtonId == "B") buttonBRef = button;

        float now = Time.time;

        if (button.ButtonId == "A")
        {
            Debug.Log("A버튼 눌림");
            aPressed = true;
            lastPressTimeA = now;

        }
        else if (button.ButtonId == "B")
        {
            Debug.Log("B버튼 눌림");
            bPressed = true;
            lastPressTimeB = now;

        }
        if (timeoutCo == null && (aPressed ^ bPressed))
        {
            timeoutCo = StartCoroutine(WaitButtonTimeout());
        }

        TryResolveSync();
    }
    public void TryResolveSync()
    {
        if (!(aPressed && bPressed)) return;
        StopTimeout();
        float diff = Mathf.Abs(lastPressTimeA - lastPressTimeB);
        
        if (diff <= syncWindow)
        {
            if (door != null) door.SetActive(false);
            buttonARef?.SetSolved(true);
            buttonBRef?.SetSolved(true);
        }
        else
        {
            ResetSyncPuzzle();
        }
    }

    private IEnumerator WaitButtonTimeout()
    {
        yield return new WaitForSeconds(syncWindow);

        // 시간이 지났는데 아직 둘 다 안 눌렸으면 리셋
        if (!(aPressed && bPressed))
        { 
            ResetSyncPuzzle();
        }

        // 코루틴 끝났으니 핸들 비우기
        timeoutCo = null;
    }
    private void StopTimeout()
    {
        if (timeoutCo != null)
        {
            StopCoroutine(timeoutCo);
            timeoutCo = null;
        }
    }

    public void ResetSyncPuzzle()
    {
        StopTimeout();
        aPressed = false;
        bPressed = false;
        lastPressTimeA = -999;
        lastPressTimeB = -999;
        buttonARef?.ResetVisual();
        buttonBRef?.ResetVisual();
    }


}
