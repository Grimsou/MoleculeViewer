using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("Paramètres de rotation")]
    [Tooltip("Objet à faire tourner.")]
    [SerializeField] private Transform targetObject;

    [Tooltip("Distance de la caméra par rapport à l'objet cible.")]
    [SerializeField] private float distanceFromTarget = 5f;

    [Tooltip("Vitesse de rotation horizontale.")]
    [SerializeField] private float horizontalRotationSpeed = 5f;

    [Tooltip("Vitesse de rotation verticale.")]
    [SerializeField] private float verticalRotationSpeed = 5f;

    [Tooltip("Vitesse de déplacement lissé.")]
    [SerializeField] private float smoothSpeed = 5f;

    private bool isRotating = false;
    private Quaternion initialRotation;
    private Vector3 desiredPosition;
    private Rigidbody targetRigidbody;

    private float rotationX;
    private float rotationY;

    /// <summary>
    /// Démarrage de la rotation de l'objet autour des axes horizontal et vertical.
    /// </summary>
    public void ActivateRotation()
    {
        isRotating = true;

        // Sauvegarde la rotation initiale de l'objet
        initialRotation = targetObject.rotation;

        // Récupère le composant Rigidbody de l'objet cible
        targetRigidbody = targetObject.GetComponent<Rigidbody>();

        // Désactive la gravité du rigidbody de l'objet cible
        if (targetRigidbody != null)
        {
            targetRigidbody.useGravity = false;
            targetRigidbody.isKinematic = true;
        }

        // Focalise la caméra sur l'objet cible
        desiredPosition = targetObject.position - targetObject.forward * distanceFromTarget;
        transform.position = desiredPosition;
        transform.LookAt(targetObject);
    }

    /// <summary>
    /// Désactive la rotation de l'objet.
    /// </summary>
    public void DeactivateRotation()
    {
        isRotating = false;

        // Réactive la gravité du rigidbody de l'objet cible
        if (targetRigidbody != null)
        {
            targetRigidbody.useGravity = true;
            targetRigidbody.isKinematic = false;
            float forceMagnitude = 15f; // Changez cette valeur selon vos préférences
            Vector3 randomDirection = Random.onUnitSphere;
            targetRigidbody.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (isRotating)
        {
            rotationX = Input.GetAxis("Vertical") * horizontalRotationSpeed;
            rotationY = Input.GetAxis("Horizontal") * verticalRotationSpeed;
        }
    }

    private void LateUpdate()
    {
        if (isRotating)
        {
            RotateObject();
        }
    }

    /// <summary>
    /// Fait tourner l'objet autour des axes horizontal et vertical.
    /// </summary>
    private void RotateObject()
    {
        // Rotation horizontale autour de l'objet cible
        Quaternion horizontalRotation = Quaternion.AngleAxis(rotationY * Time.deltaTime, Vector3.up);
        Quaternion verticalRotation = Quaternion.AngleAxis(rotationX * Time.deltaTime, Vector3.left);

        targetObject.rotation = horizontalRotation * verticalRotation * initialRotation;

        // Déplacement lissé de la caméra vers la position désirée
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
