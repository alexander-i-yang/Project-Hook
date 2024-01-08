using UnityEngine;

public interface IInputController
{
    //Get -> Returns if the button is pressed
    //_Started -> Returns true the first frame pressed
    //_Finished -> Returns true the first frame released

    public int GetMovementInput();
    public bool MovementStarted();
    public bool MovementFinished();
    public bool RetryStarted();
    public bool GetJumpInput();
    public bool JumpStarted();
    public bool JumpFinished();
    public bool GetDiveInput();
    public bool DiveStarted();
    public bool DiveFinished();
    public bool PausePressed();

    public bool GetGrappleInput();
    public bool GrappleStarted();
    public bool GrappleFinished();

    public bool GetShotgunInput();
    public bool ShotgunStarted();
    public bool ShotgunFinished();
    
    public bool GetParryInput();
    public bool ParryStarted();
    public bool ParryFinished();

    public Vector3 GetMousePos();

    public Vector2 GetStickAim();
}
