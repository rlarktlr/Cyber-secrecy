using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogBoxPopup : PopupBase
{
    [SerializeField] TMP_Text _textMessage;
    [SerializeField] Button _buttonCancel;
    Action _callbackCancel;

    public override WindowUIType WindowType => WindowUIType.DialogPopup;

    public void Initialize(string title, string message, bool showConfirm, bool showCancel, bool isClosable, Action onClose, Action onConfirm = null, Action onCancel = null)
    {
        _textMessage.text = message;
        _buttonCancel.gameObject.SetActive(showCancel);
        
        if (showCancel)
            _callbackCancel = onCancel;

        base.Initialize(title, showConfirm, isClosable, onClose, onConfirm);
    }

    public void OnCancel()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);

        _callbackCancel?.Invoke();
        Close();
    }
    public override void Close()
    {
        base.Close();
        _callbackCancel = null;
    }

}
