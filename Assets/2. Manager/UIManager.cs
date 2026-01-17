using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    /*
     * OpenKeypad(int puzzleId) : 패널 켜고 puzzleId 저장

       CloseKeypad() : 패널 끄기
       
       OnKeypadNumber(int n) : RequestPress(puzzleId, action=0, value=n)
       
       OnKeypadBack() : RequestPress(puzzleId, action=1, value=0)
       
       OnKeypadClear() : RequestPress(puzzleId, action=2, value=0)
       
       OnKeypadEnter() : RequestPress(puzzleId, action=3, value=0)
     * 
     */

    [SerializeField] GameObject KeypadPanel;
    [SerializeField] 
    private void Start()
    {
        KeypadPanel.SetActive(false);
    }

    public void OpenKeyPad()
    {
        KeypadPanel.SetActive(true);
    }
    public void CloseKeyPad()
    {
        KeypadPanel.SetActive(false);
    }
    public void OnKeypadNumber(int n)
    {
        PuzzleManager.Instance.RequestPress(3, 0, n);
    }
    public void OnKeypadEnter()
    {
        PuzzleManager.Instance.RequestPress(3, 1, 0);
    }


}
