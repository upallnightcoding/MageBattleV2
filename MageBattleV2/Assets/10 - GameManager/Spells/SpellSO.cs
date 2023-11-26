using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Mage Battle/Spell")]
public class SpellSO : ScriptableObject
{
    // Name of the spell
    public string spellName;

    // Rate inwhich a spell can be casted within a second
    public float castPerSec;

    // Number of times a spell can be casted within a round 
    public int castPerRound;    

    // Wait time for the next round to begin
    public float coolDownTime;

    // The amount of force produced by the spell forward
    public float forwardForce;

    public GameObject fx;

    public GameObject modelPreFab;

    public TrailSO trail = null;
}
