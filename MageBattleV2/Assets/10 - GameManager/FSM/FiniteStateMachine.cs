using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    private Dictionary<string, FiniteState> machine = null;
    private FiniteState currentState = null;
    private PlayerContext context = null;
    private string nextState = null;

    public FiniteStateMachine(PlayerContext context)
    {
        this.machine = new Dictionary<string, FiniteState>();

        this.context = context;
    }

    public void Add(FiniteState state)
    {
        machine.Add(state.Title, state);

        state.Context = context;

        if (currentState == null)
        {
            currentState = state;
        }
    }

    public void OnUpdate(InputKeys inputKeys, float dt)
    {
        if (nextState == null)
        {
            nextState = currentState.OnUpdate(inputKeys, dt);
        } else
        {
            if (machine.TryGetValue(nextState, out FiniteState state))
            {
                //currentState.OnExit();
                currentState = state;
                currentState.OnEnter();
                nextState = null;
            }
        }
    }
}
