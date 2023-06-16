using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Paramètres de Rotation")]
    [SerializeField, Tooltip("Sensibilité de la souris pour la rotation")] private float mouseSensitivity = 360.0f;

    [Header("Paramètres de Zoom")]
    [SerializeField, Tooltip("Vitesse de zoom")] private float zoomSpeed = 10.0f;
    [SerializeField, Tooltip("Zoom minimum")] private float minZoom = 10.0f;
    [SerializeField, Tooltip("Zoom maximum")] private float maxZoom = 50.0f;

    [Header("Paramètres de Mouvement")]
    [SerializeField, Tooltip("Vitesse de déplacement")] private float moveSpeed = 25.0f;

    [Header("Touches de Contrôle")]
    [SerializeField, Tooltip("Touche pour avancer")] private KeyCode forwardKey = KeyCode.Z;
    [SerializeField, Tooltip("Touche pour reculer")] private KeyCode backwardKey = KeyCode.S;
    [SerializeField, Tooltip("Touche pour monter")] private KeyCode upKey = KeyCode.LeftShift;
    [SerializeField, Tooltip("Touche pour descendre")] private KeyCode downKey = KeyCode.LeftAlt;
    [SerializeField, Tooltip("Touche pour aller à gauche")] private KeyCode leftKey = KeyCode.Q;
    [SerializeField, Tooltip("Touche pour aller à droite")] private KeyCode rightKey = KeyCode.D;
    [SerializeField, Tooltip("Touche pour réinitialiser la position")] private KeyCode resetKey = KeyCode.T;

    private Camera mainCamera;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    /// <summary>
    /// Initialise les valeurs de la caméra au démarrage.
    /// </summary>
    void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// Gère la rotation de la caméra en fonction du mouvement de la souris.
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
    /// Gère le zoom de la caméra en fonction de la molette de la souris.
    /// </summary>
    private void HandleZoom()
    {
        float zoomChange = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newSize = Mathf.Clamp(mainCamera.fieldOfView - zoomChange, minZoom, maxZoom);
        mainCamera.fieldOfView = newSize;
    }

    /// <summary>
    /// Gère le déplacement de la caméra en fonction des touches de contrôle.
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
    /// Réinitialise la position et la rotation de la caméra.
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
    /// Appelée à chaque frame après toutes les autres mises à jour.
    /// Gère les différentes fonctionnalités de la caméra.
    /// </summary>
    void LateUpdate()
    {
        HandleMouseRotation();
        HandleZoom();
        HandleMovement();
        HandleResetPosition();
    }
}
