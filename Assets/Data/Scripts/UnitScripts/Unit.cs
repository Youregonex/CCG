using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour, IHaveFaction, ICanGetStatusEffect
{
    //-------------Unit Stats-----------------//
    [SerializeField] protected string unitName;
    [SerializeField] protected DamageType damageType;

    protected Stat attackDamage;
    protected Stat movement;
    protected Stat health;
    protected Stat attacksPerTurn;
    protected Stat attackRange;
    
    public Dictionary<string, Stat> StatDir { get; set; } = new Dictionary<string, Stat>();
    //----------------------------------------//

    [SerializeField] protected float speed = 4f;
    [SerializeField] protected bool isSelected;

    protected SpriteRenderer spriteRenderer;
    protected Animator unitAnimator;
    protected HealthBar unitHealthBar;
    protected Color baseColor;
    protected bool isPlaced = false;

    [SerializeField] private Factions faction;
    public Factions Faction
    {
        get { return faction; }
        set {faction = value; }
    }

    protected RangeFinder rangeFinder = new RangeFinder();
    public List<TerrainTile> TilesInRange { get; protected set; } = new List<TerrainTile>();

    public PathFinder pathFinder = new PathFinder();
    public List<TerrainTile> path = new List<TerrainTile>();

    public List<StatusEffect> attackStatusEffects = new List<StatusEffect>();
    public List<SECounter> appliedStatusEffects { get; set; } = new List<SECounter>();
    
    public bool IsMoving { get; protected set; }

    public TerrainTile TileBelow;


    protected void Awake()
    {
        unitHealthBar = GetComponent<HealthBar>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        unitAnimator = GetComponent<Animator>();
        TurnManager.Instance.OnPlayerOneTurnStart += OnTurnStart;
    }

    protected void OnDestroy()
    {
        TurnManager.Instance.OnPlayerOneTurnStart -= OnTurnStart;
    }

    protected virtual void Start()
    {
        baseColor = spriteRenderer.color;
        IsMoving = false;
    }

    protected void Update()
    {
        IsMoving = path.Count > 0;
        spriteRenderer.flipX = false;

        if (IsMoving)
        {
            MoveAlongPath();
        }
    }

    protected void MoveAlongPath()
    {
        float step = speed * Time.deltaTime;

        if (path[0].transform.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        if(movement.CurrentStatValue > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
        }
        else
        {
            path.Clear();
        }

        if(Vector2.Distance(transform.position, path[0].transform.position) < .0001f)
        {
            movement.ModifyCurrentValue(1, Operator.Minus);
            path[0].someoneOnTop = this;
            TileBelow.someoneOnTop = null;
            PositionUnitOnTile(path[0]);
            path.RemoveAt(0);
        }
    }

    protected void GetTilesInRange(TerrainTile originTile, int range)
    {
        TilesInRange = rangeFinder.GetTilesInRange(originTile, range);

        List<TerrainTile> tilesInRangeCopy = new List<TerrainTile>();

        foreach(TerrainTile tile in TilesInRange)
        {
            tilesInRangeCopy.Add(tile);
        }

        foreach (TerrainTile tile in tilesInRangeCopy)
        {
            if((pathFinder.FindPath(TileBelow, tile, TilesInRange).Count > movement.CurrentStatValue) ||
               (pathFinder.FindPath(TileBelow, tile, TilesInRange).Count == 0 && tile.someoneOnTop == null))
            {
                TilesInRange.Remove(tile);
            }
        }

        foreach (TerrainTile tile in TilesInRange)
        {
            if (tile.someoneOnTop != null)
            {
                if(((IHaveFaction)tile.someoneOnTop).Faction == Faction)
                {
                    tile.SetColor(Color.gray, .8f);
                }
            }
            else
            {
                tile.SetColor(Color.green, .8f);
            }
        }
    }

    protected void HideTilesInRange()
    {
        if(TilesInRange.Count > 0)
        {
            //Set color for tiles in range to original color
            foreach (TerrainTile tile in TilesInRange)
            {
                tile.SetColor(tile.baseColor);
            }
        }
    }

    public bool TrySetDistanation(TerrainTile tile)
    {
        if(pathFinder.FindPath(TileBelow, tile, TilesInRange).Count <= movement.CurrentStatValue)
            path = pathFinder.FindPath(TileBelow, tile, TilesInRange);

        return path.Count > 0 && path.Count <= movement.CurrentStatValue;
    }

    public virtual void TakeDamage(DamageStruct damage, bool retaliate = false)
    {
        if(damage.damage < 0)
        {
            GetHealed(Mathf.Abs(damage.damage));
            return;
        }

        health.ModifyCurrentValue(damage.damage, Operator.Minus);
        unitAnimator.SetTrigger("TookDamage");

        TextManager.Instance.CreateText(transform.position, damage.damage, Color.red);

        if (health.CurrentStatValue <= 0)
        {
            Die(damage);
            return;
        }

        unitHealthBar.UpdateHealthBar();
    }

    public void GetHealed(int amount)
    {
        health.ModifyCurrentValue(amount, Operator.Add);

        if (health.CurrentStatValue >= health.MaxStatValue)
        {
            health.ResetCurrentValue();
        }

        TextManager.Instance.CreateText(transform.position, amount, Color.green);

        unitHealthBar.UpdateHealthBar();
    }

    public void Die(DamageStruct damage)
    {
        if(damage.damageSource != null)
            Debug.Log($"Died from {damage.damageSource.name} attack");

        TileBelow.someoneOnTop = null;
        TileBelow.SetColor(TileBelow.baseColor);

        Destroy(gameObject);
    }

    protected void PositionUnitOnTile(TerrainTile tile)
    {
        transform.position = tile.transform.position;
        TileBelow = tile;
        tile.someoneOnTop = this;
    }

    protected virtual void OnTurnStart()
    {
        StatusEffectTick();

        movement.ResetCurrentValue();
        attacksPerTurn.ResetCurrentValue();
    }

    protected void StatusEffectTick()
    {
        if (appliedStatusEffects.Count > 0)
        {
            for (int i = 0; i < appliedStatusEffects.Count; i++)
            {
                appliedStatusEffects[i].Tick();
            }
        }
    }

    public void RemoveAppliedStatusEffect(SECounter statusEffect)
    {
        appliedStatusEffects.Remove(appliedStatusEffects[appliedStatusEffects.IndexOf(statusEffect)]);
    }

    public void InitializeUnit(string unitName, int attackDamage, int attacksPerTurn, int maxHealth, int maxMovement,
                               Sprite sprite, int attackRange, DamageType damageType, Factions faction,
                               RuntimeAnimatorController unitAnimController, List<StatusEffect> statusEffects)
    {
        this.unitName = unitName;
        this.damageType = damageType;
        this.faction = faction;
        
        this.attackDamage = new Stat("AttackDamage", attackDamage);
        this.health = new Stat("Health", maxHealth);
        this.movement = new Stat("Movement", maxMovement);
        this.attacksPerTurn = new Stat("AttacksPerTurn", attacksPerTurn);
        this.attackRange = new Stat("AttackRange", attackRange);

        StatDir.Add(this.attackDamage.StatName, this.attackDamage);
        StatDir.Add(this.health.StatName, this.health);
        StatDir.Add(this.movement.StatName, this.movement);
        StatDir.Add(this.attacksPerTurn.StatName, this.attacksPerTurn);
        StatDir.Add(this.attackRange.StatName, this.attackRange);

        spriteRenderer.sprite = sprite;
        unitAnimator.runtimeAnimatorController = unitAnimController;
        isPlaced = true;
        unitHealthBar.healthBarObj.gameObject.SetActive(true);

        foreach(MaxValueStatusEffect statusEffect in statusEffects)
        {
            attackStatusEffects.Add(statusEffect);
        }
    }

    public void SetColor(Color color, float alpha = 1f)
    {
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public void SetSelected()
    {
        if(!IsMoving)
        {
            isSelected = true;
            UnitManager.Instance.unitSelected = true;
            GetTilesInRange(TileBelow, movement.CurrentStatValue);
            UnitUI.Instance.UpdateUnitUI(health.CurrentStatValue, health.MaxStatValue, attackDamage.MaxStatValue, movement.CurrentStatValue, movement.MaxStatValue, spriteRenderer.sprite);
            HighlightEnemyInAttackRange();
        }
    }

    public void SetUnSelected()
    {
        isSelected = false;
        UnitManager.Instance.unitSelected = false;
        HideTilesInRange();
        UnHighlightEnemyInAttackRange();
    }

    public void ApplyStatusEffectOnSelf(SECounter statusEffect)
    {
        appliedStatusEffects.Add(statusEffect);
    }

    public void ApplyStatusEffectToTarget(ICanGetStatusEffect target)
    {
        for(int i = 0; i < attackStatusEffects.Count; i++)
        {
            SECounter newEffect = new SECounter(attackStatusEffects[i], target);
            target.ApplyStatusEffectOnSelf(newEffect);
        }
    }

    public void GetAttackStatusEffect(MaxValueStatusEffect statusEffect)
    {
        attackStatusEffects.Add(statusEffect);
    }

    public abstract bool TryAttack(IDamageable target, bool retaliate = false);
    protected abstract bool InRangeOfAttack(IDamageable target);
    protected abstract void HighlightEnemyInAttackRange();
    protected abstract void UnHighlightEnemyInAttackRange();
}
