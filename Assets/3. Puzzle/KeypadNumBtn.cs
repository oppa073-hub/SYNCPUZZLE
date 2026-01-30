using UnityEngine;

public class KeypadNumBtn : MonoBehaviour
{
    [SerializeField] int number = 1;
    public void OnKeypadNumBtnClick()
    {
        UIManager.Instance.OnKeypadNumber(number);
        AudioManager.instance.PlayKeyPad();
        UIManager.Instance.InputPasswordText(number.ToString());
    }
}
