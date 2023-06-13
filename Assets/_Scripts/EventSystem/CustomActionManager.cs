using UnityEngine;
using UnityEngine.Events;

public class CustomActionManager : MonoBehaviour
{
    private static CustomActionManager instance;
    public static CustomActionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CustomActionManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("CustomActionManager");
                    instance = obj.AddComponent<CustomActionManager>();
                }
            }
            return instance;
        }
    }

    // Actions déclarées pour les événements
    public UnityAction<AtomeBehaviour, AtomeBehaviour> OnAtomCollide;
    public UnityAction<AtomeBehaviour> OnAtomDies;
    public UnityAction<Molecule> OnMoleculeSpawn;
    public UnityAction<GameObject> OnObjectSelected;
    public UnityAction OnSimulationStart;
    public UnityAction OnSimulationEnd;
    public UnityAction OnSimulationPause;

    // Méthodes pour déclencher les événements

    public void TriggerAtomCollide(AtomeBehaviour atom1, AtomeBehaviour atom2)
    {
        OnAtomCollide?.Invoke(atom1, atom2);
        Debug.Log("Atom Collide event triggered.");
    }

    public void TriggerAtomDies(AtomeBehaviour atom)
    {
        OnAtomDies?.Invoke(atom);
        Debug.Log("Atom Dies event triggered.");
    }

    public void TriggerMoleculeSpawn(Molecule molecule)
    {
        OnMoleculeSpawn?.Invoke(molecule);
        Debug.Log("Molecule Spawn event triggered.");
    }

    public void TriggerObjectSelected(GameObject selectedObject)
    {
        OnObjectSelected?.Invoke(selectedObject);
        Debug.Log("Object Selected event triggered.");
    }

    public void TriggerSimulationStart()
    {
        OnSimulationStart?.Invoke();
        Debug.Log("Simulation Start event triggered.");
    }

    public void TriggerSimulationEnd()
    {
        OnSimulationEnd?.Invoke();
        Debug.Log("Simulation End event triggered.");
    }

    public void TriggerSimulationPause()
    {
        OnSimulationPause?.Invoke();
        Debug.Log("Simulation Pause event triggered.");
    }
}
