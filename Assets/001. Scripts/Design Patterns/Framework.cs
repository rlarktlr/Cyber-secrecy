using System;
using UnityEngine;

#region Data
[Serializable]
public class SettingsData
{
    public int resolutionIndex;
    public int screenTypeIndex;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float uiVolume;
}
[Serializable]
public class GameData
{
    public string gameTitle;
    public int lastSceneIndex;
    public int lastRoomIndex;

    public StatData Stat;
}
public enum StatType
{
    Speed,
    Strength,
    MaxHP,
    CurrentHP,
    Level,
    MaxExp,
    CurrentExp,
    MaxStamina,
    CurrentStamina,
    StaminaRegenRate,
    MaxNumberOfItems
}
[Serializable]
public struct StatData
{
    public float strength;
    public float speed;

    public float maxHP;
    public float currentHP;

    public float maxStamina;
    public float currentStamina;
    public float staminaRegenRate;
    public int deathCount;
}
public abstract class Stat : MonoBehaviour
{
    public abstract float GetStat(StatType type);
    public abstract void Damage(int amount);
    public abstract void Death();
}
#endregion

#region UI
#region World UI
[Serializable]
public struct WorldUIInfo
{
    public WorldUIType type;
    public GameObject prefab;
}
public interface IWorldUI
{
    WorldUIType Type { get; }
    void Activate();
    void Deactivate();
    void Release();
}
public enum WorldUIType
{
    Slider,
    StatValueDisplay,
}
#endregion
#region Window UI
[Serializable]
public struct WindowInfo
{
    public WindowUIType type;
    public GameObject prefab;
}
public interface IWindowUI
{
    WindowUIType WindowType { get; }
    bool IsActive { get; }
    void Open();
    void Close();
    void Confirm();
    void OnClose();
}
public enum WindowUIType
{
    DialogPopup,
    InputPopup,
    Settings,
    DataSlot,
    Paused,
    TrackingSlider,
    NetworkTrackingSlider,
    PlayerInterface,
    Inventory
}
#endregion
#endregion

#region Sound
public enum AudioMusicTable
{
    BackGoundMusic1,
    BackGoundMusic2,
    BackGoundMusic3
}
public enum AudioUITable
{
    Slider,
    ButtonClick,
    ButtonHighlighted
}
public enum AudioSFXTable
{
    WalkSFX,
    JumpSFX,
    BulletFireSFX,
    BulletImpactSFX
}
[Serializable]
public struct MusicClipData
{
    public AudioMusicTable type;
    public AudioClip clip;
}
[Serializable]
public struct SFXClipData
{
    public AudioSFXTable type;
    public AudioClip clip;
}
[Serializable]
public struct UIClipData
{
    public AudioUITable type;
    public AudioClip clip;
}
#endregion
public interface IInteractable
{
    public void Interact();
}
public interface IPuzzle
{
    public void ResetPuzzle();
    public void PuzzleStart();
    public bool IsPuzzleDone();
}

[Serializable]
public struct SpawnData
{
    public Vector3 spawnPoint;
    public GameObject prefab;
}
static class MyUtility
{
    public static Vector3 CalculateProjectileVelocity(Vector3 start, Vector3 end, float height)
    {
        Vector3 displacementXZ = new Vector3(end.x - start.x, 0f, end.z - start.z);
        float displacementY = end.y - start.y;
        float gravity = Mathf.Abs(Physics.gravity.y);

        height = Mathf.Max(height, 0.1f);

        float timeUp = Mathf.Sqrt(2f * height / gravity);
        float timeDown = Mathf.Sqrt(2f * Mathf.Max(height - displacementY, 0.1f) / gravity);
        float totalTime = timeUp + timeDown;

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(2f * gravity * height);
        Vector3 velocityXZ = displacementXZ / totalTime;

        return velocityXZ + velocityY;
    }
    public static readonly string[] SceneByIndex = {
        "Title", 
        "SampleScene",
    };
}
public abstract class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;
    public string itemName;
    public int itemID;

    public abstract void Use(SlotUI inventorySlot);
}
[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Armor")]
public class ArmorItem : Item
{
    [SerializeField] float hp;
    [SerializeField] float durability;
    public override void Use(SlotUI slot)
    {
        slot.EquipArmor();
    }
}

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class PotionItem : Item
{
    [SerializeField] int healthAmount;
    public override void Use(SlotUI slot)
    {
        slot.inventory.playerStatus.Heal(healthAmount);
        Destroy(slot.transform.GetChild(0).gameObject);
    }
}
public enum SlotTag { Head, Chest, Legs, Feet, Weapon, None }