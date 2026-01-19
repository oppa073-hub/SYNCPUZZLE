using UnityEngine;
using TMPro;
public class KeypadUI : MonoBehaviour
{
    [SerializeField] TMP_Text inputPasswordText;

    public void InputPasswordText(string passwordText)
    {
        inputPasswordText.text += passwordText;
    }
}
