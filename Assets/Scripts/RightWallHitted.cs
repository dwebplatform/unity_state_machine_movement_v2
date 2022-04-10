using UnityEngine;

public class RightWallHitted : MonoBehaviour
{

    private  void Start()
    {
    }
     void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log(collisionInfo.collider.name+ "COLLIDED RIGHT WALLL");
    }
}
