using UnityEngine;

public class MoleculeBehaviour : PhysicObjectBehaviour
{
    public Molecule MoleculeData { get; set; }

    protected override void Start()
    {
        base.Start();

        var rb = GetComponent<Rigidbody>();
        
        AnimateObject();
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