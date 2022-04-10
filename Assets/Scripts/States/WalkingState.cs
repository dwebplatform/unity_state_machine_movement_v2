using UnityEngine;
public class WalkingState : BaseState
{
  private MovementController _movementController;

  private bool _isGrounded;

  private HittedParams hittedParams;

  private float offset = 0.1f;

  private float _movementSpeed = 5f;

  private float _horizontalInput;

  public WalkingState(string name, MovementController movementController) :
      base(name)
  {
    _movementController = movementController;
  }

  public override void Enter()
  {
    base.Enter();
  }

  public override void HandleInput()
  {
    base.HandleInput();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
    _horizontalInput = Input.GetAxis("Horizontal");
    if (Mathf.Abs(_horizontalInput) < Mathf.Epsilon)
    {
      _movementController.ChangeState(MovementController.idleState);
    }
  }

  private void TryMove()
  {
    _movementController.playerVelocity = new Vector2(_horizontalInput * _movementSpeed,_movementController.playerVelocity.y);
    if (Mathf.Sign(_movementController.playerVelocity.x) > 0 &&hittedParams.isHittedRight)
    {
      _movementController.playerVelocity.x = 0f;
    }
    if (Mathf.Sign(_movementController.playerVelocity.x) < 0 &&hittedParams.isHittedLeft)
    {
      _movementController.playerVelocity.x = 0f;
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    //* check ground
    MovementController
        .collisionManager
        .CheckGround(offset,
        //* is grounded
        (Vector2 surfacePoint, Vector2 boxSize) =>
        {
          _isGrounded = true;
          _movementController.transform.position = new Vector3(_movementController.transform.position.x,surfacePoint.y + boxSize.y / 2,_movementController.transform.position.z);
          _movementController.playerVelocity.y = 0f;
        },
        //* is not grounded
        () =>
        {
          _isGrounded = false;
        })
        .WallCheck(MovementController.WALL_OFFSET,
        (rightCollidedInfo, isHitted) =>
        {
          hittedParams.isHittedRight = isHitted;
          if (!isHitted)
          {
            return;
          }
          _movementController.transform.position = CollisionUtils.AdjustPositionRight(rightCollidedInfo, _movementController);
        },
        (leftCollidedInfo, isHitted) =>
        {
          hittedParams.isHittedLeft = isHitted;

          if (!isHitted)
          {
            return;
          }
          _movementController.transform.position = CollisionUtils.AdjustPositionLeft(leftCollidedInfo, _movementController);
        });

    if (!_isGrounded)
    {
      _movementController.playerVelocity.y -=
          MovementController.GRAVITY_SCALE * Time.fixedDeltaTime;
    }
    TryMove();
  }
}
