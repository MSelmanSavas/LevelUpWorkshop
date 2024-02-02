using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsefulExtensions
{
    public static class BoundsExtensions
    {
        public static Bounds Get2DBounds(this Bounds aBounds)
        {
            var ext = aBounds.extents;
            ext.z = float.PositiveInfinity;
            aBounds.extents = ext;
            return aBounds;
        }
    }
}
