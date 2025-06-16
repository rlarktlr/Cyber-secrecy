using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class PopupBase : MonoBehaviour, IWindowUI
{
    [Header("UI Elements")]
    [SerializeField] protected GameObject _root;

    [SerializeField] protected Transform _animationTarget;
    [SerializeField] protected TMP_Text _textTitle;

    [SerializeField] protected Button _buttonConfirm;
    [SerializeField] protected Button _buttonClose;
    [SerializeField] protected Button _buttonBackdrop;

    protected Action _OnConfirm;
    protected Action _OnClose;

    public abstract WindowUIType WindowType { get; }

    public bool IsActive => _root.activeSelf;

    protected void Initialize(string title, bool showConfirm, bool isClosable, Action onClose, Action onConfirm)
    {
        _textTitle.text = title;
        _OnConfirm = onConfirm;
        _OnClose = onClose;

        _buttonConfirm.gameObject.SetActive(showConfirm);
        _buttonClose.gameObject.SetActive(isClosable);
        _buttonBackdrop.gameObject.SetActive(isClosable || !showConfirm);
    }

    public virtual void Close()
    {
        _OnClose?.Invoke();
        StopAllCoroutines();
        UIAnimationUtil.PlayScaleOut(this, _root, _animationTarget);
    }

    public void Open()
    {
        StopAllCoroutines();
        UIAnimationUtil.PlayScaleIn(this, _root, _animationTarget);
    }

    public virtual void Confirm()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);
        _OnConfirm?.Invoke();
        Close();
    }
    public void OnClose()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);
        WindowUIManager.Instance.CloseWindow(this);
    } 
}

