using Player;
using UnityEngine;

public class GrappleRenderer : MonoBehaviour {
    private LineRenderer _lr;
    private AbilityStateMachine _parent;
    
    private void Awake() {
        _lr = GetComponent<LineRenderer>();
        _parent = transform.parent.GetComponent<AbilityStateMachine>();
    }


    private void Update() {
        if (_parent.IsGrappleExtending()) {
            Vector2 v = _parent.GetGrappleExtendPos();
            _lr.enabled = true;
            UpdatePoints(v);
        } else if (_parent.IsGrappling()) {
            _lr.enabled = true;
            UpdatePoints(_parent.GetGrapplePos());
        } else {
            _lr.enabled = false;
        }
    }

    private void UpdatePoints(Vector2 p1) {
        Vector2 p0 = _parent.transform.position;
        _lr.SetPosition(0, p0);
        _lr.SetPosition(1, p1);
    }
}