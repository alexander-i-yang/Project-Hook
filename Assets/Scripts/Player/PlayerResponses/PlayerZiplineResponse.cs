using Mechanics;
using UnityEngine;

namespace Player.PlayerResponses
{
    public class PlayerZiplineResponse : MonoBehaviour, IZiplineResponse
    {
        [SerializeField] private float mass;

        private PlayerActor p;

        private void Awake()
        {
            p = GetComponent<PlayerActor>();
        }
        
        public bool OnZiplineCollide(Zipline z, Vector2 direction)
        {
            // if (z.ShouldGetPushed(p))
            // {
            //     /*p.velocity = */
            //     z.ResolvePushVelocities(mass, p.velocity);
            // }
            
            // z.ResolvePushVelocities(mass, p.velocity, direction);
            return true;
        }
    }
}