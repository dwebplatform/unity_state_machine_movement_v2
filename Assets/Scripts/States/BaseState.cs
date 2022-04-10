using UnityEngine;

public class BaseState 
{
        public string name;
    public BaseState(string name){
        this.name = name;
    }
    public virtual void HandleInput(){}
    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void LogicUpdate(){
        Debug.Log(name);
    }

    public virtual void PhysicsUpdate(){}

}
