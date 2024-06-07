using System.Collections.Generic;

public interface IHaveStats : IDamageable
{
    public Dictionary<string, Stat> StatDir { get; set; }
}
