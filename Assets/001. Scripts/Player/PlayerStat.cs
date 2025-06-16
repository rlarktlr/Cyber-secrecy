using System.Collections;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] Rigidbody _rb;
    bool _isDamageInterval = false;
    [SerializeField] int Strength { get; set; }
    [SerializeField] int Speed { get; set; }
    [SerializeField] int MaxHP { get; set; }
    [SerializeField] int CurrentHP { get; set; }
    [SerializeField] float MaxStamina { get; set; }
    [SerializeField] float CurrentStamina { get; set; }
    [SerializeField] float StaminaRegenRate { get; set; }
    [SerializeField] int DeathCount { get; set; }
    public override float GetStat(StatType type)
    {
        return type switch
        {
            StatType.Strength => Strength,
            StatType.Speed => Speed,
            StatType.MaxHP => MaxHP,
            StatType.CurrentHP => CurrentHP,
            StatType.MaxStamina => MaxStamina,
            StatType.CurrentStamina => CurrentStamina,
            StatType.StaminaRegenRate => StaminaRegenRate,
            _ => 0
        };
    }
    public StatData GetStatData()
    {
        return new StatData
        {
            strength = Strength,
            speed = Speed,
            maxHP = MaxHP,
            currentHP = CurrentHP,
            maxStamina = MaxStamina,
            currentStamina = CurrentStamina,
            staminaRegenRate = StaminaRegenRate,
            deathCount = DeathCount
        };
    }
    public void SetStatData(StatData data)
    {
        Strength  = (int)data.strength;
        Speed = (int)data.speed;
        MaxHP = (int)data.maxHP;
        CurrentHP = (int)data.currentHP;
        MaxStamina = data.maxStamina;
        CurrentStamina = data.currentStamina;
        StaminaRegenRate = data.staminaRegenRate;
    }

    void StaminaTick()
    {
        if (CurrentStamina < MaxStamina)
            CurrentStamina = Mathf.Clamp(CurrentStamina + StaminaRegenRate, 0, MaxStamina);
    }
    public void ConsumeStamina(float value)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - value, 0, MaxStamina);
    }
    public void Heal(int value)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + value, 0, MaxHP);
    }


    public override void Damage(int value)
    {
        if (_isDamageInterval) return;

        //CameraManager.Instance?.Shake();

        CurrentHP = Mathf.Clamp(CurrentHP - value, 0, MaxHP);
        _isDamageInterval = true;
        StartCoroutine(HitEffect());

        if (CurrentHP <= 0)
            Death();
    }
    public override void Death()
    {
        DeathCount++;
        CurrentHP = MaxHP;
        transform.position = Vector3.up;
        GameDataManager.Instance.SaveGameData();
    }
    IEnumerator HitEffect()
    {
        yield return new WaitForSeconds(0.6f);
        _isDamageInterval = false;
    }
}
