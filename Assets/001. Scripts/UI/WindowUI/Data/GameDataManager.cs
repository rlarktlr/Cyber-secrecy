using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : Singleton<GameDataManager>, IWindowUI
{
    #region Field
    public GameData CurrentData { get; private set; }
    string _savePath;

    [SerializeField] GameObject _root;
    [SerializeField] Transform _aniamtionTarget;
    [SerializeField] Transform _slotParent;
    [SerializeField] GameObject _slotPrefab;

    public WindowUIType WindowType => WindowUIType.DataSlot;
    public bool IsActive => _root.activeSelf;
    #endregion
    #region IWindowUI
    public void Open()
    {
        LoadData();
        StopAllCoroutines();
        UIAnimationUtil.PlayScaleIn(this, _root, _aniamtionTarget);
    }
    public void Close()
    {
        StopAllCoroutines();
        UIAnimationUtil.PlayScaleOut(this, _root, _aniamtionTarget);
    }
    public void Confirm() { }

    public void OnClose()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);
        WindowUIManager.Instance.CloseWindow(this);
    }
    public void OnNewGame()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);

        WindowUIManager.Instance?.OpenInput("New Game Name", "Enter Name...",
        (name) =>
        {
            if (!string.IsNullOrEmpty(name))
            {
                CreateNewGame(name);
                LoadData();
            }

            else
                WindowUIManager.Instance?.OpenError("Invalid Game Name");
        });
    }
    void LoadData()
    {
        foreach (Transform child in _slotParent)
            Destroy(child.gameObject);

        foreach (var file in Directory.GetFiles(Application.persistentDataPath, "*.json"))
        {
            string json = File.ReadAllText(file);
            var data = JsonUtility.FromJson<GameData>(json);

            if (data == null || string.IsNullOrEmpty(data.gameTitle)) continue;

            GameObject slot = Instantiate(_slotPrefab, _slotParent);
            slot.GetComponent<DataSlotUI>().Initialize(data);
        }
    }
    #endregion
    #region Data Logic
    public void CreateNewGame(string title)
    {
        if (File.Exists(GetFilePath(title)))
        {
            WindowUIManager.Instance?.OpenError("Game Already Exists");
            return;
        }
        var data = new GameData
        {
            gameTitle = title,
            lastSceneIndex = 1,
            lastRoomIndex = -1,

            Stat = new StatData
            {
                strength = 0,
                speed = 10,
                maxHP = 100,
                currentHP = 100,
                maxStamina = 100,
                currentStamina = 100,
                staminaRegenRate = 100,
                deathCount = 0,
            },
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetFilePath(title), json);
    }
    public bool LoadGame(string gameTitle)
    {
        string fullPath = GetFilePath(gameTitle);

        if (!File.Exists(fullPath))
            return false;
        else
        {
            string json = File.ReadAllText(fullPath);
            CurrentData = JsonUtility.FromJson<GameData>(json);
            _savePath = fullPath;
            return true;
        }
    }
    public void SaveGameData()
    {
        if (CurrentData == null)
            return;

        CurrentData.Stat = PlayerController.Instance.GetComponent<PlayerStat>().GetStatData();

        string json = JsonUtility.ToJson(CurrentData);
        File.WriteAllText(_savePath, json);
    }
    public void DeleteGame(string title)
    {
        var path = GetFilePath(title);
        if (File.Exists(path))
        {
            File.Delete(path);
            LoadData();
        }
    }

    #endregion
    #region Utils
    public string GetFilePath(string title) => Path.Combine(Application.persistentDataPath, $"{HashTitle(title)}.json");
    public string HashTitle(string title)
    {
        var sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(title);
        byte[] hash = sha.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var b in hash) sb.Append(b.ToString("x2"));
        return sb.ToString()[..12];
    }
    #endregion
}
