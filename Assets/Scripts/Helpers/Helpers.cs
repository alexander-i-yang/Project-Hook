using UnityEngine;

namespace Helpers
{
    public static class Helpers
    {
        public static Vector2 Abs(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        
        public static Vector2 CombineVectorsWithReset(Vector2 original, Vector2 apply)
        {
            Vector2 proj = Vector3.Project(original, apply);
            bool oppositeDir = Vector2.Dot(apply, proj) < 0;
            return apply + (oppositeDir ? Vector2.zero : proj);
        }
    }
}