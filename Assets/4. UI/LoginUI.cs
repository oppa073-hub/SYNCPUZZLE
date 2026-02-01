using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private GameObject authRoot;   // Canvas 또는 AuthPanelRoot

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject signUpPanel;

    [SerializeField] private TMP_InputField loginEmailInput;
    [SerializeField] private TMP_InputField loginPasswordInput;

    [SerializeField] private TMP_InputField signUpEmailInput;
    [SerializeField] private TMP_InputField signUpPasswordInput;
    [SerializeField] private TMP_InputField signUpNicknameInput;

    [SerializeField] private TMP_Text resultText;

    [SerializeField] private AudioClip clickSfx;

    private void Start()
    {
        // 게임 시작 시 로그인 패널만 보이게
        ShowLoginPanel();
    }

    public void ShowLoginPanel()
    {
        AudioManager.instance.PlaySFX(clickSfx);
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
        if (resultText) resultText.text = "";
    }

    public void ShowSignUpPanel()
    {
        AudioManager.instance.PlaySFX(clickSfx);
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
        if (resultText) resultText.text = "";
    }

    private void HideAllAuthUI()
    {
        // 로그인 완료하면 로그인 관련 UI 전체 끄기
        if (authRoot != null) authRoot.SetActive(false);
        else
        {
            loginPanel.SetActive(false);
            signUpPanel.SetActive(false);
        }
    }

    private string MakeNicknameForSignUp()
    {
        if (!string.IsNullOrWhiteSpace(signUpNicknameInput.text))
        {
            return signUpNicknameInput.text.Trim();
        }

        var email = signUpEmailInput.text.Trim();
        if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
        {
            return email.Split('@')[0];
        }

        return "Player";
    }

    public async void OnLogin()
    {
        AudioManager.instance.PlaySFX(clickSfx);
        var email = loginEmailInput.text.Trim();
        var pw = loginPasswordInput.text;

        var msg = await FirebaseEmailAuthManager.Instance.Login(email, pw);
        resultText.text = msg;

        if (msg == "로그인 성공")
        {
            // 로그인에서는 닉네임 덮어쓰기 X
            FirebaseEmailAuthManager.Instance.ConnectPhotonAfterLogin();

            // 로그인 UI 전부 끄기
            HideAllAuthUI();
            SceneManager.LoadScene("Lobby");
        }
    }

    public async void OnSignUp()
    {
        AudioManager.instance.PlaySFX(clickSfx);
        var email = signUpEmailInput.text.Trim();
        var pw = signUpPasswordInput.text;
        var nickname = MakeNicknameForSignUp();

        var msg = await FirebaseEmailAuthManager.Instance.SignUp(email, pw);
        resultText.text = msg;

        if (msg == "회원가입 성공")
        {
            await FirebaseEmailAuthManager.Instance.SetDisplayNameAsync(nickname);

            // 회원가입 끝나면 로그인 패널로 복귀
            ShowLoginPanel();
        }
    }

    public void OnClickGoSignUp() => ShowSignUpPanel();   // 회원가입 버튼
    public void OnClickBackToLogin() => ShowLoginPanel(); // 뒤로가기 버튼
}
