using UnityEngine;

public class Ticker : SingletonDontDestroy<Ticker>
{
    public const float TICK_TIME_30HZ = 0.03f;    // Fast
    public const float TICK_TIME_10HZ = 0.1f;    // Medium
    public const float TICK_TIME_2HZ = 0.5f;     // Slow

    float _tickTimer30Hz = 0f;
    float _tickTimer10Hz = 0f;
    float _tickTimer2Hz = 0f;

    public bool IsPlaying;
    float _playTime = 0f;

    public delegate void Tick();

    public static event Tick OnTick30Hz;
    public static event Tick OnTick10Hz;
    public static event Tick OnTick2Hz;

    void Update()
    {
        float timeCash = Time.deltaTime;

        _tickTimer30Hz += timeCash;
        if (_tickTimer30Hz >= TICK_TIME_30HZ)
        {
            _tickTimer30Hz -= TICK_TIME_30HZ;
            OnTick30Hz?.Invoke();
        }
        _tickTimer10Hz += timeCash;
        if (_tickTimer10Hz >= TICK_TIME_10HZ)
        {
            _tickTimer10Hz -= TICK_TIME_10HZ;
            OnTick10Hz?.Invoke();
        }

        _tickTimer2Hz += timeCash;
        if (_tickTimer2Hz >= TICK_TIME_2HZ)
        {
            _tickTimer2Hz -= TICK_TIME_2HZ;
            OnTick2Hz?.Invoke();
        }

        if (IsPlaying)
            _playTime += timeCash;
    }
    public float GetGametime()
    {
        if (_playTime <= 0f)
            return 0f;

        float temp = _playTime;
        _playTime = 0;
        return temp;
    }
}