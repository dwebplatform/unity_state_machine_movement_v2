using UnityEngine;
struct HittedParams
{
  public bool isHittedRight;
  public bool isHittedLeft;
};
public class IdleState : BaseState
{

  private MovementController _movementController;
  private float _horizontalInput;
  HittedParams hittedParams;
  // public 
  private bool _isGrounded;
  private float offset = 0.1f;
  public IdleState(string name, MovementController movementController) : base(name)
  {
    _movementController = movementController;
  }

  public override void Enter()
  {
    base.Enter();
    _movementController.playerVelocity = new Vector2(0f, 0f);
  }


  public void TryWalk()
  {
    //* if wall in front of movement don't enter to walking state
    bool isAllowedMoveForward = true;
    if(_horizontalInput> Mathf.Epsilon && hittedParams.isHittedRight){
      isAllowedMoveForward = false;
    }
    if(_horizontalInput < 0f && hittedParams.isHittedLeft){
      isAllowedMoveForward = false;
    }
    if(isAllowedMoveForward){
      _movementController.ChangeState(MovementController.walkingState);
    }
  }
  public override void HandleInput()
  {
    base.HandleInput();
    _horizontalInput = Input.GetAxis("Horizontal");

    if (Mathf.Abs(_horizontalInput) > Mathf.Epsilon)
    {
      TryWalk();
    }
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
  }
  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

  MovementController.collisionManager.CheckGround(offset,
  //* is grounded
  (Vector2 surfacePoint, Vector2 boxSize) =>
  {
    _isGrounded = true;
    _movementController.transform.position = new Vector3(_movementController.transform.position.x,
    surfacePoint.y + boxSize.y / 2, _movementController.transform.position.z);
    _movementController.playerVelocity.y = 0f;
  },
  //* is not grounded
  () =>
  {
    _isGrounded = false;
  }).WallCheck(MovementController.WALL_OFFSET, (collidedInfo,isHitted) =>
  {
    //*
    hittedParams.isHittedRight  = isHitted; 
  }, (leftCollidedInfo, isHitted) =>
  {
    hittedParams.isHittedLeft = isHitted;
    
  });
    if (!_isGrounded)
    {
      _movementController.playerVelocity.y -= MovementController.GRAVITY_SCALE * Time.fixedDeltaTime;
    }
  }
}
