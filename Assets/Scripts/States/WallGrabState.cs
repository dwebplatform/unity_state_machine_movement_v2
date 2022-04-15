using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabState : BaseState
{

    private MovementController _movementController;
    public Vector2 hittedNormal;

    private bool _isGrounded;
    private float offset = 0.1f;
    private HittedParams hittedParams;
    public WallGrabState(string name, MovementController movementController): base(name){
        _movementController = movementController;
    }
  public override void Enter()
  {
    base.Enter();
    _movementController.playerVelocity = Vector2.zero;
  }

  public override void HandleInput()
  {
    base.HandleInput();
    if(Input.GetKeyDown(KeyCode.Space)){
        _movementController.playerVelocity  = new Vector2(-hittedNormal.x*5f,5f);
        _movementController.ChangeState(MovementController.jumpFromWallState);
    }
  }
  public override void Exit()
  {
    base.Exit();
    _isGrounded = false;
  }
  public override void LogicUpdate()
  {
    base.LogicUpdate();
    if(_isGrounded){
        _movementController.ChangeState(MovementController.idleState);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
    //* falling down
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
        }).WallCheck(MovementController.WALL_OFFSET, (rightCollider,isHitted) =>
  {
    //*
    hittedParams.isHittedRight  = isHitted; 
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

        if(!_isGrounded){
            _movementController.playerVelocity.y -= MovementController.GRAVITY_SCALE/2*Time.fixedDeltaTime;
        }
   
  }
}
