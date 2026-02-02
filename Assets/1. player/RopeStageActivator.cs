using UnityEngine;
using UnityEngine.SceneManagement;

public class RopeStageActivator : MonoBehaviour
{
    private RopeConstraint2D rope;

    private void Awake()
    {
        rope = GetComponent<RopeConstraint2D>();
    }

    private void Start()
    {
        if (rope == null) return;

        // Stage4일 때만 로프 활성화
        if (SceneManager.GetActiveScene().name == "Stage4")
        {
            rope.enabled = true;
        }
        else
        {
            rope.enabled = false;
        }
    }
}
