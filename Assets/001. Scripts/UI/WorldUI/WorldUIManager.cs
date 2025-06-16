using UnityEngine;
using System.Collections.Generic;

public class WorldUIManager : SingletonDontDestroy<WorldUIManager>
{
    [Header("Window Table")]
    [SerializeField] List<WorldUIInfo> windowTable;
    Dictionary<WorldUIType, Queue<IWorldUI>> _pools;

    protected override void OnAwake()
    {
        _pools = new();

        foreach (var entry in windowTable)
            _pools[entry.type] = new();
    }

    #region Open Window
    public StatValueDisplayUI GetHealthBar(Stat stat, Transform target = null)
    {
        var ui = GetUI(WorldUIType.StatValueDisplay) as StatValueDisplayUI;
        ui.Initialize(stat, target);
        return ui;
    }
    public TrackingSlider GetTrackingSliderUI(Transform target, float maxValue)
    {
        var ui = GetUI(WorldUIType.Slider) as TrackingSlider;
        ui.SetTrack(target);
        ui.SetSliderMaxValue(maxValue);
        return ui;
    }
    #endregion
    #region Main Logic
    IWorldUI GetUI(WorldUIType type)
    {
        IWorldUI win;
        if (_pools[type].Count > 0)
            win = _pools[type].Dequeue();
        else
        {
            var prefab = windowTable.Find(e => e.type == type).prefab;
            var instance = Instantiate(prefab, transform);
            win = instance.GetComponent<IWorldUI>();
        }

        return win;
    }
    public void CloseWindow(IWorldUI instance)
    {
        instance.Deactivate();
        _pools[instance.Type].Enqueue(instance);
    }
    #endregion
}
