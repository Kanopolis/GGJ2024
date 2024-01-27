using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public static CameraTracker mainCameraTracker;

    private void Awake()
    {
        if (mainCameraTracker == null)
            mainCameraTracker = this;
        else
            Destroy(this.gameObject);
    }

    public void TrackPosition(Vector3 _toTrack)
    {
        this.transform.position = _toTrack;
    }
}
