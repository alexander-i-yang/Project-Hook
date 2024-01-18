using System;
using ASK.Helpers;
using MyBox;
using UnityEngine;

namespace VFX
{
    public class ShowAfter : MonoBehaviour
    {
        [SerializeField] private float delay;
        
        private void Awake()
        {
            GetComponent<SpriteRenderer>().SetAlpha(0);
        }

        public void Play()
        {
            StartCoroutine(Helper.DelayAction(delay, () =>
            {
                GetComponent<SpriteRenderer>().SetAlpha(1);
            }));
        }
    }
}