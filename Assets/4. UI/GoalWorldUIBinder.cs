using TMPro;
using UnityEngine;

public class GoalWorldUIBinder : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text goalText;

    private void Start()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.BindGoalWorldUI(panel, goalText);
    }
}
