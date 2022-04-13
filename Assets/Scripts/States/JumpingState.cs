using UnityEngine;

public class JumpingState : BaseState
{
  private MovementController _movementController;
  private float _jumpForce = 8f;
  private float _startTime;
  private bool _isGravityIgnored;
  private bool _isWallIgnored;
  private int counter = 0;
  private float _movementSpeed = 5f;
  private float offset = 0.1f;
  private bool _isGrounded;
  private float _horizontalInput;
  private HittedParams hittedParams;
  public JumpingState(string name, MovementController movementController) : base(name)
  {
    _movementController = movementController;
  }
  public override void Enter()
  {
    base.Enter();
    _movementController.playerVelocity = new Vector2(_movementController.playerVelocity.x, _jumpForce);
    //* нужно знать, от чего отскочил объект, чтобы игнорировать только его, а иначе бывает, что
    //* объект прыгнул и влетел в стену, в этот момент игнорировалась гравитация И ПРОВЕРКА СТЕНЫ, они должны быть независимыми
    _isGrounded = false;
    counter += 1;
  }

  public override void HandleInput()
  {
    base.HandleInput();
    _horizontalInput = Input.GetAxis("Horizontal");
  }
  public override void Exit()
  {
    base.Exit();
    hittedParams.isHittedLeft = false;
    hittedParams.isHittedRight = false;
    _isWallIgnored = false;
  }
  public override void LogicUpdate()
  {
    base.LogicUpdate();
    if(_isGrounded)
    {
      _movementController.ChangeState(MovementController.idleState);
    }
    
    
    if(!_isWallIgnored && hittedParams.isHittedLeft){
      _movementController.ChangeState(MovementController.leftWallHitted);
    } 
    if(!_isWallIgnored && hittedParams.isHittedRight){
      _movementController.ChangeState(MovementController.rightWallHitted);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    _isGravityIgnored = CheckIgnoreGravity();
    CollisionManager collisionManager = MovementController.collisionManager;
    if (!_isGravityIgnored)
    {
      collisionManager
        .CheckGround(offset,
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
      if (!_isGrounded && !_isWallIgnored)
      {
        _movementController.playerVelocity.y -= MovementController.GRAVITY_SCALE * Time.fixedDeltaTime;
      }
    }


    _isWallIgnored = CheckIgnoreWall();
    
    collisionManager.WallCheck(MovementController.WALL_OFFSET, (rightCollidedInfo, isHitted) =>
    {
      hittedParams.isHittedRight = isHitted;
      if (!isHitted)
      {
        return;
      }
      
      _movementController.transform.position = CollisionUtils.AdjustPositionRight(rightCollidedInfo, _movementController);
    }, (leftCollidedInfo, isHitted) =>
    {
      hittedParams.isHittedLeft = isHitted;
      if (!isHitted)
      {
        return;
      }
      _movementController.transform.position = CollisionUtils.AdjustPositionLeft(leftCollidedInfo, _movementController);
    });
    TryMove();
  }
  private void TryMove()
  {
    if(!_isWallIgnored){
      _movementController.playerVelocity = new Vector2(_horizontalInput*_movementSpeed,_movementController.playerVelocity.y);
    }
    if (Mathf.Sign(_movementController.playerVelocity.x) > 0 && hittedParams.isHittedRight)
    {
        _movementController.playerVelocity.x = 0f;
    }
    if (Mathf.Sign(_movementController.playerVelocity.x) < 0 && hittedParams.isHittedLeft)
    {
        _movementController.playerVelocity.x = 0f;
    }
  }

  private bool CheckIgnoreWall()
  {
    float deltaTime = Time.time - MovementController.wallStartTime;
    if(deltaTime<0.2f){
      return true;
    }
    return false;
  }
  private bool CheckIgnoreGravity()
  {
    float deltaTime = Time.time - MovementController.gravityStartTime;
    if (deltaTime < 0.2f)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
}
