using System;

public class SECounter
{
    public StatusEffect statusEffect;
    public ICanGetStatusEffect target;
    public int duration;
    public bool isActive = false;
    public Stat affectedStat;

    public SECounter(StatusEffect statusEffect, ICanGetStatusEffect target)
    {
        if(statusEffect != null)
            this.statusEffect = statusEffect;

        duration = this.statusEffect.duration;
        this.target = target;
        affectedStat = target.StatDir[Enum.GetName(typeof(StatNames), statusEffect.statName)];
    }

    public void Tick()
    {
        if(statusEffect.appliesEveryTurn)
        {
            if(duration > 0)
            {
                duration--;
                statusEffect.ApplyEffect(target);
            }
        }
        else
        {
            if (duration > 0)
            {
                duration--;

                if (!isActive)
                {
                    statusEffect.ApplyEffect(target);
                    isActive = true;
                }
            }
            else
            {
                target.StatDir[affectedStat.StatName].ResetMaxValue();
                target.RemoveAppliedStatusEffect(this);
            }
        }
                
    }
}
