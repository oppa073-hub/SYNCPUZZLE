using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class FirebaseEmailAuthManager : MonoBehaviour
{
    public static FirebaseEmailAuthManager Instance;

    private FirebaseAuth auth;
    private FirebaseUser user;

    private async void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        await InitFirebase();
    }

    private async Task InitFirebase()
    {
        var status = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (status != DependencyStatus.Available)
        {
            Debug.LogError($"Firebase error: {status}");
            return;
        }

        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnAuthStateChanged;
        OnAuthStateChanged(this, null);
    }

    private void OnAuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
            Debug.Log(user != null ? $"로그인 상태: {user.Email}" : "로그아웃 상태");
        }
    }

    // 회원가입
    public async Task<string> SignUp(string email, string password)
    {
        try
        {
            await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            return "회원가입 성공";
        }
        catch (Exception e)
        {
            return ParseError(e);
        }
    }

    // 로그인
    public async Task<string> Login(string email, string password)
    {
        try
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            return "로그인 성공";
        }
        catch (Exception e)
        {
            return ParseError(e);
        }
    }

    public void Logout()
    {
        auth.SignOut();
    }
    public async Task SetDisplayNameAsync(string nickname)
    {
        if (auth.CurrentUser == null) return;

        var profile = new UserProfile { DisplayName = nickname };
        await auth.CurrentUser.UpdateUserProfileAsync(profile);

        // 최신값 반영
        await auth.CurrentUser.ReloadAsync();
    }

    public void ConnectPhotonAfterLogin(string nicknameFromUI = null)
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("Firebase 로그인 상태가 아닌데 Photon 연결을 시도함");
            return;
        }

        var u = auth.CurrentUser;

        // UI에서 받은 닉네임 우선
        string nickname = !string.IsNullOrWhiteSpace(nicknameFromUI)
            ? nicknameFromUI
            : (string.IsNullOrEmpty(u.DisplayName)
                ? ((u.Email != null && u.Email.Contains("@")) ? u.Email.Split('@')[0] : "Player")
                : u.DisplayName);

        PhotonNetwork.AuthValues = new AuthenticationValues(u.UserId);
        PhotonNetwork.NickName = nickname;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log($"Photon 연결 시도: Nick={PhotonNetwork.NickName}, UID={u.UserId}");
        }
    }

    private string ParseError(Exception e)
    {
        var msg = e.Message;
        if (msg.Contains("EMAIL_EXISTS")) return "이미 가입된 이메일";
        if (msg.Contains("INVALID_EMAIL")) return "이메일 형식 오류";
        if (msg.Contains("WEAK_PASSWORD")) return "비밀번호 6자 이상";
        if (msg.Contains("WRONG_PASSWORD")) return "비밀번호 틀림";
        if (msg.Contains("USER_NOT_FOUND")) return "계정 없음";
        return "로그인 false";
    }
}
