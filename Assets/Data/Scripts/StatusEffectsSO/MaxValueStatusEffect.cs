using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StatusEffects/MaxValueStatusEffect")]
public class MaxValueStatusEffect : StatusEffect
{
    public override void ApplyEffect(ICanGetStatusEffect target)
    {
        if(damaging)
        {
            DamageStruct dmg = new DamageStruct()
            {
                damage = value,
                damageType = damageType
            };

            target.TakeDamage(dmg);
            return;
        }

        string statNameStr = Enum.GetName(typeof(StatNames), statName);

        if (target.StatDir.ContainsKey(statNameStr))
            target.StatDir[statNameStr].ModifyMaxValue(value, op);
    }
}

