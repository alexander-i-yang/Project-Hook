using System;
using Combat;
using MyBox;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Puncher))]
    public class ParryRenderer : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private Puncher _puncher;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _puncher = GetComponent<Puncher>();
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