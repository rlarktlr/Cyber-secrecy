using System.Collections;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] Rigidbody _rb;
    bool _isDamageInterval = false;
    [SerializeField] int _strength;
    [SerializeField] int _speed;
    [SerializeField] int _maxHP;
    [SerializeField] int _currentHP;
    [SerializeField] float _maxEP;
    [SerializeField] float _currentEP;
    [SerializeField] float _regenRateEP;
    [SerializeField] int _deathCount;

    public override float GetStat(StatType type)
    {
        return type switch
        {
            StatType.Strength => _strength,
            StatType.Speed => _speed,
            StatType.MaxHP => _maxHP,
            StatType.CurrentHP => _currentHP,
            StatType.MaxStamina => _maxEP,
            StatType.CurrentStamina => _currentEP,
            StatType.StaminaRegenRate => _regenRateEP,
            _ => 0
        };
    }

    public StatData GetStatData()
    {
        return new StatData
        {
            strength = _strength,
            speed = _speed,
            maxHP = _maxHP,
            currentHP = _currentHP,
            maxStamina = _maxEP,
            currentStamina = _currentEP,
            staminaRegenRate = _regenRateEP,
            deathCount = _deathCount
        };
    }

    public void SetStatData(StatData data)
    {
        _strength = (int)data.strength;
        _speed = (int)data.speed;
        _maxHP = (int)data.maxHP;
        _currentHP = (int)data.currentHP;
        _maxEP = data.maxStamina;
        _currentEP = data.currentStamina;
        _regenRateEP = data.staminaRegenRate; 
        _deathCount = data.deathCount;
    }

    public void StaminaTick()
    {
        if (_currentEP < _maxEP)
            _currentEP = Mathf.Clamp(_currentEP + _regenRateEP, 0, _maxEP);
    }

    public void ConsumeStamina(float value)
    {
        _currentEP = Mathf.Clamp(_currentEP - value, 0, _maxEP);
    }

    public void Heal(int value)
    {
        _currentHP = Mathf.Clamp(_currentHP + value, 0, _maxHP);
    }

    public override void Damage(int value)
    {
        if (_isDamageInterval) return;

        //CameraManager.Instance?.Shake();

        _currentHP = Mathf.Clamp(_currentHP - value, 0, _maxHP);
        _isDamageInterval = true;
        StartCoroutine(HitEffect());

        if (_currentHP <= 0)
            Death();
    }

    public override void Death()
    {
        _deathCount++;
        _currentHP = _maxHP;
        transform.position = Vector3.up;
        GameDataManager.Instance.SaveGameData();
    }

    IEnumerator HitEffect()
    {
        yield return new WaitForSeconds(0.6f);
        _isDamageInterval = false;
    }
}
