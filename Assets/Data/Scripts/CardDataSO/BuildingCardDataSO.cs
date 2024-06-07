using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Card/Building Card")]

public class BuildingCardDataSO : BaseCardDataSO
{
    public int size;
    public int health;
    public Building buildingPrefab;
    public Sprite buildingSprite;
    public SpellShapes buildingShape;
}
