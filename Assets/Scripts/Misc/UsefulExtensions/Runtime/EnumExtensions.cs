using System;
using System.Collections.Generic;

namespace UsefulExtensions.Enum
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> Elements<TEnum>() where TEnum : struct, System.Enum
        {
            return (TEnum[])System.Enum.GetValues(typeof(TEnum));
        }

        public static int AsInt<TEnum>(this TEnum e) where TEnum : struct, System.Enum
        {
            return (int)(object)e;
        }
    }
}

