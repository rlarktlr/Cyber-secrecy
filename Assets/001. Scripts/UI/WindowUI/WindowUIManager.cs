using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class WindowUIManager : SingletonDontDestroy<WindowUIManager>
{
    public bool IsUIOpen;

    [SerializeField] float _clickDelay;
    float _lastOpenTime;


    [Header("Window Table")]
    [SerializeField] List<WindowInfo> windowTable;

    Dictionary<WindowUIType, Queue<IWindowUI>> _pools;
    List<IWindowUI> _zOrder;

    protected override void OnAwake()
    {
        _pools = new();
        _zOrder = new();

        foreach (var entry in windowTable)
            _pools[entry.type] = new();

        PrewarmUI(WindowUIType.DataSlot);
        PrewarmUI(WindowUIType.Settings);
    }

    void PrewarmUI(WindowUIType type)
    {
        if (!_pools.ContainsKey(type))
            _pools[type] = new Queue<IWindowUI>();

        if (_pools[type].Count > 0)
            return;
        var prefab = windowTable.Find(e => e.type == type).prefab;
        var instance = Instantiate(prefab, transform);
        var win = instance.GetComponent<IWindowUI>();

        win.Close();
        _pools[type].Enqueue(win);
    }

    #region Open Window
    public void OpenDataSlot() => OpenWindowSafe(WindowUIType.DataSlot);
    public void OpenSettings() => OpenWindowSafe(WindowUIType.Settings);
    public void OpenPaused() => OpenWindowSafe(WindowUIType.Paused);
    public void OpenError(string message, Action onConfirm = null)
    {
        var win = OpenWindowSafe(WindowUIType.DialogPopup) as DialogBoxPopup;
        win.Initialize(
            title: "ERROR",
            message: message,
            showConfirm: true,
            showCancel: false,
            isClosable: false,
            null,
            onConfirm,
            null
        );
    }

    public void OpenDialogBox(string title, string message, bool showConfirm = true, bool showCancel = false, bool isClosable = false,
        Action onConfirm = null, Action onCancel = null)
    {
        var win = OpenWindowSafe(WindowUIType.DialogPopup) as DialogBoxPopup;
        win.Initialize(
            title,
            message,
            showConfirm,
            showCancel,
            isClosable,
            null,
            onConfirm,
            onCancel
        );
    }

    public void OpenInput(string title, string placeholder = "", Action<string> onConfirm = null, bool isClosable = true)
    {
        var win = OpenWindowSafe(WindowUIType.InputPopup) as InputPopup;
        win.Initialize(
            title,
            placeholder,
            null,
            onConfirm,
            isClosable
        );
    }
    public void OpenInventory() => OpenWindowSafe(WindowUIType.Inventory);
    #endregion
    #region Main Logic
    IWindowUI OpenWindowSafe(WindowUIType type)
    {
        if (Time.time - _lastOpenTime < _clickDelay)
            return null;

        IsUIOpen = true;

        _lastOpenTime = Time.time;

        IWindowUI win;
        if (_pools[type].Count > 0)
            win = _pools[type].Dequeue();
        else
        {
            var prefab = windowTable.Find(e => e.type == type).prefab;
            var instance = Instantiate(prefab, transform);
            win = instance.GetComponent<IWindowUI>();
        }

        _zOrder.Add(win);
        win.Open();
        return win;
    }
    public void CloseWindow(IWindowUI instance)
    {
        if (instance == null) return;
        instance.Close();
        _pools[instance.WindowType].Enqueue(instance);
        _zOrder.Remove(instance);

        if (_zOrder.Count == 0)
            IsUIOpen = false;
    }
    public void CloseAllWindows()
    {
        var openWindows = new List<IWindowUI>(_zOrder);
        foreach (var win in openWindows)
            CloseWindow(win);
    }
    #endregion
    #region Input
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            OnTab();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnEsc();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            OnEnter();
    }
    void OnEnter()
    {
        if (_zOrder.Count == 0) return;
        _zOrder[^1].Confirm();
    }
    void OnEsc()
    {
        AudioManager.Instance?.PlayUI(AudioUITable.ButtonClick);

        if (_zOrder.Count == 0)
            OpenPaused();
        else
            CloseWindow(_zOrder[^1]);
    }
    void OnTab()
    {
        bool isInventoryOpen = _zOrder.Any(w => w.WindowType == WindowUIType.Inventory);
        if (isInventoryOpen)
            return;

        OpenInventory();
    }
    #endregion
}
