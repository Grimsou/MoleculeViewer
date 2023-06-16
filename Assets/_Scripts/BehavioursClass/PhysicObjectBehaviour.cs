using UnityEngine;

public class PhysicObjectBehaviour : MonoBehaviour
{
    private bool isFrozen = false;
    private Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        FreezeObject();
    }

    /// <summary>
    /// Anime l'objet en le rendant dynamique et en lui appliquant une force aléatoire.
    /// </summary>
    public void AnimateObject()
    {
        if (isFrozen)
        {
            rb.isKinematic = false;
            float forceMagnitude = 15f; // Change this to whatever value you like
            Vector3 randomDirection = Random.onUnitSphere;
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
            rb.WakeUp();
            isFrozen = false;
        }
    }

    /// <summary>
    /// Immobilise l'objet en le rendant cinématique.
    /// </summary>
    public void FreezeObject()
    {
        if (!isFrozen)
        {
            rb.isKinematic = true;
            rb.Sleep();
            isFrozen = true;
        }
    }

    protected virtual void OnEnable()
    {
        // S'abonner aux événements de simulation
        CustomActionManager.Instance.OnSimulationStart += AnimateObject;
        CustomActionManager.Instance.OnSimulationPause += FreezeObject;
        CustomActionManager.Instance.OnSimulationEnd += FreezeObject;
    }

    protected virtual void OnDisable()
    {
        // Se désabonner des événements de simulation
        CustomActionManager.Instance.OnSimulationStart -= AnimateObject;
        CustomActionManager.Instance.OnSimulationPause -= FreezeObject;
        CustomActionManager.Instance.OnSimulationEnd -= FreezeObject;
    }
}