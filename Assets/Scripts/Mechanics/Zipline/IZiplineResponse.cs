using UnityEngine;

namespace Mechanics
{
    public interface IZiplineResponse
    {
        public bool OnZiplineCollide(Zipline z, Vector2 direction);
    }
}