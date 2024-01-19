using System;
using ASK.Helpers;
using MyBox;
using UnityEngine;

namespace VFX
{
    public class ShowAfterQuad : MonoBehaviour
    {
        [SerializeField] private float delay;
        
        private void Awake()
        {
            GetComponent<MeshRenderer>().enabled=false;
        }

        public void Play()
        {
            StartCoroutine(Helper.DelayAction(delay, () =>
            {
                GetComponent<MeshRenderer>().enabled=true;
                GetComponent<Animator>().Play("BigMode");
            }));
        }
    }
}