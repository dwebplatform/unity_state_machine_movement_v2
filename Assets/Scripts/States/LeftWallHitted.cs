using UnityEngine;

public class LeftWallHitted : BaseState
{
  private MovementController _movementController;
  private float offset = 0.1f;
  private float _impulseSpeed = 8f;
  private bool _isGrounded;
  public LeftWallHitted(string name, MovementController movementController) : base(name)
  {
    _movementController = movementController;
  }
  public override void Enter()
  {
    base.Enter();
  }
  public override void Exit()
  {
    base.Exit();
    MovementController.wallStartTime = Time.time;
    _isGrounded = false;
  }

  private void TryJump()
  {
    _movementController.playerVelocity = new Vector2(_impulseSpeed, 0f);
    _movementController.ChangeState(MovementController.jumpingState);
  }
  public override void LogicUpdate()
  {
    base.LogicUpdate();
    if (_isGrounded)
    {
      _movementController.ChangeState(MovementController.idleState);
    }
    bool isRightPressed = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

    if (Input.GetKeyDown(KeyCode.Space) && isRightPressed)
    {
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
      _movementController.playerVelocity.y -= MovementController.GRAVITY_SCALE * Time.fixedDeltaTime;
    }
  }


}
