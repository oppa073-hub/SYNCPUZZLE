using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.IK;

public class PuzzleManager : MonoBehaviourPun
{
    #region Singleton
    public static PuzzleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    #region Inspector - Puzzle Targets
    [Header("Puzzle 1 (Single Door)")]
    [SerializeField] private GameObject door1;

    [Header("Puzzle 2 (Sync Buttons)")]
    [SerializeField] private float syncWindow = 1.0f; // 몇 초 안에 같이 눌러야 성공?
    [SerializeField] private GameObject door2;         // 성공 시 열 문(비활성화)

    [Header("Puzzle 3 (Password)")]
    string password = "2580";
    string inputPassword = "";
    int maxLen = 4;
    bool passwordSolved = false;
    int keypadOwnerActor = -1;
    [SerializeField] private GameObject blocker;
    #endregion

    #region Sync Puzzle Runtime State
    private float lastPressTimeA = -999f;
    private float lastPressTimeB = -999f;

    private bool aPressed;
    private bool bPressed;

    private SyncButton buttonARef;
    private SyncButton buttonBRef;

    // 아래 테스트 로컬 방식에서 쓰던 코루틴 핸들
    private Coroutine timeoutCo;
    #endregion

    #region Public API - Called by Puzzle Objects
    // 버튼/레버/퍼즐이 눌렸을 때 요청 (클라에서 호출)
    public void RequestPress(int puzzleId, int action, int value)
    {
        photonView.RPC(nameof(RPC_RequestPress), RpcTarget.MasterClient, puzzleId, action, value);
    }

    // SyncButton이 Start에서 등록하는 용도
    public void RegisterSyncButton(SyncButton b)
    {
        if (b.ButtonId == "A") buttonARef = b;
        else if (b.ButtonId == "B") buttonBRef = b;
    }

    #endregion

    #region RPC - Master Judge
    [PunRPC] // 마스터만 받음: 여기서 판정/상태변경 후 결과를 뿌림
    void RPC_RequestPress(int puzzleId, int action, int value, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // Puzzle 1 : 단일 문 열기
        if (puzzleId == 1)
        {
            photonView.RPC(nameof(RPC_ApplyResult), RpcTarget.All, puzzleId, true);
            return;
        }

        // Puzzle 2 : 동시 버튼
        if (puzzleId == 2)
        {
            float now = (float)PhotonNetwork.Time;

            if (action == 0) { aPressed = true; lastPressTimeA = now; }
            else if (action == 1) { bPressed = true; lastPressTimeB = now; }

            photonView.RPC(nameof(RPC_ApplyPressedVisual), RpcTarget.All, puzzleId, action);

            if (aPressed && bPressed)
            {
                float diff = Mathf.Abs(lastPressTimeA - lastPressTimeB);
                bool solved = diff <= syncWindow;

               
                photonView.RPC(nameof(RPC_ApplyResult), RpcTarget.All, puzzleId, solved);

                if (!solved)
                {
                    aPressed = bPressed = false;
                    lastPressTimeA = lastPressTimeB = -999f;
                }
            }
        }
        if (puzzleId == 3)
        {
            if (passwordSolved == true) return;
            if (action == 9)
            {
                if (keypadOwnerActor == -1)
                {
                    keypadOwnerActor = info.Sender.ActorNumber;
                    inputPassword = "";
                }
            }
            if (action == 8)
            {
                if (info.Sender.ActorNumber == keypadOwnerActor)
                {
                    keypadOwnerActor = -1;
                    inputPassword = "";
                }
            }
            if (action == 0)
            {
                if (info.Sender.ActorNumber != keypadOwnerActor) return;
                if (inputPassword.Length < maxLen)
                {
                    inputPassword += value.ToString();
                }
            }
            if (action == 1)
            {
                if (info.Sender.ActorNumber != keypadOwnerActor) return;
                if (inputPassword == password)
                {
                    passwordSolved = true;
                    keypadOwnerActor = -1;
                    inputPassword = "";
                    photonView.RPC(nameof(RPC_ApplyResult), RpcTarget.All, puzzleId, passwordSolved);
      
                }
                else
                {
                    passwordSolved = false;
                    inputPassword = "";
         
                }
            }
          
        }
    }
    #endregion

    #region RPC - All Apply (Door/Visual)
    [PunRPC] // 모두가 실행: 실제 문열기/애니/비주얼 적용
    void RPC_ApplyResult(int puzzleId, bool solved)
    {

        // Puzzle 1
        if (puzzleId == 1)
        {
            if (solved && door1) door1.SetActive(false);
            return;
        }

        // Puzzle 2
        if (puzzleId == 2)
        {
            if (solved)
            {
                if (door2) door2.SetActive(false);
                buttonARef?.SetSolved(true);
                buttonBRef?.SetSolved(true);
            }
            else
            {
                // 실패면 둘 다 리셋 비주얼
                buttonARef?.ResetVisual();
                buttonBRef?.ResetVisual();
            }
        }
        if (puzzleId == 3)
        {
            if (solved)
            {
                UIManager.Instance.CloseKeyPad();
                blocker.SetActive(false);
            }
        }
    }

    [PunRPC]
    void RPC_ApplyPressedVisual(int puzzleId, int action)
    {
        if (puzzleId != 2) return;

        if (action == 0) buttonARef?.SetPressedVisual();
        if (action == 1) buttonBRef?.SetPressedVisual();
    }
    #endregion

    #region 테스트 Local Sync Logic (현재 네트워크 방식에선 사용 안 함)
    // 아래는 예전에 로컬에서만 동시 버튼 판정하던 코드.
    // 지금은 RequestPress -> RPC_RequestPress(마스터판정) 흐름을 쓰고 있어서

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
            if (door2 != null) door2.SetActive(false);
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

        if (!(aPressed && bPressed))
        {
            ResetSyncPuzzle();
        }

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
    #endregion
}
