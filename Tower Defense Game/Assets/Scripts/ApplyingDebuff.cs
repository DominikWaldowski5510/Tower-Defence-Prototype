using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ApplyingDebuff 
{
    public enum DebuffType
    {
        Freeze,
        Fire,
        RangeBoost,
        DamageBoost
    }
    public DebuffType typeOfDebuff;
    public float debuffDuration;
    public float debuffDamage;

}
