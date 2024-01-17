using UnityEngine;

namespace Mechanics
{
    public class ZiplineHolder : MonoBehaviour
    {
        //Function for Tiled importing - do not delete
        public void SetEndpoint(GameObject g)
        {
            GetComponentInChildren<Zipline>().SetTrackEndpoint(g);
        }

        public void Speed(int s) => GetComponentInChildren<Zipline>().SetSpeed(s);
    }
}