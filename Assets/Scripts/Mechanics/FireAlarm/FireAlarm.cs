using UnityEngine.Events;
using UnityEngine;
using Combat;

namespace Mechanics
{
    public class FireAlarm : MonoBehaviour, IPunchable
    {
        [SerializeField] private GameObject waterContainerDoor;
        private bool usedOnce = false;
        private GameObject bucket;

        void Start()
        {

        }
        public bool ReceivePunch(Vector2 v)
        {
            if (!usedOnce) {
                Debug.Log("Alarm Used");
                waterContainerDoor.SetActive(false);
                usedOnce = true;
            }
            return false;
        }
    }
}
