using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    int currentPuzzleId = -1;
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] GameObject KeypadPanel;
    [SerializeField] TMP_Text inputPasswordText;
    [SerializeField] Image inputTextPanel;
    [SerializeField] GameObject goalTextPanel;
    [SerializeField] TMP_Text goalText;
    [SerializeField] private AudioClip KeypadSfx;
    [SerializeField] private GameObject leavePanel;
    [SerializeField] private TMP_Text leaveText;
    Button KeypadenterBtn;
    Button KeypadexitBtn;
    private Coroutine leaveCo;
    private void Start()
    {
        KeypadPanel.SetActive(false);
        mirrorPuzzleHintPanel.SetActive(false);
        BindKeypadButtons();
       if (goalTextPanel != null) goalTextPanel.SetActive(false);
        if (leavePanel != null) leavePanel.SetActive(false);
    }
    #region keyPad
    public void OpenKeyPad(int puzzleId)
    {
        if (currentPuzzleId != -1) return;
        PuzzleManager.Instance.RequestPress(puzzleId, 9, 0);
        currentPuzzleId = puzzleId;
        inputPasswordText.text = "";
        KeypadPanel.SetActive(true);
    }
    public void CloseKeyPad()
    {
        if (currentPuzzleId == -1) return;
        AudioManager.instance.PlaySFX(KeypadSfx);
        PuzzleManager.Instance.RequestPress(currentPuzzleId, 8, 0);
        KeypadPanel.SetActive(false); 
        currentPuzzleId = -1;
    }
    public void OnKeypadNumber(int n)
    {
        if (currentPuzzleId == -1) return;
        PuzzleManager.Instance.RequestPress(currentPuzzleId, 0, n);
    }
    public void OnKeypadEnter()
    {
        if (currentPuzzleId == -1) return;
        AudioManager.instance.PlaySFX(KeypadSfx);
        PuzzleManager.Instance.RequestPress(currentPuzzleId, 1, 0);
        inputPasswordText.text = "";
    }
    public void InputPasswordText(string passwordText)
    {
        if (currentPuzzleId == -1) return;
        if (inputPasswordText.text.Length >= 4) return;
        inputPasswordText.text += passwordText;
    }
    public void OnKeypadFail()
    {
        StartCoroutine(KeypadFailColor());
    }
    IEnumerator KeypadFailColor()
    {
        Debug.Log("FailColor 시작");
        Color color = inputTextPanel.color;
        inputTextPanel.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        inputTextPanel.color = color;
    }
    #endregion

    #region mirror
    [SerializeField] private GameObject mirrorPuzzleHintPanel;
    public void OpenMirrorPuzzleHint()
    {
        mirrorPuzzleHintPanel.SetActive(true);
    }
    public void CloseMirrorPuzzleHint()
    {
        mirrorPuzzleHintPanel.SetActive(false);
    }
    #endregion

    public void UpdateGoalUI(int count, int total)
    {
        if (goalTextPanel == null) return;
        goalTextPanel.SetActive(count > 0);
        goalText.text = $"{count}/{total}";
    }
    public void BindGoalWorldUI(GameObject panel, TMP_Text text)
    {
        goalTextPanel = panel;
        goalText = text;

        if (goalTextPanel) goalTextPanel.SetActive(false);
    }
    void BindKeypadButtons()
    {
        if (!KeypadPanel) return;

        KeypadenterBtn = KeypadPanel.transform.Find("EnterButton")?.GetComponent<Button>();

        KeypadexitBtn = KeypadPanel.transform.Find("ExitButton")?.GetComponent<Button>();

        if (KeypadenterBtn != null)
        {
            KeypadenterBtn.onClick.RemoveAllListeners();
            KeypadenterBtn.onClick.AddListener(OnKeypadEnter);
        }

        if (KeypadexitBtn != null)
        {
            KeypadexitBtn.onClick.RemoveAllListeners();
            KeypadexitBtn.onClick.AddListener(CloseKeyPad);
        }
    }

    public void ShowLeaveAndReturn(string nick)
    {
        // 중복 호출 방지
        if (leaveCo != null) StopCoroutine(leaveCo);
        leaveCo = StartCoroutine(CoShowLeaveAndReturn(nick));
    }
    private IEnumerator CoShowLeaveAndReturn(string nick)
    {
        if (leaveText) leaveText.text = $"{nick} 님이 나갔습니다.\n로비로 이동합니다...";
        if (leavePanel) leavePanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom(); 
        else
            SceneManager.LoadScene("Lobby");
    }
    public void HideLeaveUI()
    {
        if (leaveCo != null) StopCoroutine(leaveCo);
        leaveCo = null;

        if (leavePanel) leavePanel.SetActive(false);
    }
}
