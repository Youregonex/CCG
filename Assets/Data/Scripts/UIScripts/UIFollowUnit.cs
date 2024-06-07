using UnityEngine;

public class UIFollowUnit : MonoBehaviour
{
   [SerializeField] private Transform unitToFollow;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (unitToFollow != null)
            rectTransform.position = unitToFollow.localPosition;
    }
}
