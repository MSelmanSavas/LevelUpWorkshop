using UnityEngine;

namespace UsefulExtensions.UIExtensions
{
    public static class UIExtensions
    {
        public static UnityEngine.Vector2 CalculateReferenceSize(
            RectTransform referenceRect,
            float referenceOrthSize,
            UnityEngine.Vector2 referenceResolution,
            UnityEngine.Vector2 referenceCameraPos,
            UnityEngine.Camera referenceCamera)
        {
            float halfWidth = referenceRect.rect.width / 2f;
            float halfHeight = referenceRect.rect.height / 2f;

            UnityEngine.Vector2 leftDown = new UnityEngine.Vector2(-halfWidth, -halfHeight);
            UnityEngine.Vector2 rightUp = new UnityEngine.Vector2(halfWidth, halfHeight);

            UnityEngine.Vector2 baseWorldPositionLeftDown = GetWorldPosition3(leftDown, referenceCameraPos, referenceOrthSize, referenceResolution);
            UnityEngine.Vector2 screenPointLeftDown = referenceCamera.WorldToScreenPoint(baseWorldPositionLeftDown);

            UnityEngine.Vector2 baseWorldPositionRightUp = GetWorldPosition3(rightUp, referenceCameraPos, referenceOrthSize, referenceResolution);
            UnityEngine.Vector2 screenPointRightUp = referenceCamera.WorldToScreenPoint(baseWorldPositionRightUp);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(referenceRect, screenPointLeftDown, referenceCamera, out UnityEngine.Vector2 localPointLeftDown);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(referenceRect, screenPointRightUp, referenceCamera, out UnityEngine.Vector2 localPointRightUp);

            UnityEngine.Vector2 size = localPointRightUp - localPointLeftDown;

            return size;
        }

        public static UnityEngine.Vector2 CalculateReferencePosition(
            RectTransform referenceRect,
            UnityEngine.Vector2 originalScreenPoint,
             float referenceOrthSize,
            UnityEngine.Vector2 referenceResolution,
            UnityEngine.Vector2 referenceCameraPos,
            UnityEngine.Camera referenceCamera)
        {
            UnityEngine.Vector2 halfScreenResBase = (referenceResolution / 2f);
            UnityEngine.Vector2 calculatedScreenPos = halfScreenResBase + originalScreenPoint;


            UnityEngine.Vector2 baseWorldPosition = GetWorldPosition3(calculatedScreenPos, referenceCameraPos, referenceOrthSize, referenceResolution);
            UnityEngine.Vector2 screenPoint = referenceCamera.WorldToScreenPoint(baseWorldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(referenceRect, screenPoint, referenceCamera, out UnityEngine.Vector2 localPoint);

            return localPoint;
        }

        public static UnityEngine.Vector2 GetWorldPosition3(
            UnityEngine.Vector2 screenPos,
            UnityEngine.Vector2 cameraPosition,
            float orthographicSize,
            UnityEngine.Vector2 screenResolution)
        {
            float halfHeight = orthographicSize;
            float halfWidth = halfHeight * screenResolution.x / screenResolution.y;

            UnityEngine.Vector2 viewportPos = new UnityEngine.Vector3(screenPos.x / screenResolution.x, screenPos.y / screenResolution.y);
            UnityEngine.Vector2 cameraPos = new UnityEngine.Vector3(viewportPos.x * 2 - 1, viewportPos.y * 2 - 1);

            cameraPos.x *= halfWidth;
            cameraPos.y *= halfHeight;

            UnityEngine.Vector3 worldPos = cameraPos + cameraPosition;
            return worldPos;
        }
    }
}
