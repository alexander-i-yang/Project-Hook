using ASK.Helpers;
using MyBox;
using UnityEngine;

namespace VFX
{
    public class DialogueBubble : MonoBehaviour
    {
        private Animator _animatorA;

        [SerializeField] private float startDelay;
        [SerializeField] private float readTime;
        
        void Awake()
        {
            _animatorA = GetComponent<Animator>();
            GetComponent<SpriteRenderer>().SetAlpha(0);
        }

        public void Play()
        {
            StartCoroutine(Helper.DelayAction(startDelay, () =>
            {
                _animatorA.Play("DialogueIn");
                StartCoroutine(Helper.DelayAction(readTime, () =>
                {
                    _animatorA.Play("DialogueOut");
                }));
            }));
        }
    }
}