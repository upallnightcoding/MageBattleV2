using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterCntrl 
{
    private SpellSO spell;
    private int castPerRound;
    private float lastCoolDownTime;
    private float lastCastingRateSec;

    private float coolDownTimeSec;
    private float castPerSec;
    private GameObject modelPreFab;
    private float spellForce;

    public void Set(SpellSO spell)
    {
        this.spell = spell;

        castPerRound = spell.castPerRound;
        coolDownTimeSec = spell.coolDownTime;
        castPerSec = CalcCastingRate();
        modelPreFab = spell.modelPreFab;
        spellForce = spell.forwardForce;

        lastCoolDownTime = InitCheckTime(coolDownTimeSec);
        lastCastingRateSec = InitCheckTime(castPerSec);
    }

    public void Cast(Vector3 spawnPoint, Vector3 direction)
    {
        Casting(spawnPoint, direction);
    }

    private void Casting(Vector3 spawnPoint, Vector3 direction)
    {
        if (CheckTime(lastCoolDownTime, coolDownTimeSec))
        {
            if (castPerRound != 0)
            {
                if (CheckTime(lastCastingRateSec, castPerSec))
                {
                    GameObject cast = Object.Instantiate(modelPreFab, spawnPoint, Quaternion.identity);
                    cast.transform.forward = direction;
                    cast.GetComponent<Rigidbody>().AddForce(direction * spellForce, ForceMode.Impulse);

                    lastCastingRateSec = Time.time;
                    castPerRound--;
                } 
            } else
            {
                lastCoolDownTime = Time.time;
                castPerRound = spell.castPerRound;
            }
        } 
    }

    private float InitCheckTime(float delta)
    {
        return (Time.time + delta);
    }

    private bool CheckTime(float lastTimeCheck, float delta)
    {
        return ((Time.time - lastTimeCheck) >= delta);
    }

    private float CalcCastingRate()
    {
        return (1.0f / spell.castPerSec);
    }
}
