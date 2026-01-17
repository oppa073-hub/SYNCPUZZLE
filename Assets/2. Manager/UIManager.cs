using System;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] TextMeshPro inputPasswordText;
    private void Start()
    {
        KeypadPanel.SetActive(false);
    }

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


}
