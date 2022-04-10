using UnityEngine;

public class CollisionUtils
{
  static public Vector3 AdjustPositionRight(CollidedInfo collidedInfo, MovementController movementController)
  {
    Vector3 normal = collidedInfo.normal.normalized;
    Vector3 difference = collidedInfo.collider.transform.position - movementController.transform.position;
    float gap = difference.x - collidedInfo.collider.bounds.size.x / 2 - movementController._boxCollider.size.x / 2;
    Vector3 correctedPosition = movementController.transform.position - new Vector3(normal.x * gap, normal.y * gap, 0f);
    return correctedPosition;
  }
  static public Vector3 AdjustPositionLeft(CollidedInfo collidedInfo, MovementController movementController)
  {
    Vector3 normal = collidedInfo.normal.normalized;
    Vector3 difference = movementController.transform.position - collidedInfo.collider.transform.position;
    float gap = difference.x - collidedInfo.collider.bounds.size.x / 2 - movementController._boxCollider.bounds.size.x / 2;
    Vector3 newPosition = movementController.transform.position - new Vector3(normal.x * gap, normal.y * gap, 0f);
    return newPosition;
  }

}
