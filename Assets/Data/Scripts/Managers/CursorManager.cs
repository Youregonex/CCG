using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite baseCursor;

    [SerializeField] private Vector2 cursorOffset = new Vector2(.1f, -.1f);
    [SerializeField] private Sprite redCursor;
    [SerializeField] private Sprite greenCursor;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        baseCursor = spriteRenderer.sprite;
        Cursor.visible = false;
    }

    public void Update()
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = position + cursorOffset;
    }

}
