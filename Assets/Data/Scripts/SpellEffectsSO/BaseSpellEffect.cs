using UnityEngine;

public abstract class BaseSpellEffect : ScriptableObject
{
    public string spellName;
    public int range = 1;
    public GameObject spellEffectPrefab;
    public SpellShapes shape;
    public abstract void CastSpell(TerrainTile tile);
}
