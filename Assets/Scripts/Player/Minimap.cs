using System;
using UnityEngine;

namespace Player
{
    public class Minimap : MonoBehaviour
    {
        private PlayerCore _pCore;

        private MeshRenderer _meshRenderer;

        [SerializeField] private GrappleEndpoint grappleEndpoint;
        
        void Awake()
        {
            _pCore = FindObjectOfType<PlayerCore>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            if (grappleEndpoint.OutsideOfScreen())
            {
                SetShouldRender(true);
            }
            else
            {
                SetShouldRender(false);
            }
        }

        void SetShouldRender(bool b)
        {
            _meshRenderer.enabled = b;
            ASK.Helpers.Helper.ForEachChild(transform, (g, i) =>
            {
                g.SetActive(b);
            });
        }
    }
}