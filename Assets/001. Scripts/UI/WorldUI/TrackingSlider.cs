using UnityEngine;
using UnityEngine.UI;

public class TrackingSlider : TrackingUI, IWorldUI
{
    public Slider _slider;
    public WorldUIType Type => WorldUIType.Slider;

    public void SetSliderMaxValue(float value)
    {
        _slider.maxValue = value;
    }
    public void SetSliderValue(float value)
    {
        _slider.value = value;
    }
    public void Activate()
    {
        _slider.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _slider.gameObject.SetActive(false);
    }

    public void Release()
    {
        WorldUIManager.Instance.CloseWindow(this);
    }
}
