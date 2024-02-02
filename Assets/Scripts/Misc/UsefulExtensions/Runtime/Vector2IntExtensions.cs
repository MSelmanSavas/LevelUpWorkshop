namespace UsefulExtensions.Vector2Int
{
    public static class Vector2IntExtensions
    {
        public static int GetSizeMultiply(this UnityEngine.Vector2Int vector2Int)
        {
            return vector2Int.x * vector2Int.y;
        }

        public static float Dot(this UnityEngine.Vector2Int dotFromVector, UnityEngine.Vector2Int dotToVector)
        {
            return UnityEngine.Vector2.Dot((UnityEngine.Vector2)dotFromVector, (UnityEngine.Vector2)dotToVector);
        }
    }
}
