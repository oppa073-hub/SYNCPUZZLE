using UnityEngine;
using TMPro;
public class KeypadUI : MonoBehaviour
{
    [SerializeField] TextMeshPro inputPasswordText;

    public void InputPasswordText(string passwordText)
    {
        inputPasswordText.text += passwordText;
    }
}
