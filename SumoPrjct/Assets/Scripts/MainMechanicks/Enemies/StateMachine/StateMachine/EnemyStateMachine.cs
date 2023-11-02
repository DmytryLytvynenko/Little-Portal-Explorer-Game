using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState { get; set; }
    public List<EnemyState> AllStates;

    public void Initiallize(EnemyState startingState)
    {
        CurrentEnemyState = startingState;
        CurrentEnemyState.EnterState();
        Debug.Log(CurrentEnemyState);
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = newState;
        CurrentEnemyState.EnterState();
        Debug.Log(CurrentEnemyState);
    }

    public void AllStatesOnEnable()
    {
        foreach (EnemyState state in AllStates) 
        {
            state.OnEnable();
        }
    }    
    public void AllStatesOnDisable()
    {
        foreach (EnemyState state in AllStates)
        {
            state.OnDisable();
        }
    }
    public void AddState(EnemyState state)
    {
        AllStates.Add(state);
    }
}
