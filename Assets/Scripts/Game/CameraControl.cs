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

        public Vector3 ScreenSpaceToWorldSpace(Vector2 position, float distance)
        {
            float height = 2 * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float width = height * 16 / 9;
            Vector3 centre = new Vector3(mainCamera.transform.position.x - Mathf.Sign(mainCamera.transform.position.x) * distance * Mathf.Cos(mainCamera.transform.localEulerAngles.x), mainCamera.transform.position.x - distance * Mathf.Cos(mainCamera.transform.localEulerAngles.x), 0);

            return centre + Vector3.right * position.x * width + Vector3.Cross((mainCamera.transform.position - centre), Vector3.right).normalized * position.y * height;
        }
    }
}
