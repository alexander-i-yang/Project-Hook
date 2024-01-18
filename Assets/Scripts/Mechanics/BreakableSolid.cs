using A2DK.Phys;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics
{
    public class BreakableSolid : Solid
    {
        [SerializeField] private float breakSpeed = 200;
        
        [SerializeField] private UnityEvent<Vector2, Vector2> onBreak;

        public override bool Collidable(PhysObj collideWith) => true;
        
        public bool ShouldBreak(Vector2 direction, PhysObj against)
        {
            return ((against.velocity - this.velocity) * direction).magnitude >= breakSpeed;
        }
        
        public override bool OnCollide(PhysObj p, Vector2 direction)
        {
            bool col = base.OnCollide(p, direction);
            if (col)
            {
                if (ShouldBreak(direction, p))
                {
                    BreakAgainst(p);
                    return false;
                }
            }
            return col;
        }
        
        private void BreakAgainst(PhysObj p)
        {
            onBreak?.Invoke(p.velocity - this.velocity, transform.position);
            gameObject.SetActive(false);
        }
    }
}