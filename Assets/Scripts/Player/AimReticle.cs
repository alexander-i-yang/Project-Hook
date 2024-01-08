using UnityEngine;

namespace Player
{
    public class AimReticle : MonoBehaviour
    {
        [SerializeField] private Transform reticle;
        [SerializeField] private Transform playerOrigin;
        [SerializeField] private PlayerInputController _playerInputController;
        
        void Update()
        {
            reticle.position = _playerInputController.GetGrappleAimPos(playerOrigin.transform.position);
        }
    }
}