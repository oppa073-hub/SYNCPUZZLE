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

    [SerializeField] GameObject KeypadPanel;
    [SerializeField] 
    private void Start()
    {
        KeypadPanel.SetActive(false);
    }

    public void OpenKeyPad()
    {
        KeypadPanel.SetActive(true);
        PuzzleManager.Instance.RequestPress(3, 9, 0);
    }
    public void CloseKeyPad()
    {
        KeypadPanel.SetActive(false);
        PuzzleManager.Instance.RequestPress(3, 8, 0);
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
