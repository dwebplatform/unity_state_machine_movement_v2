using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFromWallState : BaseState
{
  private MovementController _movementController;
  HittedParams hittedParams;
  public JumpFromWallState(string name, MovementController movementController) : base(name)
  {
    _movementController = movementController;
  }
  private float _startTime;
  public override void Enter()
  {
    base.Enter();
    _startTime = Time.time;
  }

  public override void HandleInput()
  {
    base.HandleInput();
  }
  public override void LogicUpdate()
  {
    base.LogicUpdate();
    float delta = Time.time - _startTime;

    bool isBadForCheckWall = delta < 0.2;
    
    //* если мы встретились со стенкой, то переходим в wallGrabState
    if (!isBadForCheckWall && hittedParams.isHittedLeft)
    {
      MovementController.wallGrabState.hittedNormal = new Vector2(-1f, 0);
      _movementController.ChangeState(MovementController.wallGrabState);
    }
    if (!isBadForCheckWall && hittedParams.isHittedRight)
    {
      MovementController.wallGrabState.hittedNormal = new Vector2(1f, 0);
      _movementController.ChangeState(MovementController.wallGrabState);
    }
  }
  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
    Debug.Log("Works FINE");
    MovementController.collisionManager.WallCheck(MovementController.WALL_OFFSET, (rightCollider, isHitted) =>
{
  hittedParams.isHittedRight = isHitted;
  if (!isHitted)
  {
    return;
  }
  _movementController.transform.position = CollisionUtils.AdjustPositionRight(rightCollider, _movementController);
}, (leftCollidedInfo, isHitted) =>
{
  hittedParams.isHittedLeft = isHitted;
  if (!isHitted)
  {
    return;
  }
  _movementController.transform.position = CollisionUtils.AdjustPositionLeft(leftCollidedInfo, _movementController);
});
  }
  public override void Exit()
  {
    base.Exit();
    hittedParams.isHittedLeft = false;
    hittedParams.isHittedRight = false;
  }

}
