using UnityEngine;

public class SkillRoll : Skill
{
    [Header("구르기 설정")]
    public float rollDistance = 5f;
    public float rollSpeed = 10f;

    private bool _isRolling;
    private Vector3 _rollDirection;
    private Vector3 _startPos;

    void OnEnable() => Ticker.OnTick10Hz += TimerTick;
    void OnDisable() => Ticker.OnTick10Hz -= TimerTick;

    protected override void Activate()
    {
        if (_isRolling) return;

        _rollDirection = transform.forward.normalized;
        _startPos = transform.position;
        _isRolling = true;
    }

    private void Update()
    {
        if (!_isRolling) return;

        float step = rollSpeed * Time.deltaTime;
        transform.position += _rollDirection * step;

        if (Vector3.Distance(_startPos, transform.position) >= rollDistance)
        {
            _isRolling = false;
        }
    }
}