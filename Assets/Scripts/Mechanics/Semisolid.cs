using A2DK.Phys;

namespace Mechanics
{
    public class Semisolid : Solid
    {
        public override bool Collidable(PhysObj collideWith) => PassThrough(collideWith);

        public bool PassThrough(PhysObj p)
        {
            bool pAboveMe = p.ColliderBottomY() >= ColliderTopY();
            return p.velocityY <= 0 && pAboveMe;
        }

        public override bool IsGround(PhysObj whosAsking)
        {
            return PassThrough(whosAsking);
        }
    }
}