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

    [Header("Événements")]
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

    /// <summary>
    /// Déclenche l'événement de collision entre deux atomes.
    /// </summary>
    /// <param name="atom1">Le premier atome en collision.</param>
    /// <param name="atom2">Le deuxième atome en collision.</param>
    public void TriggerAtomCollide(AtomeBehaviour atom1, AtomeBehaviour atom2)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnAtomCollide?.Invoke(atom1, atom2);
        Debug.Log("Événement Atom Collide déclenché.");
        AddEventToLog("Atom Collide");

        isEventTriggering = false;
    }

    /// <summary>
    /// Déclenche l'événement de mort d'un atome.
    /// </summary>
    /// <param name="atom">L'atome qui meurt.</param>
    public void TriggerAtomDies(AtomeBehaviour atom)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnAtomDies?.Invoke(atom);
        Debug.Log("Événement Atom Dies déclenché.");
        AddEventToLog("Atom Dies");

        isEventTriggering = false;
    }

    /// <summary>
    /// Déclenche l'événement de création d'une molécule.
    /// </summary>
    /// <param name="molecule">La molécule créée.</param>
    public void TriggerMoleculeSpawn(Molecule molecule)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnMoleculeSpawn?.Invoke(molecule);
        Debug.Log("Événement Molecule Spawn déclenché.");
        AddEventToLog("Molecule Spawn");

        isEventTriggering = false;
    }

    /// <summary>
    /// Déclenche l'événement de sélection d'un objet.
    /// </summary>
    /// <param name="selectedObject">L'objet sélectionné.</param>
    public void TriggerObjectSelected(GameObject selectedObject)
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnObjectSelected?.Invoke(selectedObject);
        Debug.Log("Événement Object Selected déclenché.");
        AddEventToLog("Object Selected");

        isEventTriggering = false;
    }

    /// <summary>
    /// Déclenche l'événement de démarrage de la simulation.
    /// </summary>
    public void TriggerSimulationStart()
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnSimulationStart?.Invoke();
        Debug.Log("Événement Simulation Start déclenché.");
        AddEventToLog("Simulation Start");

        isEventTriggering = false;
    }

    /// <summary>
    /// Déclenche l'événement de fin de la simulation.
    /// </summary>
    public void TriggerSimulationEnd()
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnSimulationEnd?.Invoke();
        Debug.Log("Événement Simulation End déclenché.");
        AddEventToLog("Simulation End");

        isEventTriggering = false;
    }

    /// <summary>
    /// Déclenche l'événement de mise en pause de la simulation.
    /// </summary>
    public void TriggerSimulationPause()
    {
        // Vérifier si la destruction des objets est déjà en cours
        if (isEventTriggering)
        {
            return;
        }

        isEventTriggering = true;

        OnSimulationPause?.Invoke();
        Debug.Log("Événement Simulation Pause déclenché.");
        AddEventToLog("Simulation Pause");

        isEventTriggering = false;
    }

    /// <summary>
    /// Récupère le journal des événements.
    /// </summary>
    /// <returns>La liste des événements.</returns>
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
