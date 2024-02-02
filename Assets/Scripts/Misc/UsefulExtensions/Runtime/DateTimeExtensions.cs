using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsefulExtensions.DateTime
{
    public static class DateTimeExtensions
    {
        public static int GetUNIXTimeNow() => (int)System.DateTime.UtcNow.Subtract(System.DateTime.UnixEpoch).TotalSeconds;
    }
}
