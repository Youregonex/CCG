using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public Operator op;
    public StatNames statName;
    public int value;
    public bool appliesEveryTurn;
    public bool damaging;
    public DamageType damageType;
    public int duration;
    public SEType seType;
    public abstract void ApplyEffect(ICanGetStatusEffect target);
}
