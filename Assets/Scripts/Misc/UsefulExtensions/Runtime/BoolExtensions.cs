namespace UsefulExtensions.Bool
{
    public static class BoolExtensions
    {
        public static int AsInt(this bool b, int whenTrue = 1, int whenFalse = 0) => b ? whenTrue : whenFalse;
        public static int AsInt(this bool b) => b ? 1 : 0;
    }
}

