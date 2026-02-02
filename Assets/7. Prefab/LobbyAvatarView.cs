using TMPro;
using UnityEngine;

public class LobbyAvatarView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private RuntimeAnimatorController animA;
    [SerializeField] private RuntimeAnimatorController animB;
    [SerializeField] private TMP_Text nickText;

    public void Set(string nickname, PlayerRole role)
    {
        if (nickText) nickText.text = nickname;

        if (animator == null) animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = (role == PlayerRole.A) ? animA : animB;

        animator.Play("Idle", 0, 0f);

        if (body) body.flipX = false;
    }
}
