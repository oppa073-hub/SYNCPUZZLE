using UnityEngine;

public class StageBGM : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    private void Start()
    {
        AudioManager.instance.PlayBGM(bgm);
    }
}
