using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text _titleText;
    [SerializeField] Button _loadButton;
    [SerializeField] Button _deleteButton;

    public void Initialize(GameData gameData)
    {
        string title = gameData.gameTitle;

        _titleText.text = title;

        _loadButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);
            GameDataManager.Instance.LoadGame(title);
        });
        _deleteButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);
            WindowUIManager.Instance.OpenDialogBox("Delete Game", $"Are you sure you want to\ndelete '{title}'?", showCancel: true,isClosable: true,onConfirm:
            () =>
            {
                GameDataManager.Instance.DeleteGame(title);
            });
        });
    }
}
