using System;
using UnityEngine;

namespace Combat
{
    public abstract class Brain<I, B> : MonoBehaviour
        where I : MonoBehaviour, IInput
        where B : MonoBehaviour, IBehavior
    {
        protected I input;
        protected B behavior;
        
        private void Awake()
        {
            input = GetComponent<I>();
            behavior = GetComponent<B>();
        }
    }
}