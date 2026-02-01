using UnityEngine;

public class LoginBtn : MonoBehaviour
{
    [SerializeField] GameObject LoginUI;
    private void Start()
    {
        LoginUI.SetActive(false);

    }
    public void OnLoginUI()
    {
        LoginUI.SetActive(true);
    }
}
