using UnityEngine;
using System;

public struct CollidedInfo
{
  public Vector3 normal;
  public Collider2D collider;
  public string wallType;
}

public class MovementController : MonoBehaviour
{
  Rigidbody2D _rigidBody;
  public BoxCollider2D _boxCollider;
  public Vector2 playerVelocity;
  static public float GRAVITY_SCALE = 32f;
  //* managers: start
  public static CollisionManager collisionManager;
  public static float WALL_OFFSET = 0.01f;
  public static float gravityStartTime;
  public static float wallStartTime;
  #region 
  private BaseState _currentState;
  public static IdleState idleState;
  public static WalkingState walkingState;
  public static JumpingState jumpingState;
  public static RightWallHitted rightWallHitted;
  public static LeftWallHitted leftWallHitted;
  #endregion
  //* States: end

  public void ChangeState(BaseState newState)
  {
    if (newState != _currentState)
    {
      _currentState.Exit();
      _currentState = newState;
      _currentState.Enter();
    }
  }
  private void Awake()
  {
    _rigidBody = GetComponent<Rigidbody2D>();
    _boxCollider = GetComponent<BoxCollider2D>();
    MovementController.collisionManager = new CollisionManager(this);

    MovementController.idleState = new IdleState("Idle", this);
    MovementController.walkingState = new WalkingState("Walking", this);
    MovementController.jumpingState = new JumpingState("Jumping", this);
    MovementController.rightWallHitted  = new RightWallHitted("Right", this);
    MovementController.leftWallHitted = new LeftWallHitted("Left",this);
    
    _currentState = MovementController.idleState;
  }

  public void HandleInput()
  {
    _currentState.HandleInput();
  }
  private void Update()
  {
    HandleInput();
    _currentState.LogicUpdate();
  }
  private void FixedUpdate()
  {
    _currentState.PhysicsUpdate();
    _rigidBody.velocity = playerVelocity;
  }
}
