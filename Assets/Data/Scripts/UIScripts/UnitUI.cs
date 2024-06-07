using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    private static UnitUI instance;
    public static UnitUI Instance { get { return instance; } }

    [SerializeField] private Image sprite;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text movementText;
    [SerializeField] private TMP_Text maxHealthText;
    [SerializeField] private TMP_Text maxMovementText;
    [SerializeField] private TMP_Text attackText;

    private void Awake()
    {
        gameObject.SetActive(false);
        instance = this;
    }

    private void UpdateMovement(int movement, int maxMovement)
    {

        if (movement < 0)
            movement = 0;

        movementText.text = movement.ToString();
        maxMovementText.text = maxMovement.ToString();
    }

    private void UpdateHealth(int health, int maxHealth)
    {
        healthText.text = health.ToString();
        maxHealthText.text = maxHealth.ToString();
    }

    private void UpdateAttack(int attack)
    {
        attackText.text = attack.ToString();
    }

    private void UpdateSprite(Sprite sprite)
    {
        this.sprite.sprite = sprite;
    }

    public void UpdateUnitUI(int health, int maxHealth, int attack, int movement, int maxMovement, Sprite sprite)
    {
        UpdateMovement(movement, maxMovement);
        UpdateHealth(health, maxHealth);
        UpdateSprite(sprite);
        UpdateAttack(attack);
        gameObject.SetActive(true);
    }

    private void Update()
    {
        bool instanceSelected = UnitManager.Instance.selectedInstance != null;
        gameObject.SetActive(instanceSelected);
    }


}
