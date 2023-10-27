using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterCntrl 
{
    private SpellSO spell;
    private int castPerRound;
    private float reloadTimeSec;
    private float castingRateSec;

    public void Set(SpellSO spell)
    {
        this.spell = spell;
        castPerRound = spell.castPerRound;
        reloadTimeSec = 0.0f;
        castingRateSec = CalcCastingRate();
    }

    public void Cast(Vector3 spawnPoint, Vector3 direction, float dt)
    {
        Casting(spawnPoint, direction, dt);
    }

    private void Casting(Vector3 spawnPoint, Vector3 direction, float dt)
    {
        if (reloadTimeSec <= 0.0f)
        {
            if (castPerRound != 0)
            {
                if (castingRateSec <= 0.0f)
                {
                    GameObject cast = 
                        Object.Instantiate(spell.modelPreFab, spawnPoint, Quaternion.identity);
                    //cast.transform.forward = direction;
                    cast.GetComponent<Rigidbody>().AddForce(direction * spell.forwardForce, ForceMode.Impulse);

                    castingRateSec = CalcCastingRate();
                    castPerRound--;
                } else
                {
                    castingRateSec -= dt;
                }
            } else
            {
                reloadTimeSec = spell.reloadTimeSec;
                castPerRound = spell.castPerRound;
            }
        } else
        {
            reloadTimeSec -= dt;
        }
    }

    private float CalcCastingRate()
    {
        return (1.0f / spell.castPerSec);
    }
}
