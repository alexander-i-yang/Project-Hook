using A2DK.Phys;

namespace Mechanics
{
    public class Frame : Solid
    {
        public override bool Collidable(PhysObj collideWith) => true;
    }
}