using UnityEngine;

public class TrackingUI : MonoBehaviour
{
    Camera _camera;
    Transform _target;
    public void SetTrack(Transform target)
    {
        _target = target;
        Track();
        Ticker.OnTick30Hz += Track;
    }

    private void OnDisable()
    {
        if (_target != null)
        {
            Ticker.OnTick30Hz -= Track;
            _target = null;
        }
    }
    public void Track()
    {
        if (_target == null)
            return;
        if (_camera == null)
            _camera = Camera.main;
        Vector3 screenPos = _camera.WorldToScreenPoint(_target.position);
        transform.position = screenPos;
    }
}
