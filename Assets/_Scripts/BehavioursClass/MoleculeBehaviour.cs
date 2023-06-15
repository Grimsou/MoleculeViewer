using UnityEngine;

public class MoleculeBehaviour : PhysicObjectBehaviour
{
    public Molecule MoleculeData { get; set; }

    protected override void Start()
    {
        base.Start();

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            float forceMagnitude = 15f; // Change this to whatever value you like
            Vector3 randomDirection = Random.onUnitSphere;
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // S'abonner aux événements de simulation
        CustomActionManager.Instance.OnSimulationStart += AnimateObject;
        CustomActionManager.Instance.OnSimulationPause += FreezeObject;
        CustomActionManager.Instance.OnSimulationEnd += FreezeObject;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // Se désabonner des événements de simulation
        CustomActionManager.Instance.OnSimulationStart -= AnimateObject;
        CustomActionManager.Instance.OnSimulationPause -= FreezeObject;
        CustomActionManager.Instance.OnSimulationEnd -= FreezeObject;
    }
}