using UnityEngine;
using System;
public class CollisionManager
  {
    private MovementController _movementController;
    private LayerMask isWallLayer;
    private ContactFilter2D _filter;
    private Collider2D[] results = new Collider2D[1];
    public CollisionManager(MovementController movementController)
    {
      _movementController = movementController;
      _filter.SetLayerMask(LayerMask.GetMask("Ground"));
      isWallLayer = LayerMask.GetMask("Wall");
    }
    public CollisionManager CheckGround(float offset, Action<Vector2, Vector2> groundedCallBack, Action notGroundedCallBack)
    {
      Vector2 boxSize = new Vector2(_movementController.transform.localScale.x, _movementController.transform.localScale.y);
      Vector2 point = _movementController.transform.position + Vector3.down * offset;
      if (Physics2D.OverlapBox(point, boxSize, 0, _filter, results) > 0)
      {
        Vector2 surfacePosition = Physics2D.ClosestPoint(_movementController.transform.position, results[0]);
        groundedCallBack(surfacePosition, boxSize);
      }
      else
      {
        notGroundedCallBack();
      }
      return this;
    }

    public CollisionManager WallCheck(float offset, Action<CollidedInfo, bool> onHitRightWallCallBack,
    Action<CollidedInfo, bool> onHitLeftWallCallBack)
    {
      // RaycastHit2D rightHit = Physics2D.CircleCast(
      //   _movementController.transform.position, offset,
      //    Vector2.right, _movementController._boxCollider.size.x/2, 
      //    isWallLayer);
      //    if(rightHit){
      //     onHitRightWallCallBack(true); 
      //    } else {
      //     onHitRightWallCallBack(false); 
      //    }

      //    RaycastHit2D leftHit = Physics2D.CircleCast(
      //   _movementController.transform.position, offset,
      //    Vector2.left, _movementController._boxCollider.size.x/2, 
      //    isWallLayer);
      //    if(leftHit){
      //     onHitLeftWallCallBack(true); 
      //    } else {
      //     onHitLeftWallCallBack(false); 
      //    }

      // float distance, int layerMask
      RaycastHit2D rightHit = Physics2D.Raycast(_movementController.transform.position,
      Vector2.right, _movementController._boxCollider.size.y / 2 + offset,
      isWallLayer);

      RaycastHit2D leftHit = Physics2D.Raycast(_movementController.transform.position,
      -Vector2.right,
      _movementController._boxCollider.size.y / 2 + offset,
      isWallLayer);

      CollidedInfo rightCollidedInfo = new CollidedInfo();
      CollidedInfo leftCollidedInfo = new CollidedInfo();
      if (rightHit.collider != null)
      {
        rightCollidedInfo.normal = rightHit.normal;
        rightCollidedInfo.collider = rightHit.collider;
        rightCollidedInfo.wallType = "RIGHT";
        onHitRightWallCallBack(rightCollidedInfo, true);
      }
      else
      {
        onHitRightWallCallBack(rightCollidedInfo, false);
      }

      if (leftHit.collider != null)
      {
        leftCollidedInfo.normal = leftHit.normal;
        leftCollidedInfo.collider = leftHit.collider;
        leftCollidedInfo.wallType = "LEFT";
        Vector3 normal = leftCollidedInfo.normal;

        onHitLeftWallCallBack(leftCollidedInfo,true);
      }
      else
      {
        onHitLeftWallCallBack(leftCollidedInfo,false);
      }

      return this;
    }
  }
