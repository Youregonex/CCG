using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : Unit
{

    protected bool retaliated;


    protected override void Start()
    {
        retaliated = false;
    }

    public override bool TryAttack(IDamageable target, bool retaliate = false)
    {
        if (attacksPerTurn.CurrentStatValue <= 0 || attackDamage.MaxStatValue <= 0)
            return false;

        if(InRangeOfAttack(target))
        {
            DamageStruct damage = new DamageStruct
            {
                damage = attackDamage.MaxStatValue,
                damageSource = this,
                damageType = this.damageType
            };

            target.TakeDamage(damage, retaliate);
            attacksPerTurn.ModifyCurrentValue(1, Operator.Minus);

            ApplyStatusEffectToTarget((ICanGetStatusEffect)target);

            movement.ModifyCurrentValue(movement.CurrentStatValue, Operator.Minus);
        }

        return InRangeOfAttack(target);
    }

    protected override bool InRangeOfAttack(IDamageable target)
    {
        List<TerrainTile> attackRangeArea = TileManager.Instance.GetNeighboursIncludingCorners(TileBelow, attackRange.MaxStatValue);

        foreach (TerrainTile tile in attackRangeArea)
        {
            if (tile.someoneOnTop == null)
                continue;

            if (tile.someoneOnTop == target)
                return true;
        }

        return false;
    }

    protected override void HighlightEnemyInAttackRange()
    {
        List<TerrainTile> attackRangeArea = TileManager.Instance.GetNeighboursIncludingCorners(TileBelow, attackRange.MaxStatValue);

        foreach (TerrainTile tile in attackRangeArea)
        {
            if (tile.someoneOnTop != null)
            {
                if (((IHaveFaction)tile.someoneOnTop).Faction != Faction)
                {
                    tile.SetColor(Color.red, .7f);
                }
            }
        }
    }

    protected override void UnHighlightEnemyInAttackRange()
    {
        List<TerrainTile> attackRangeArea = TileManager.Instance.GetNeighboursIncludingCorners(TileBelow, attackRange.MaxStatValue);

        foreach (TerrainTile tile in attackRangeArea)
        {
            if (tile.someoneOnTop != null)
            {
                tile.SetColor(tile.baseColor);
            }
        }
    }

    public void Retaliate(IDamageable damageSource)
    {
        if (damageSource != null && !retaliated)
        {
            if(TryAttack(damageSource))
            {
                retaliated = true;
            }
        }
    }

    protected override void OnTurnStart()
    {
        base.OnTurnStart();

        retaliated = false;
    }

    public override void TakeDamage(DamageStruct damage, bool retaliate = false)
    {
        base.TakeDamage(damage);

        if(retaliate && health.CurrentStatValue > 0)
            Retaliate(damage.damageSource);
    }
}
