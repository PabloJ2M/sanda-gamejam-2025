using UnityEngine;

namespace Unity.Cinemachine
{
    public enum CinemachineCameraType
    {
        None = -1, Orbit = 0, ThreeRig = 1
    }
    public class CinemachineStateCamera : SingletonBasic<CinemachineStateCamera>
    {
        [SerializeField] private CinemachineCamera[] _cameras;

        protected override void Awake() { base.Awake(); DisableCameras(); }

        public void DisableCameras() => SetCamera(CinemachineCameraType.None);
        public void SetTarget(Transform transform)
        {
            foreach (var camera in _cameras)
                camera.Follow = transform;
        }
        public void SetCamera(CinemachineCameraType type)
        {
            for (int i = 0; i < _cameras.Length; i++)
                _cameras[i].gameObject.SetActive(i == (int)type);
        }
    }
}