using System.Collections.Generic;
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

    // Événements
    public UnityAction OnSimulationStart;
    public UnityAction OnSimulationEnd;
    public UnityAction OnSimulationPause;
    public UnityAction<AtomeBehaviour, AtomeBehaviour> OnAtomCollide;
    public UnityAction<AtomeBehaviour> OnAtomDies;
    public UnityAction<Molecule> OnMoleculeSpawn;
    public UnityAction<GameObject> OnObjectSelected;

    private List<string> eventLog = new List<string>();

    // Méthodes pour déclencher les événements

    public void TriggerAtomCollide(AtomeBehaviour atom1, AtomeBehaviour atom2)
    {
        OnAtomCollide?.Invoke(atom1, atom2);
        Debug.Log("Atom Collide event triggered.");
        AddEventToLog("Atom Collide");
    }

    public void TriggerAtomDies(AtomeBehaviour atom)
    {
        OnAtomDies?.Invoke(atom);
        Debug.Log("Atom Dies event triggered.");
        AddEventToLog("Atom Dies");
    }

    public void TriggerMoleculeSpawn(Molecule molecule)
    {
        OnMoleculeSpawn?.Invoke(molecule);
        Debug.Log("Molecule Spawn event triggered.");
        AddEventToLog("Molecule Spawn");
    }

    public void TriggerObjectSelected(GameObject selectedObject)
    {
        OnObjectSelected?.Invoke(selectedObject);
        Debug.Log("Object Selected event triggered.");
        AddEventToLog("Object Selected");
    }

    public void TriggerSimulationStart()
    {
        OnSimulationStart?.Invoke();
        Debug.Log("Simulation Start event triggered.");
        AddEventToLog("Simulation Start");
    }

    public void TriggerSimulationEnd()
    {
        OnSimulationEnd?.Invoke();
        Debug.Log("Simulation End event triggered.");
        AddEventToLog("Simulation End");
    }

    public void TriggerSimulationPause()
    {
        OnSimulationPause?.Invoke();
        Debug.Log("Simulation Pause event triggered.");
        AddEventToLog("Simulation Pause");
    }

    public List<string> GetEventLog()
    {
        return eventLog;
    }

    private void AddEventToLog(string eventText)
    {
        eventLog.Add(eventText);

        // Limite la taille du journal à 5 événements
        if (eventLog.Count > 5)
        {
            eventLog.RemoveAt(0);
        }
    }
}
