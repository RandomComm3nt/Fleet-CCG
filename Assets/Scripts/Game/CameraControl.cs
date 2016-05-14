using UnityEngine;

namespace Assets.Scripts.Game
{
    public class CameraControl : MonoBehaviour
    {
        private Camera mainCamera;
        public static CameraControl singleton;
     
        private void Start()
        {
            if (singleton)
                Destroy(singleton);
            singleton = this;

            mainCamera = GetComponent<Camera>();
        }

        public void TweenTo(Vector3 position, Quaternion rotation)
        {
            mainCamera.transform.localPosition = position;
            mainCamera.transform.localRotation = rotation;
        }
    }
}
