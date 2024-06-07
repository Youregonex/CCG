using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{ 
    private TextMeshPro textMesh;
    private float ySpeed;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, float textSpeed, Color color)
    {
        textMesh.SetText($"{Mathf.Abs(damageAmount)}");
        ySpeed = textSpeed;
        textMesh.color = color;
        textColor = textMesh.color;
        disappearTimer = 1f;
    }

    private void Update()
    {
        transform.position += new Vector3(0, ySpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;

        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
                Destroy(gameObject);
        }
    }
}
