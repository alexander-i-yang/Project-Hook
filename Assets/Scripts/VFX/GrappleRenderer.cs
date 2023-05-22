using Player;
using UnityEngine;

public class GrappleRenderer : MonoBehaviour {
    private LineRenderer _lr;
    private PlayerStateMachine _parent;
    
    private void Awake() {
        _lr = GetComponent<LineRenderer>();
        _parent = transform.parent.GetComponent<PlayerStateMachine>();
    }


    private void Update() {
        if (_parent.IsGrappling()) {
            _lr.enabled = true;
            _lr.SetPosition(0, transform.parent.position);
            _lr.SetPosition(1, _parent.GetGrapplePos());
        } else {
            _lr.enabled = false;
        }
    }
}