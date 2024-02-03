using UnityEngine;

namespace Player
{
    public class Arm : MonoBehaviour
    {
        private float lastAngle;
        [SerializeField] private float lerpSpeed;
        
        public void SetAngle(float a)
        {
            var rot = transform.rotation;
            rot.eulerAngles = new Vector3(0, 0, SmoothOut(a));
            transform.rotation = rot;
        }
        
        float SmoothOut(float target)
        {
            if (Mathf.Abs(target - lastAngle) > 180) target += 360;
            
            float ret = Mathf.Lerp(lastAngle, target, lerpSpeed);
            lastAngle = ret > 360 ? ret - 360 : ret;
            return ret;
        }
    }
}