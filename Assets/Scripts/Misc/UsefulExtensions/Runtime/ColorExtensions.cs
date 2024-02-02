namespace UsefulExtensions.Color
{
    public static class ColorExtensions
    {
        public static UnityEngine.Color WithA(this UnityEngine.Color c, float a) => new UnityEngine.Color(c.r, c.g, c.b, a);
    }
}
