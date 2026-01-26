using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    int currentPuzzleId = -1;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField] GameObject KeypadPanel;
    [SerializeField] TMP_Text inputPasswordText;
    [SerializeField] Image inputTextPanel;
    [SerializeField] GameObject goalTextPanel;
    [SerializeField] TMP_Text goalText;

    private void Start()
    {
        KeypadPanel.SetActive(false);
        mirrorPuzzleHintPanel.SetActive(false);
        goalTextPanel.SetActive(false);
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

    public void UpdateGoalUI(Dictionary<int, bool> playersInGoal)
    {
        int count = 0;
        int total = PhotonNetwork.CurrentRoom.PlayerCount;

        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (playersInGoal.TryGetValue(p.ActorNumber, out bool inGoal) && inGoal)
                count++;
        }

        goalTextPanel.SetActive(count > 0);
        goalText.text = $"{count}/{total}";
    }
}
