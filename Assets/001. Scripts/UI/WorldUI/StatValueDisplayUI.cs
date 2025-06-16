using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatValueDisplayUI : TrackingUI, IWorldUI
{
    #region FIELD
    const float LERP_SPEED = 0.2f;

    [Header("Value")]
    [SerializeField] Stat _stat;
    [SerializeField] StatType _maxValue;
    [SerializeField] StatType _currentValue;

    [Header("Components")]
    [SerializeField] TMP_Text _text;
    [SerializeField] Slider _slider;
    [SerializeField] Slider _easeSlider;

    public WorldUIType Type => WorldUIType.StatValueDisplay;

    #endregion
    #region Logic
    void OnDisable() => Ticker.OnTick10Hz -= UpdateUITick;

    public void Initialize(Stat stat, Transform target = null)
    {
        _stat = stat;
        Ticker.OnTick10Hz += UpdateUITick;
        if (target != null)
            SetTrack(target);

        _slider.gameObject.SetActive(true);
    }
    void UpdateUITick()
    {
        if (_stat == null)
        {
            Release();
            return;
        }


        float currentCache = _stat.GetStat(_currentValue);
        float maxCache = _stat.GetStat(_maxValue);

        if (_slider.maxValue != maxCache)
        {
            _slider.maxValue = maxCache;
            if (_easeSlider != null)
                _easeSlider.maxValue = maxCache;
        }

        _slider.value = currentCache;


        if (_easeSlider != null)
            _easeSlider.value = Mathf.Lerp(_easeSlider.value, currentCache, LERP_SPEED);

        if (_text != null)
            _text.text = currentCache.ToString();
    }

    public void Activate() { }

    public void Deactivate() { }

    public void Release()
    {
        Ticker.OnTick30Hz -= Track;
        Ticker.OnTick10Hz -= UpdateUITick;
        _slider.gameObject.SetActive(false);
        WorldUIManager.Instance.CloseWindow(this);
    }
    #endregion
}
