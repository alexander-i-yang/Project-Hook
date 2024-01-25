using System;
using UnityEngine;

namespace VFX.Lines
{
    public class LineRendererAnchors : MonoBehaviour
    {
        public Transform Anchor0;
        public Transform Anchor1;

        private LineRenderer _lr;

        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _lr.SetPosition(0, Anchor0.position);
            _lr.SetPosition(1, Anchor1.position);
        }
    }
}