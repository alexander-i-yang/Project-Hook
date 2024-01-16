using System.Collections.Generic;
using Mechanics;
using Player;
using UnityEngine;
using VFX;

public class GrappleRenderer : MonoBehaviour {
    private LineRenderer _lr;
    private GrapplerStateMachine _parent;
    [SerializeField] private GameObject leafPrefab;
    [SerializeField] private GameObject curvePointPrefab;
    [SerializeField] private float leafSpacing;
    private List<Leaf> _leaves = new();
    private List<GrappleCurvePoint> _curvePoints = new();
    [SerializeField] private float curveSpeed;
    
    private void Awake() {
        _lr = GetComponent<LineRenderer>();
        _parent = transform.parent.GetComponent<GrapplerStateMachine>();
    }


    void Update() {
        if (_parent.IsGrappleExtending()) {
            Vector2 v = _parent.GetGrappleExtendPos();
            _lr.enabled = true;
            UpdatePoints(v);
        } else if (_parent.IsGrappling()) {
            _lr.enabled = true;
            UpdatePoints(_parent.GetGrapplePos());
        } else {
            RenderOff();
        }
    }

    private void RenderOff()
    {
        _lr.enabled = false;
        foreach (var leaf in _leaves)
        {
            leaf.SetEnabled(false);
        }
    }

    private void UpdatePoints(Vector2 p1) {
        Vector2 p0 = _parent.transform.position;
        _lr.SetPosition(0, p0);
        _lr.SetPosition(1, p1);

        Vector2 vineVector = p1 - p0;
        
        
        int numLeaves = (int)(vineVector.magnitude / leafSpacing);
        float vectorAngle = Vector2.SignedAngle(Vector2.right, vineVector);
        /*_lr.positionCount = numLeaves;
        for (int i = 0; i < numLeaves; ++i)
        {
            if (i >= _curvePoints.Count)
            {
                var newP = Instantiate(curvePointPrefab, transform).GetComponent<GrappleCurvePoint>();
                _curvePoints.Add(newP);
            }

            float prevAngle = i == 0 ? vectorAngle : _curvePoints[i - 1].Angle;
            _curvePoints[i].CalcPos(curveSpeed, prevAngle, p0, i*leafSpacing);
            _lr.SetPosition(i, _curvePoints[i].transform.position);
        }*/
        
        for (int l = 0; l < numLeaves; ++l)
        {
            if (l >= _leaves.Count)
            {
                var newLeaf = Instantiate(leafPrefab, transform).GetComponent<Leaf>();
                _leaves.Add(newLeaf);
            }

            var curLeaf = _leaves[l];
            curLeaf.SetEnabled(true);
            curLeaf.transform.position = p0 + vineVector.normalized * (l * leafSpacing);
            curLeaf.SetRotation(vectorAngle + (l % 2 == 0 ? -90 : 90));
        }
        
        for (int l = numLeaves; l < _leaves.Count; ++l) _leaves[l].SetEnabled(false);
    }
}