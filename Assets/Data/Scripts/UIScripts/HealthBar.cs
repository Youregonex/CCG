using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image healthColor;

    public Transform healthBarObj;

    private void Start()
    {
        healthColor.color = GetComponent<Unit>().Faction == Factions.Player ? Color.green : Color.red;
    }

    public void UpdateHealthBar()
    {
        slider.maxValue = GetComponent<Unit>().StatDir["Health"].MaxStatValue;
        slider.value = GetComponent<Unit>().StatDir["Health"].CurrentStatValue;
    }
}
