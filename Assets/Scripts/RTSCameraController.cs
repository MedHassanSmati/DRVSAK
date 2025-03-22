using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 20f;
    public float edgeScrollSpeed = 30f;
    public float edgeThickness = 10f;
    public Vector2 moveLimitsX = new Vector2(-50, 50);
    public Vector2 moveLimitsZ = new Vector2(-50, 50);

    [Header("Zoom")]
    public Transform cameraPivot;
    public float zoomSpeed = 200f;
    public float minZoom = 10f;
    public float maxZoom = 50f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    public bool rightMouseRotation = true;

    private Camera mainCam;
    private float currentZoom;

    void Start()
    {
        mainCam = Camera.main;
        currentZoom = -cameraPivot.localPosition.z;
    }

    void Update()
    {
        HandleMovement();
        HandleEdgeScrolling();
        HandleZoom();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);

        ClampPosition();
    }

    void HandleEdgeScrolling()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 dir = Vector3.zero;

        if (mousePos.x <= edgeThickness) dir += Vector3.left;
        else if (mousePos.x >= Screen.width - edgeThickness) dir += Vector3.right;

        if (mousePos.y <= edgeThickness) dir += Vector3.back;
        else if (mousePos.y >= Screen.height - edgeThickness) dir += Vector3.forward;

        transform.Translate(dir.normalized * edgeScrollSpeed * Time.deltaTime, Space.World);

        ClampPosition();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed * Time.deltaTime;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        Vector3 newPos = cameraPivot.localPosition;
        newPos.z = -currentZoom;
        cameraPivot.localPosition = newPos;
    }

    void HandleRotation()
    {
        float rot = 0f;
        if (Input.GetKey(KeyCode.Q)) rot = -1f;
        else if (Input.GetKey(KeyCode.E)) rot = 1f;

        if (rightMouseRotation && Input.GetMouseButton(1))
        {
            rot += Input.GetAxis("Mouse X");
        }

        transform.Rotate(Vector3.up, rot * rotationSpeed * Time.deltaTime);
    }

    void ClampPosition()
    {
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, moveLimitsX.x, moveLimitsX.y);
        clampedPos.z = Mathf.Clamp(clampedPos.z, moveLimitsZ.x, moveLimitsZ.y);
        transform.position = clampedPos;
    }
}
