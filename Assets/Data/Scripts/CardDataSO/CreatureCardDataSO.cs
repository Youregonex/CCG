using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Card/Creature Card")]
public class CreatureCardDataSO : BaseCardDataSO
{
    public int health;
    public int attack;
    public int movement;
    public Unit unitPrefab;
    public Sprite unitSprite;
    public int attackRange;
    public DamageType damageType;
    public int attacksPerTurn;
    public RuntimeAnimatorController unitAnimController;
    public List<StatusEffect> statusEffects;
}
