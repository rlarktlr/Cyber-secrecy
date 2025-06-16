using UnityEngine;

public class PausedUI : Singleton<PausedUI>, IWindowUI
{
    [SerializeField] GameObject _root;
    [SerializeField] RectTransform _animationRoot;
    public WindowUIType WindowType => WindowUIType.Paused;
    public bool IsActive => _root.activeSelf;


    public void OnSettings()
    {
        AudioManager.Instance?.PlayUI(AudioUITable.ButtonClick);
        WindowUIManager.Instance?.CloseWindow(Instance);
        WindowUIManager.Instance?.OpenSettings();
    }
    public void OnQuit()
    {
        AudioManager.Instance?.PlayUI(AudioUITable.ButtonClick);

        WindowUIManager.Instance?.CloseWindow(Instance);

        WindowUIManager.Instance?.OpenDialogBox("Go To Title", "Are you sure?", true, true, true, () =>
        {
            Ticker.Instance.IsPlaying = false;
            LoadingSceneManager.Instance.LoadScene(MyUtility.SceneByIndex[0]);
        });
    }
    public void OnClose()
    {
        AudioManager.Instance?.PlayUI(AudioUITable.ButtonClick);
        WindowUIManager.Instance?.CloseWindow(Instance);
    }
    public void Open()
    {
        StopAllCoroutines();
        UIAnimationUtil.PlayScaleIn(this, _root, _animationRoot);
    }

    public void Close()
    {
        StopAllCoroutines();
        UIAnimationUtil.PlayScaleOut(this, _root, _animationRoot);
    }

    public void Confirm()
    {
        OnClose();
    }
}
