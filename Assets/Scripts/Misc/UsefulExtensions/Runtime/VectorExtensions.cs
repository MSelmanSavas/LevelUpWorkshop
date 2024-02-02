using UnityEngine;

namespace UsefulExtensions.Vector2
{
    public static class Vector2Extensions
    {
        public static float GetSizeMultiply(this UnityEngine.Vector2 vector2)
        {
            return vector2.x * vector2.y;
        }

        public static UnityEngine.Vector2 WithX(this UnityEngine.Vector2 vector2, float x)
        {
            vector2.x = x;
            return vector2;
        }
        public static UnityEngine.Vector2 WithY(this UnityEngine.Vector2 vector2, float y)
        {
            vector2.y = y;
            return vector2;
        }
    }
}

namespace UsefulExtensions.Vector3
{
    public static class Vector3Extensions
    {
        public static UnityEngine.Vector3 WithX(this UnityEngine.Vector3 vector3, float x)
        {
            vector3.x = x;
            return vector3;
        }
        public static UnityEngine.Vector3 WithY(this UnityEngine.Vector3 vector3, float y)
        {
            vector3.y = y;
            return vector3;
        }

        public static UnityEngine.Vector3 WithZ(this UnityEngine.Vector3 vector3, float z)
        {
            vector3.z = z;
            return vector3;
        }
    }
}
