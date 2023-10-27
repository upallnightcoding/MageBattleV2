using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    private Dictionary<string, FiniteState> machine;
    private FiniteState currentState = null;

    private void Start()
    {
        machine = new Dictionary<string, FiniteState>();
    }

    public void Add(FiniteState state)
    {
        machine.Add(state.Title, state);

        if (currentState == null)
        {
            currentState = state;
        }
    }

    private void Update()
    {
        string nextState = currentState.OnUpdate();

        if (nextState != null)
        {
            if (machine.TryGetValue(nextState, out FiniteState state))
            {
                currentState = state;
            }
        }
    }
}
