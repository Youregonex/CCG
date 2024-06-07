using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    [SerializeField] private float targetZoom;
    [SerializeField] private float zoomFactor = 3f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float outlineSize = 5f;

    [SerializeField] private float cameraMovementSpeed;

    private Camera cam;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    private Vector3 cameraMovementDirection;
    
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        cam.transform.position = new Vector3(TileManager.Instance.GetSizeOfTheMap().x/2,
                                             TileManager.Instance.GetSizeOfTheMap().y/2,
                                             cam.transform.position.z);

        mapMinX = 0;
        mapMaxX = TileManager.Instance.GetSizeOfTheMap().x;
        mapMinY = 0;
        mapMaxY = TileManager.Instance.GetSizeOfTheMap().y;
    }

    private void Update()
    {
        cameraMovementDirection = Vector3.zero;
        ZoomCamera();
        MoveCamera();
    }

    private void ZoomCamera()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        transform.position = ClampCamera(transform.position);
    }

    private void MoveCamera()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        cameraMovementDirection = new Vector3(x, y);

        transform.position += cameraMovementDirection * cameraMovementSpeed * Time.deltaTime;
        transform.position = ClampCamera(transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth - outlineSize;
        float maxX = mapMaxX - camWidth + outlineSize;
        float minY = mapMinY + camHeight - outlineSize;
        float maxY = mapMaxY - camHeight + outlineSize;
        
        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY , transform.position.z);
    }
}
