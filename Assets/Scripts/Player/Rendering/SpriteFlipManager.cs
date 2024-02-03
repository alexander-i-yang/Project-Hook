using UnityEngine;

namespace Player
{
    public class SpriteFlipManager : MonoBehaviour
    {
        private PlayerInputController _input;
        [SerializeField] private Transform sprite;
        
        private void Awake()
        {
            _input = GetComponentInParent<PlayerInputController>();
        }

        void Update()
        {
            int direction = _input.GetMovementInput();
            if (direction != 0) SetFlip(direction);
        }

        void SetFlip(int direction)
        {
            var scale = sprite.transform.localScale;
            scale.x = direction;
            sprite.transform.localScale = scale;
        }
    }
}