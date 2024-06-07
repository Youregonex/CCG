using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    private static TextManager instance;
    public static TextManager Instance { get { return instance; } }

    [SerializeField] private Transform damageTextPrefab;
    [SerializeField] private TMP_Text warningText;

    private float timer;
    private Color textColor;

    private Color baseColor;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        baseColor = warningText.color;
    }

    public void CreateText(Vector3 position, int damageAmmount, Color color, float textSpeed = 2f)
    {
        Transform obj = Instantiate(damageTextPrefab, position, Quaternion.identity);
        DamageText damageText = obj.GetComponent<DamageText>();
        damageText.Setup(damageAmmount, textSpeed, color);
    }

    public void ShowWarningText(string text, float timeToShow = 3f)
    {
        timer = timeToShow;
        warningText.text = text;
        textColor = baseColor;
        warningText.color = baseColor;
        //warningText.color = color;
        warningText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (timer < 0)
        {
            float disappearSpeed = 3f;

            textColor.a -= disappearSpeed * Time.deltaTime;
            warningText.color = textColor;

            if (textColor.a <= 0)
            {
                warningText.gameObject.SetActive(false);
            }
        }
        else textColor.a = 1f;

    }
}
