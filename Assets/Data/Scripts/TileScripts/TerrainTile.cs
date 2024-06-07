using UnityEngine;

public abstract class TerrainTile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;

    public int G;
    public int H;
    public int F { get { return G + H; } }
    public TerrainTile previousTile;

    public Vector2Int gridLocation;
    public GameObject highlightingObject;
    public Color baseColor;
    public IDamageable someoneOnTop;

    public bool walkable;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        baseColor = spriteRenderer.color;
    }

    protected virtual void OnMouseOver()
    {
        highlightingObject.SetActive(true);
    }

    protected virtual void OnMouseExit()
    {
        highlightingObject.SetActive(false);
    }

    public void SetColor(Color color, float alpha = 1f)
    {
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
