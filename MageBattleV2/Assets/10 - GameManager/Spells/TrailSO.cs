using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail", menuName = "Mage Battle/Trail")]
public class TrailSO : ScriptableObject
{
    public AnimationCurve animationCurve;
    public float time = 0.5f;
    public float minvertexDistance = 0.1f;
    public Gradient gradient;
    public Material material;
}
