using System.Collections.Generic;

public interface ICanGetStatusEffect : IHaveStats
{
    public void ApplyStatusEffectOnSelf(SECounter statusEffect);
    public void RemoveAppliedStatusEffect(SECounter statusEffect);
    public List<SECounter> appliedStatusEffects { get; set; }
}
