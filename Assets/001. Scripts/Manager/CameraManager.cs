using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] CinemachineImpulseSource _basicImpulse;
    [SerializeField] CinemachineCamera _cinemachine;
    [SerializeField] Camera _camera;

    const float TARGET_ASPECT = 16f / 9f;
    private void Update()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / TARGET_ASPECT;

        if (scaleHeight < 1.0f)
        {
            Rect rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            _camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
            _camera.rect = rect;
        }
    }

    public void SetTarget(Transform target)
    {
        _cinemachine.Follow = target;
    }
    public void Shake()
    {
        _basicImpulse.GenerateImpulse();
    }
    public void Shake(CinemachineImpulseSource source)
    {
        source.GenerateImpulse();
    }
}
