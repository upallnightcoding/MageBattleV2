using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteState
{
    public string Title { get; private set; }

    public FiniteState(string title)
    {
        Title = title;
    }

    public abstract void OnEnter();
    public abstract string OnUpdate(InputCntrl inputKeys, float dt);
    public abstract void OnExit();
}
