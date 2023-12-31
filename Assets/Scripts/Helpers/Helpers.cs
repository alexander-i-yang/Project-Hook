using UnityEngine;

namespace Helpers
{
    public static class Helpers
    {
        public static Vector2 Abs(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }
}