using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFiniteStateMachine 
{
    public void SetAnimator(float speed);
    public void SetCharCntrl(CharacterController charCntrl);
    public void SetGameObject(GameObject gameObject);
}
