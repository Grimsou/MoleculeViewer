using System;
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
    private bool isEventTriggering = false; // Indicateur pour éviter les événements pendant la destruction des objets

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Méthodes pour déclencher les événements

    public void TriggerAtomCollide(AtomeBehaviour atom1, AtomeBehaviour atom2)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnAtomCollide?.Invoke(atom1, atom2);
        Debug.Log("Atom Collide event triggered.");
        AddEventToLog("Atom Collide");

        isEventTriggering = false;
    }

    public void TriggerAtomDies(AtomeBehaviour atom)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnAtomDies?.Invoke(atom);
        Debug.Log("Atom Dies event triggered.");
        AddEventToLog("Atom Dies");

        isEventTriggering = false;
    }

    public void TriggerMoleculeSpawn(Molecule molecule)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnMoleculeSpawn?.Invoke(molecule);
        Debug.Log("Molecule Spawn event triggered.");
        AddEventToLog("Molecule Spawn");

        isEventTriggering = false;
    }

    public void TriggerObjectSelected(GameObject selectedObject)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnObjectSelected?.Invoke(selectedObject);
        Debug.Log("Object Selected event triggered.");
        AddEventToLog("Object Selected");

        isEventTriggering = false;
    }

    public void TriggerSimulationStart()
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnSimulationStart?.Invoke();
        Debug.Log("Simulation Start event triggered.");
        AddEventToLog("Simulation Start");

        isEventTriggering = false;
    }

    public void TriggerSimulationEnd()
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnSimulationEnd?.Invoke();
        Debug.Log("Simulation End event triggered.");
        AddEventToLog("Simulation End");

        isEventTriggering = false;
    }

    public void TriggerSimulationPause()
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnSimulationPause?.Invoke();
        Debug.Log("Simulation Pause event triggered.");
        AddEventToLog("Simulation Pause");

        isEventTriggering = false;
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
