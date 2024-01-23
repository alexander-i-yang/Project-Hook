using System;
using UnityEngine;

namespace VFX
{
    [RequireComponent(typeof(LineRenderer))]
    public class Circle : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private bool _shouldDraw;

        public float Radius;

        private int _numPoints => (int)(Radius * NumPointsScale);

        [SerializeField] private float NumPointsScale;
        
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        void FixedUpdate()
        {
            if (_shouldDraw)
            {
                SetMaterialLength(Radius);
                _lineRenderer.positionCount = _numPoints;
                for (int i = 0; i < _numPoints; ++i)
                {
                    float angle = (float)i / _numPoints * 2 * Mathf.PI;
                    Vector2 arc = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Radius;
                    _lineRenderer.SetPosition(i, (Vector3)arc + transform.position);
                }
            }
        }

        void SetMaterialLength(float r)
        {
            var mat = _lineRenderer.material;
            if (mat.HasProperty("_Length"))
            {
                mat.SetFloat("_Length", r * 2 * Mathf.PI);
            }
        }

        public void ShouldDraw(bool b)
        {
            _lineRenderer.enabled = b;
            _shouldDraw = true;
        }
    }
}