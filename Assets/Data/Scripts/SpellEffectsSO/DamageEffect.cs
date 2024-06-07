using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/SpellEffects/AreaSpellEffect/DamageEffect")]
public class DamageEffect : SpellsWithStatusEffects
{
    public int damage;
    public DamageType damageType;

    public override void CastSpell(TerrainTile tile)
    {
        Instantiate(spellEffectPrefab, tile.transform.position, Quaternion.identity);

        List<TerrainTile> coverArea = TileManager.Instance.GetShape(tile, shape, range);

        foreach (TerrainTile currentTile in coverArea)
        {
            if (currentTile.someoneOnTop == null)
                continue;
            
            DamageStruct damage = new DamageStruct()
            {
                damage = this.damage,
                damageSource = null,
                damageType = damageType
            };

            currentTile.someoneOnTop.TakeDamage(damage);

            if(statusEffects.Count > 0)
                foreach(MaxValueStatusEffect statusEffect in statusEffects)
                {
                    SECounter newEffect = new SECounter(statusEffect, (ICanGetStatusEffect)currentTile.someoneOnTop);
                    ((ICanGetStatusEffect)currentTile.someoneOnTop).ApplyStatusEffectOnSelf(newEffect);
                }
        }
    }
}
