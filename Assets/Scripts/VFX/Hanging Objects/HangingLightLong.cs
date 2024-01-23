using UnityEngine;
using VFX.Lines;

namespace VFX
{
    public class HangingLightLong : MonoBehaviour
    {
        //Tiled function - do not remove
        public void SetConnector(GameObject g)
        {
            var lines = GetComponentsInChildren<LineRendererAnchors>();
            var fixtures = g.GetComponentsInChildren<Rigidbody2D>();
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i].Anchor1 = fixtures[i].transform;
            }

            var springs = GetComponentsInChildren<SpringJoint2D>();
            for (int i = 0; i < springs.Length; ++i)
            {
                springs[i].connectedBody = fixtures[i];
            }
        }
    }
}