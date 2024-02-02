using UnityEngine;

namespace UsefulExtensions
{
    public static class CameraExtensions
    {
        public static Bounds WorldSpaceOrthographicBounds(this Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;

            Bounds bounds = new Bounds(camera.transform.position, new UnityEngine.Vector3(cameraHeight * screenAspect, cameraHeight, 0));

            return bounds;
        }
    }
}
