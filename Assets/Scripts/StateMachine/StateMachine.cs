using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }
    private bool canChangState = true;

    public void Initialize(EntityState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newState)
    {
        if(canChangState == false)
        {
            return;
        }
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void UpdateLiveState()
    {
        currentState.Update();
    }
    public void swithOffStateMachine()
    {
        canChangState = false;
    }
}
