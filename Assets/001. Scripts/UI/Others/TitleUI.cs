using System.IO;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void OnPlay()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);

        if(GameDataManager.Instance.LoadGame("ABC"))
            GameDataManager.Instance.CreateNewGame("ABC");
    }
    public void OnSettings()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);

        WindowUIManager.Instance.OpenSettings();
    }
    public void OnQuit()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);

        WindowUIManager.Instance?.OpenDialogBox("Quit Game", "Are you sure you want to quit?", true, true, true, () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
