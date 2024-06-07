using UnityEngine;

public class TreeHide : MonoBehaviour
{
    private Vector3Int gridCoord;

    private void Start()
    {
        gridCoord = TileManager.Instance.GetMapPosition(transform.position);
    }

    private void Update()
    {
        if(TileManager.Instance.tileGameObjects.ContainsKey((Vector2Int)gridCoord + new Vector2Int(0, 1)))
        {
            if(TileManager.Instance.tileGameObjects[(Vector2Int)gridCoord + new Vector2Int(0, 1)].someoneOnTop != null)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1,1,.5f);
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
        }
    }

}
