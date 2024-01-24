using UnityEngine;
using VFX.Lines;

namespace VFX
{
    public class HangingPlant : MonoBehaviour
    {
        //Tiled function - do not remove
        public void SetConnector(GameObject g)
        {
            var lines = GetComponentsInChildren<LineRendererAnchors>();
            foreach (var l in lines)
            {
                l.Anchor1 = g.transform;
            }

            var springs = GetComponentsInChildren<SpringJoint2D>();
            foreach (var s in springs)
            {
                s.connectedBody = g.GetComponent<Rigidbody2D>();
            }
        }
    }
}