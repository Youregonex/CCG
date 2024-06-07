using UnityEngine;
using System.Collections.Generic;

public class Building : MonoBehaviour, IHaveFaction, ICanGetStatusEffect
{
    [SerializeField] protected string buildingName;
    [SerializeField] protected bool isSelected;

    protected Stat health;

    public Dictionary<string, Stat> StatDir { get; set; } = new Dictionary<string, Stat>();
    public List<SECounter> appliedStatusEffects { get; set; } = new List<SECounter>();

    protected SpriteRenderer spriteRenderer;
    protected Color baseColor;

    [SerializeField] private Factions faction;
    public Factions Faction
    {
        get { return faction; }
        set { faction = value; }
    }

    public List<TerrainTile> tilesBelow;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        TurnManager.Instance.OnPlayerOneTurnStart += OnTurnStart;
    }

    protected void OnDestroy()
    {
        TurnManager.Instance.OnPlayerOneTurnStart -= OnTurnStart;
    }

    public void TakeDamage(DamageStruct damage, bool retaliate = false)
    {
        if (damage.damage < 0)
        {
            GetHealed(Mathf.Abs(damage.damage));
            return;
        }

        health.ModifyCurrentValue(damage.damage, Operator.Minus);

        TextManager.Instance.CreateText(transform.position, damage.damage, Color.red);

        if (health.CurrentStatValue <= 0)
        {
            health.ResetCurrentValue();
            Die(damage);
        }
    }

    public void GetHealed(int amount)
    {
        health.ModifyCurrentValue(amount, Operator.Add);

        if (health.CurrentStatValue >= health.MaxStatValue)
        {
            health.ResetCurrentValue();
        }

        TextManager.Instance.CreateText(transform.position, amount, Color.green);
    }

    private void Die(DamageStruct damage)
    {
        foreach(TerrainTile tile in tilesBelow)
        {
            tile.someoneOnTop = null;
        }

        Destroy(gameObject);
    }

    public void InitializeBuilding(string buildingName, int maxHealth, Sprite sprite, Factions faction)
    {
        this.buildingName = buildingName;

        health = new Stat("Health", maxHealth);
        StatDir.Add(health.StatName, health);

        this.faction = faction;
        spriteRenderer.sprite = sprite;
    }

    public void SetSelected()
    {
        isSelected = true;
        UnitManager.Instance.buildingSelected = true;
    }

    public void SetUnSelected()
    {
        isSelected = false;
        UnitManager.Instance.buildingSelected = false;
    }

    public void ApplyStatusEffectOnSelf(SECounter statusEffect)
    {
        appliedStatusEffects.Add(statusEffect);
    }

    public void RemoveAppliedStatusEffect(SECounter statusEffect)
    {
        appliedStatusEffects.Remove(appliedStatusEffects[appliedStatusEffects.IndexOf(statusEffect)]);
    }

    protected void ApllyStatusEffects()
    {
        if (appliedStatusEffects.Count > 0)
        {
            for (int i = 0; i < appliedStatusEffects.Count; i++)
            {
                appliedStatusEffects[i].Tick();
            }
        }
    }

    protected void OnTurnStart()
    {
        ApllyStatusEffects();
    }
}
