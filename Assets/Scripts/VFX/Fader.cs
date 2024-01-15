using System.Collections;
using ASK.Helpers;
using UnityEngine;

namespace VFX
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private float persistTime = 1f;
        [SerializeField] private float fadeTime = 1f;
        [SerializeField] private bool destroyAfterFade;
        
        private SpriteRenderer _mySR;
        private Color _origColor;

        void Awake()
        {
            _mySR = GetComponent<SpriteRenderer>();
            _origColor = _mySR.color;
        }

        void Start() => Fade();

        void Fade()
        {
            _mySR.color = _origColor;
            StartCoroutine(FadeCoroutine(persistTime, fadeTime));
        }
        
        private IEnumerator FadeCoroutine(float persistSeconds, float fadeSeconds)
        {
            yield return Helper.Sleep(persistSeconds);
            Color origColor = _mySR.color;
            Color newColor = _mySR.color;
            newColor.a = 0;
            yield return Helper.FadeColor(
                fadeSeconds, 
                origColor, 
                newColor,
                c =>
                {
                    _mySR.color = c;
                }
            );
            if (destroyAfterFade) Destroy(gameObject);
        }
    }
}