using System;
using Combat;
using MyBox;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Parrier))]
    public class ParryRenderer : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private Parrier _parrier;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _parrier = GetComponent<Parrier>();
        }

        /*private void OnEnable()
        {
            _parrier.OnAim += OnAim;
        }

        private void OnDisable()
        {
            _parrier.OnAim -= OnAim;
        }

        void OnAim()
        {
            _sr.SetAlpha(0.5f);
        }

        void OnIdle()
        {
            
        }

        void OnParry()
        {
            _sr.SetAlpha(0.5f);
        }*/
    }
}