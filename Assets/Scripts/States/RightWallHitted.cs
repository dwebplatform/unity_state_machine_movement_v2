using UnityEngine;
public class RightWallHitted : BaseState
{
    private MovementController _movementController;
    private float offset = 0.1f;
    private float _impulseSpeed = 8f;
    private bool _isGrounded;
    public RightWallHitted(string name, MovementController movementController): base(name)
    {
        _movementController = movementController;
    }
  public override void Enter()
  {
    base.Enter();
    MovementController.previosHorizontalValue = 0f;
  }
  private void TryJump(){
    _movementController.playerVelocity = new Vector2(-_impulseSpeed, 0f);
    _movementController.ChangeState(MovementController.jumpingState);
  }
  public override void Exit()
  {
    base.Exit();
    MovementController.wallStartTime = Time.time;  
    _isGrounded = false;
    MovementController.previosHorizontalValue = Input.GetAxis("Horizontal");
  }
  public override void LogicUpdate()
  {
    base.LogicUpdate();
    if(_isGrounded){
      _movementController.ChangeState(MovementController.idleState);
    }
    if(Input.GetKeyDown(KeyCode.Space)){
      TryJump();
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    CollisionManager collisionManager = MovementController.collisionManager;
    collisionManager.CheckGround(offset,
  (Vector2 surfacePoint, Vector2 boxSize) =>
  {
    _isGrounded = true;
    _movementController.transform.position = new Vector3(_movementController.transform.position.x,
    surfacePoint.y + boxSize.y / 2, _movementController.transform.position.z);
    _movementController.playerVelocity.y = 0f;
  },
  () =>
  {
    _isGrounded = false;
  });
  if (!_isGrounded)
      {
        _movementController.playerVelocity.y -= MovementController.GRAVITY_SCALE/5 * Time.fixedDeltaTime;
      }
  }

}
