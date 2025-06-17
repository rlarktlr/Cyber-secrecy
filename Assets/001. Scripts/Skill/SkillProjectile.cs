using UnityEngine;

public class SkillProjectile : Skill
{
    [Header("투사체 설정")]
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float projectileSpeed = 20f;
    void OnEnable() => Ticker.OnTick10Hz += TimerTick;
    void OnDisable() => Ticker.OnTick10Hz -= TimerTick;
    protected override void Activate()
    {
        GameObject proj = Instantiate(
            _projectilePrefab,
            _spawnPoint.position,
            _spawnPoint.rotation
        );

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = _spawnPoint.forward * projectileSpeed;
    }
}
