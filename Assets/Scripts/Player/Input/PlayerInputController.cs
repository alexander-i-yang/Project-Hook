using ASK.Core;
using Cameras;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine.Events;

namespace Player
{
    public class PlayerInputController : MonoBehaviour, IInputController
    {
        private PlayerControls controls;
        private PlayerControls.GameplayActions inputActions;

        private System.Action PauseAction;

        public int HasPutInput = 0;

        [SerializeField] private UnityEvent firstInput;

        public void OnEnable()
        {
            if (controls == null)
            {
                controls = new PlayerControls();
                inputActions = controls.Gameplay;
            }

            inputActions.Enable();

            inputActions.Pause.performed += OnPause;
            #if UNITY_EDITOR
            inputActions.Debug.performed += OnDebug;
            #endif
        }

        public void OnDisable()
        {
            inputActions.Pause.performed -= OnPause;
            #if UNITY_EDITOR
            inputActions.Debug.performed -= OnDebug;
            #endif
            inputActions.Disable();
        }

        public bool AnyKeyPressed()
        {
            return MovementStarted() || DiveStarted() || JumpStarted();
        }

        public int GetMovementInput()
        {
            if (Game.Instance.FakeControlsArrows != -2)
            {
                return Game.Instance.FakeControlsArrows;
            }
            
            
            int rightInput = inputActions.MoveRight.IsPressed() ? 1 : 0;
            int leftInput = inputActions.MoveLeft.IsPressed() ? 1 : 0;
            return rightInput - leftInput;
        }

        public bool MovementStarted()
        {
            bool bothDirsDifferent = inputActions.MoveRight.IsPressed() ^ inputActions.MoveLeft.IsPressed();
            return MovementChanged() && bothDirsDifferent;
        }

        public bool MovementFinished()
        {
            //If left and right are held at the same time, the player will not move.
            bool bothDirsSame = inputActions.MoveRight.IsPressed() == inputActions.MoveLeft.IsPressed();
            return MovementChanged() && bothDirsSame;
        }
        
        public bool RetryStarted()
        {
            return inputActions.Restart.WasPerformedThisFrame();
        }
        
        public bool GetJumpInput()
        {
            // if (!Game.Instance.FakeControlsZ.Disabled) return Game.Instance.FakeControlsZ.Value;
            return inputActions.Jump.IsPressed();
        }

        public bool JumpStarted()
        {
            // if (!Game.Instance.FakeControlsZ.Disabled) return Game.Instance.FakeControlsZ.WasPressedThisFrame();
            return inputActions.Jump.WasPressedThisFrame();
        }

        public bool JumpFinished()
        {
            // if (!Game.Instance.FakeControlsZ.Disabled) return Game.Instance.FakeControlsZ.WasReleasedThisFrame();
            return inputActions.Jump.WasReleasedThisFrame();
        }

        public bool GetDiveInput()
        {
            return inputActions.Dive.IsPressed();
        }

        public bool DiveStarted()
        {
            return inputActions.Dive.WasPressedThisFrame();
        }

        public bool DiveFinished()
        {
            return inputActions.Dive.WasReleasedThisFrame();
        }

        public bool GetGrappleInput()
        {
            return inputActions.Grapple.IsPressed();
        }

        public bool GrappleStarted()
        {
            if (inputActions.Grapple.WasPressedThisFrame())
            {
                HasPutInput++;
                if (HasPutInput == 2)
                {
                    firstInput?.Invoke();
                }
            }
            return inputActions.Grapple.WasPressedThisFrame();
        }

        public bool GrappleFinished()
        {
            return inputActions.Grapple.WasReleasedThisFrame();
        }
        
        public bool GetParryInput()
        {
            return inputActions.Parry.IsPressed();
        }

        public bool ParryStarted()
        {
            return inputActions.Parry.WasPressedThisFrame();
        }

        public bool ParryFinished()
        {
            return inputActions.Parry.WasReleasedThisFrame();
        }

        public Vector3 GetMousePos()
        {
            Vector2 mPos = Mouse.current.position.ReadValue();
            return CameraProvider.Instance.ScreenToWorldPoint(mPos);
        }

        public Vector2 GetStickAim()
        {
            return inputActions.Aim.ReadValue<Vector2>();
        }
        
        public Vector2 GetAimPos(Vector3 playerPos)
        {
            Vector2 stickInput = GetStickAim();
            if (stickInput != Vector2.zero) return stickInput*30 + (Vector2)playerPos;
            return GetMousePos();
        }

        public void AddToPauseAction(System.Action action)
        {
            PauseAction += action;
        }

        public void RemoveFromPauseAction(System.Action action)
        {
            PauseAction -= action;
        }

        private void OnPause(InputAction.CallbackContext ctx)
        {
            PauseAction?.Invoke();
        }

        public bool PausePressed()
        {
            return inputActions.Pause.WasPressedThisFrame();
        }

        private bool MovementChanged()
        {
            bool dirPressed = inputActions.MoveRight.WasPressedThisFrame() || inputActions.MoveLeft.WasPressedThisFrame();
            bool dirReleased = inputActions.MoveRight.WasReleasedThisFrame() || inputActions.MoveLeft.WasReleasedThisFrame();

            return dirPressed || dirReleased;
        }
        
        #if UNITY_EDITOR
        private void OnDebug(InputAction.CallbackContext ctx)
        {
            Game.Instance.IsDebug = true;
        }
        #endif
    }
}