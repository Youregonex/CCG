using UnityEngine;

public class UnWalkableTile : TerrainTile
{

    protected override void OnMouseOver()
    {
        base.OnMouseOver();
        if(UnitManager.Instance.buildingPreview != null)
            UnitManager.Instance.buildingPreview.gameObject.SetActive(false);

        if (UnitManager.Instance.unitPreview != null)
            UnitManager.Instance.unitPreview.gameObject.SetActive(false);

    }
}
