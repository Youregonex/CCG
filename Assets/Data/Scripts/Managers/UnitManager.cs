using UnityEngine;
//using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    private static UnitManager instance;
    public static UnitManager Instance { get { return instance; } }

    [SerializeField] public ISelectable selectedInstance;

    //private List<Unit> playerOneUnits;
    //private List<Unit> playerTwoUnits;

    public Building buildingPreview;
    public Unit unitPreview;

    public bool unitSelected;
    public bool buildingSelected;

    private void Awake()
    {
        instance = this;
    }

    public void SelectInstance(ISelectable selectedInstance)
    {
        if (selectedInstance != null)
        {
            selectedInstance.SetSelected();
            this.selectedInstance = selectedInstance;
        }
    }

    public void UnSelectInstance(ISelectable selectedInstance)
    {
        if(this.selectedInstance == selectedInstance)
        {
            selectedInstance.SetUnSelected();
            this.selectedInstance = null;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(selectedInstance != null)
                UnSelectInstance(selectedInstance);
        }
    }

}
