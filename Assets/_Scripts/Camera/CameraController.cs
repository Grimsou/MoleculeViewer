using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Paramètres de Rotation")]
    [Tooltip("Sensibilité de la rotation de la souris.")]
    [SerializeField] private float mouseSensitivity = 360.0f;

    [Header("Paramètres de Zoom")]
    [Tooltip("Vitesse de zoom de la caméra.")]
    [SerializeField] private float zoomSpeed = 10.0f;
    [Tooltip("Niveau de zoom minimum autorisé.")]
    [SerializeField] private float minZoom = 10.0f;
    [Tooltip("Niveau de zoom maximum autorisé.")]
    [SerializeField] private float maxZoom = 50.0f;

    [Header("Paramètres de Mouvement")]
    [Tooltip("Vitesse de déplacement de la caméra.")]
    [SerializeField] private float moveSpeed = 25.0f;

    [Header("Touches de Contrôle")]
    [Tooltip("Touche pour avancer.")]
    [SerializeField] private KeyCode forwardKey = KeyCode.Z;
    [Tooltip("Touche pour reculer.")]
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [Tooltip("Touche pour monter.")]
    [SerializeField] private KeyCode upKey = KeyCode.LeftShift;
    [Tooltip("Touche pour descendre.")]
    [SerializeField] private KeyCode downKey = KeyCode.LeftAlt;
    [Tooltip("Touche pour se déplacer à gauche.")]
    [SerializeField] private KeyCode leftKey = KeyCode.Q;
    [Tooltip("Touche pour se déplacer à droite.")]
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [Tooltip("Touche pour réinitialiser la position de la caméra.")]
    [SerializeField] private KeyCode resetKey = KeyCode.T;
    [Tooltip("Touche pour aligner la caméra avec un objet.")]
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

    /// <summary>
    /// Handles the rotation of the camera based on mouse input.
    /// </summary>
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



    /// <summary>
    /// Handles the zoom functionality of the camera.
    /// </summary>
    private void HandleZoom()
    {
        float zoomChange = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newSize = Mathf.Clamp(mainCamera.fieldOfView - zoomChange, minZoom, maxZoom);
        mainCamera.fieldOfView = newSize;
    }

    /// <summary>
    /// Handles the movement of the camera based on keyboard input.
    /// </summary>
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

    /// <summary>
    /// Resets the camera to its initial position and rotation when the reset key is pressed.
    /// </summary>
    private void HandleResetPosition()
    {
        if (Input.GetKeyDown(resetKey))
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }

    /// <summary>
    /// Aligns the camera with the selected object when the align key is pressed.
    /// </summary>
    private void HandleObjectAlignment()
    {
        if (Input.GetKeyDown(alignKey))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform selectedObject = hit.transform;
                AlignWithObject(selectedObject);
            }
        }
    }

    /// <summary>
    /// Aligns the camera with the given target object.
    /// </summary>
    public void AlignWithObject(Transform target)
    {
        transform.LookAt(target);
    }
}
