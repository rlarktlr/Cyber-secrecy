using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class InputPopup : PopupBase
{
    [Header("Input Fields")]
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] TMP_Text _textPlaceholder;

    void Awake()
    {
        _inputField.onValidateInput += OnTextChanged;
    }

    char OnTextChanged(string text, int charIndex, char addedChar)
    {
        if (char.IsLetterOrDigit(addedChar))
            return char.ToUpper(addedChar);
        return '\0';
    }
    public override WindowUIType WindowType => WindowUIType.InputPopup;

    public void Initialize(string title, string placeholder, Action onClose, Action<string> onConfirm, bool isClosable = true)
    {
        if (!string.IsNullOrEmpty(placeholder))
            _textPlaceholder.text = placeholder;

        _inputField.text = string.Empty;

        _inputField.ActivateInputField();

        base.Initialize(title, true, isClosable, onClose, () => onConfirm?.Invoke(_inputField.text));
    }
}
