using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;

public class SettingManager : Singleton<SettingManager>, IWindowUI
{
    #region Field
    [SerializeField] GameObject _root;
    [SerializeField] CanvasGroup _animationTarget;
    [Header("Cursor")]
    [SerializeField] Texture2D _cursorTexture;
    [SerializeField] UnityEngine.CursorMode _cursorMode = UnityEngine.CursorMode.Auto;
    [Header("UI References")]
    [SerializeField] TMP_Dropdown _resolutionDropdown;
    [SerializeField] TMP_Dropdown _screenTypeDropdown;
    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _sfxVolumeSlider;
    [SerializeField] Slider _uiVolumeSlider;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer _audioMixer;

    Resolution[] _resolutions;
    string _settingsFilePath => Path.Combine(Application.persistentDataPath, "settings.json");

    public bool IsActive => _root.activeSelf;
    public WindowUIType WindowType => WindowUIType.Settings;
    #endregion
    #region Settings Logic
    protected override void OnAwake()
    {
        Cursor.SetCursor(_cursorTexture, Vector3.zero, _cursorMode);

        _resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();
        List<string> resText = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            var temp = _resolutions[i];

            string text = $"{temp.width} x {temp.height}";
            resText.Add(text);

            if (temp.width == Screen.currentResolution.width && temp.height == Screen.currentResolution.height)
                _resolutionDropdown.value = i;
        }
        _resolutionDropdown.AddOptions(resText);
        
        if (File.Exists(_settingsFilePath))
            LoadSettings();

        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                _screenTypeDropdown.value = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                _screenTypeDropdown.value = 1;
                break;
            case FullScreenMode.Windowed:
                _screenTypeDropdown.value = 2;
                break;
        }
    }
    public void SetResolution(int index)
    {
        if (index < 0 || index >= _resolutions.Length) return;
        var res = _resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public void SetScreenType(int index)
    {
        switch (index)
        {
            case 0: 
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: 
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: 
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void SetMasterVolume(float volume)
    {
        if (volume == 0)
            _audioMixer.SetFloat("MasterVolume", -80);
        else
            _audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume == 0)
            _audioMixer.SetFloat("MusicVolume", -80);
        else
            _audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume == 0)
            _audioMixer.SetFloat("SFXVolume", -80);
        else
            _audioMixer.SetFloat("SFXVolume", volume);
    }
    
    public void SetUIVolume(float volume)
    {
        if(volume == 0)
            _audioMixer.SetFloat("UIVolume", -80);
        else
            _audioMixer.SetFloat("UIVolume", volume);
    }

    public void SaveSettings()
    {
        var data = new SettingsData
        {
            resolutionIndex = _resolutionDropdown.value,
            screenTypeIndex = _screenTypeDropdown.value,
            masterVolume = _masterVolumeSlider.value,
            musicVolume = _musicVolumeSlider.value,
            sfxVolume = _sfxVolumeSlider.value,
            uiVolume = _uiVolumeSlider.value
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_settingsFilePath, json);
    }

    void LoadSettings()
    {
        string json = File.ReadAllText(_settingsFilePath);
        var data = JsonUtility.FromJson<SettingsData>(json);

        if(data == null)
            SaveSettings();


        _resolutionDropdown.value = data.resolutionIndex;
        _screenTypeDropdown.value = data.screenTypeIndex;
        _masterVolumeSlider.value = data.masterVolume;
        _musicVolumeSlider.value = data.musicVolume;
        _sfxVolumeSlider.value = data.sfxVolume;
        _uiVolumeSlider.value = data.uiVolume;

        _audioMixer.SetFloat("MasterVolume", data.masterVolume);
        _audioMixer.SetFloat("MusicVolume", data.musicVolume);
        _audioMixer.SetFloat("SFXVolume", data.sfxVolume);
        _audioMixer.SetFloat("UIVolume", data.uiVolume);

        SetResolution(data.resolutionIndex);
        SetScreenType(data.screenTypeIndex);
    }
    #endregion
    #region IWindowUI
    public void Open()
    {
        StopAllCoroutines();
        UIAnimationUtil.PlayFadeIn(this, _root, _animationTarget);
    }
    public void Close()
    {
        SaveSettings();
        StopAllCoroutines();
        UIAnimationUtil.PlayFadeOut(this, _root, _animationTarget);
    }
    public void Confirm() 
    {
        SaveSettings();
    }
    public void OnClose()
    {
        AudioManager.Instance.PlayUI(AudioUITable.ButtonClick);
        WindowUIManager.Instance.CloseWindow(this);
    }
    #endregion
}
