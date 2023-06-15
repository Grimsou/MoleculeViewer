using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Paramètres de Rotation")]
    [SerializeField] private float mouseSensitivity = 360.0f;

    [Header("Paramètres de Zoom")]
    [SerializeField] private float zoomSpeed = 10.0f;
    [SerializeField] private float minZoom = 10.0f;
    [SerializeField] private float maxZoom = 50.0f;

    [Header("Paramètres de Mouvement")]
    [SerializeField] private float moveSpeed = 25.0f;

    [Header("Touches de Contrôle")]
    [SerializeField] private KeyCode forwardKey = KeyCode.Z;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [SerializeField] private KeyCode upKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode downKey = KeyCode.LeftAlt;
    [SerializeField] private KeyCode leftKey = KeyCode.Q;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode resetKey = KeyCode.T;
    [SerializeField] private KeyCode alignKey = KeyCode.F;

    private Camera mainCamera;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        HandleMouseRotation();
        HandleZoom();
        HandleMovement();
        HandleResetPosition();
        HandleObjectAlignment();
    }

    private void HandleMouseRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.x -= mouseY;
            eulerRotation.y += mouseX;

            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }

    private void HandleZoom()
    {
        float zoomChange = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newSize = Mathf.Clamp(mainCamera.fieldOfView - zoomChange, minZoom, maxZoom);
        mainCamera.fieldOfView = newSize;
    }

    private void HandleMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(forwardKey))
        {
            direction += transform.forward;
        }
        if (Input.GetKey(backwardKey))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey(upKey))
        {
            direction += transform.up;
        }
        if (Input.GetKey(downKey))
        {
            direction -= transform.up;
        }
        if (Input.GetKey(leftKey))
        {
            direction -= transform.right;
        }
        if (Input.GetKey(rightKey))
        {
            direction += transform.right;
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void HandleResetPosition()
    {
        if (Input.GetKeyDown(resetKey))
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }

    private void HandleObjectAlignment()
    {
        if (Input.GetKeyDown(alignKey))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform selectedObject = hit.transform;
                CustomActionManager.Instance.TriggerObjectSelected(selectedObject.gameObject);
            }
        }
    }
}
