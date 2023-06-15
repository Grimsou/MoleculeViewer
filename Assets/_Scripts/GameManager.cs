using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private CustomActionManager eventManager;

    public int eventLogSize;

    public Button startButton;
    public Button pauseButton;
    public Button stopButton;
    public Text eventLogText;
    public GameObject objectDataPanel;
    public Text objectNameText;
    public Text objectWeightText;

    private bool isSimulationRunning = false;
    private bool isSimulationPaused = false;
    private List<string> eventLog = new List<string>();
    private GameObject selectedObject;

    private int selectableLayer;

    private List<AtomeBehaviour> atomBehaviours;
    private List<MoleculeBehaviour> moleculeBehaviours;

    private Dictionary<GameObject, ObjectData> objectDataMap = new Dictionary<GameObject, ObjectData>();
    private Dictionary<GameObject, ObjectState> objectStateMap = new Dictionary<GameObject, ObjectState>();
    private Dictionary<GameObject, Rigidbody> objectRigidbodyMap = new Dictionary<GameObject, Rigidbody>();

    private enum ObjectState
    {
        Active,
        Inactive
    }

    private class ObjectData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public Vector3 velocity;
        public Vector3 angularVelocity;

        public ObjectData(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 velocity,
            Vector3 angularVelocity)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
        }
    }

    private void Start()
    {
        eventManager = CustomActionManager.Instance;

        // Désactiver les boutons de pause et d'arrêt au démarrage
        pauseButton.interactable = false;
        stopButton.interactable = false;

        // Assigner les callbacks aux boutons
        startButton.onClick.AddListener(StartSimulation);
        pauseButton.onClick.AddListener(PauseSimulation);
        stopButton.onClick.AddListener(StopSimulation);

        // S'abonner aux événements de simulation
        eventManager.OnSimulationStart += OnSimulationStart;
        eventManager.OnSimulationEnd += OnSimulationEnd;
        eventManager.OnSimulationPause += OnSimulationPause;

        // S'abonner aux événements
        eventManager.OnAtomCollide += OnAtomCollide;
        eventManager.OnAtomDies += OnAtomDies;
        eventManager.OnMoleculeSpawn += OnMoleculeSpawn;

        selectableLayer = LayerMask.NameToLayer("Selectable");
    }

    public void StartSimulation()
    {
        isSimulationRunning = true;
        isSimulationPaused = false;

        // Activer les boutons de pause et d'arrêt
        pauseButton.interactable = true;
        stopButton.interactable = true;

        // Logique pour démarrer la simulation
        eventManager.TriggerSimulationStart();

        // Désactiver le bouton Start
        startButton.interactable = false;
    }

    public void PauseSimulation()
    {
        isSimulationPaused = !isSimulationPaused;

        // Changer l'état du bouton de pause en fonction de la pause/lecture
        if (isSimulationPaused)
        {
            pauseButton.GetComponentInChildren<Text>().text = "Play";
            //FreezeObjects();
        }
        else
        {
            pauseButton.GetComponentInChildren<Text>().text = "Pause";
            //ResumeObjects();
        }

        // Logique pour mettre en pause ou reprendre la simulation
        if (isSimulationPaused)
        {
            eventManager.TriggerSimulationPause();
        }
        else
        {
            eventManager.TriggerSimulationStart();
        }
    }

    public void StopSimulation()
    {
        isSimulationRunning = false;
        isSimulationPaused = false;

        // Désactiver les boutons de pause et d'arrêt
        pauseButton.interactable = false;
        stopButton.interactable = false;

        // Logique pour arrêter la simulation
        eventManager.TriggerSimulationEnd();
        ResetObjectPositions();
        ResetObjectDataAndState();
        RecreateMissingAtoms();

        // Réactiver le bouton Start
        startButton.interactable = true;
    }

    public void AddEventLog(string eventMessage)
    {
        eventLog.Add(eventMessage);

        // Limite la taille du journal à x événements
        if (eventLog.Count > eventLogSize)
        {
            eventLog.RemoveAt(0);
        }

        UpdateEventLogText();
    }

    private void UpdateEventLogText()
    {
        // Effacer le texte précédent
        eventLogText.text = "";

        // Parcourir la liste des événements enregistrés dans le GameManager
        foreach (string eventText in eventLog)
        {
            // Ajouter le texte de chaque événement au journal
            eventLogText.text += eventText + "\n";
        }
    }

    private void OnAtomCollide(AtomeBehaviour atom1, AtomeBehaviour atom2)
    {
        AddEventLog("Atom Collide");
    }

    private void OnAtomDies(AtomeBehaviour atom)
    {
        AddEventLog("Atom Dies");
    }

    private void OnMoleculeSpawn(Molecule molecule)
    {
        AddEventLog("Molecule Spawn");
    }

    private void OnSimulationStart()
    {
        AddEventLog("Simulation Start");
        // Mettez ici votre code pour traiter l'événement de démarrage de la simulation
    }

    private void OnSimulationEnd()
    {
        AddEventLog("Simulation End");
        // Mettez ici votre code pour traiter l'événement de fin de la simulation
    }

    private void OnSimulationPause()
    {
        AddEventLog("Simulation Pause");
        // Mettez ici votre code pour traiter l'événement de mise en pause de la simulation
    }

    public void ShowObjectData(GameObject obj)
    {
        if (obj.layer != selectableLayer)
        {
            Debug.Log("Object cannot be selected.");
            return;
        }

        selectedObject = obj;

        if (obj.TryGetComponent(out AtomeBehaviour atomBehaviour))
        {
            // Récupérer les données de l'objet Atome
            string objectName = atomBehaviour.AtomeData.Nom;
            string objectWeight = atomBehaviour.AtomeData.Poids.ToString();

            // Mettre à jour les textes dans le panneau d'affichage des données de l'objet
            objectNameText.text = objectName;
            objectWeightText.text = objectWeight;
        }
        else if (obj.TryGetComponent(out MoleculeBehaviour moleculeBehaviour))
        {
            // Récupérer les données de l'objet Molecule
            string objectName = moleculeBehaviour.MoleculeData.Nom;
            string objectWeight = moleculeBehaviour.MoleculeData.ComputeMass().ToString();

            // Mettre à jour les textes dans le panneau d'affichage des données de l'objet
            objectNameText.text = objectName;
            objectWeightText.text = objectWeight;
        }

        // Afficher le panneau d'affichage des données de l'objet
        objectDataPanel.SetActive(true);
    }

    public void HideObjectData()
    {
        // Cacher le panneau d'affichage des données de l'objet
        objectDataPanel.SetActive(false);
    }

    private void Update()
    {
        if (isSimulationRunning && !isSimulationPaused)
        {
            // Logique de mise à jour de la simulation en cours
        }

        // Vérifier si l'utilisateur a cliqué avec le bouton gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            // Créer un raycast à partir de la position du curseur de souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Lancer le raycast et vérifier s'il a frappé un objet avec le layer "Selectable"
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == selectableLayer)
            {
                // Sélectionner l'objet
                ShowObjectData(hit.collider.gameObject);
            }
        }
    }

    private void ResetObjectPositions()
    {
        foreach (KeyValuePair<GameObject, ObjectData> pair in objectDataMap)
        {
            GameObject obj = pair.Key;
            ObjectData objectData = pair.Value;

            if (obj != null)
            {
                obj.transform.position = objectData.position;
                obj.transform.rotation = objectData.rotation;
                obj.transform.localScale = objectData.scale;
            }
        }
    }

    private void ResetObjectDataAndState()
    {
        objectDataMap.Clear();
        objectStateMap.Clear();
        objectRigidbodyMap.Clear();
    }

    private void RegisterObject(GameObject obj)
    {
        if (obj.TryGetComponent(out AtomeBehaviour atomBehaviour))
        {
            if (atomBehaviours == null)
            {
                atomBehaviours = new List<AtomeBehaviour>();
            }

            atomBehaviours.Add(atomBehaviour);
        }
        else if (obj.TryGetComponent(out MoleculeBehaviour moleculeBehaviour))
        {
            if (moleculeBehaviours == null)
            {
                moleculeBehaviours = new List<MoleculeBehaviour>();
            }

            moleculeBehaviours.Add(moleculeBehaviour);
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            objectRigidbodyMap[obj] = rb;
        }

        objectDataMap[obj] = new ObjectData(obj.transform.position, obj.transform.rotation, obj.transform.localScale,
            rb.velocity, rb.angularVelocity);
        objectStateMap[obj] = ObjectState.Active;
    }

    public void RegisterAtom(AtomeBehaviour atomBehaviour)
    {
        RegisterObject(atomBehaviour.gameObject);
    }

    public void UnregisterAtom(AtomeBehaviour atomBehaviour)
    {
        if (atomBehaviours != null && atomBehaviours.Contains(atomBehaviour))
        {
            atomBehaviours.Remove(atomBehaviour);
        }

        UnregisterObject(atomBehaviour.gameObject);
    }

    public void RegisterMolecule(MoleculeBehaviour moleculeBehaviour)
    {
        RegisterObject(moleculeBehaviour.gameObject);
    }

    public void UnregisterMolecule(MoleculeBehaviour moleculeBehaviour)
    {
        if (moleculeBehaviours != null && moleculeBehaviours.Contains(moleculeBehaviour))
        {
            moleculeBehaviours.Remove(moleculeBehaviour);
        }

        UnregisterObject(moleculeBehaviour.gameObject);
    }

    private void UnregisterObject(GameObject obj)
    {
        if (objectDataMap.ContainsKey(obj))
        {
            objectDataMap.Remove(obj);
        }

        if (objectStateMap.ContainsKey(obj))
        {
            objectStateMap.Remove(obj);
        }

        if (objectRigidbodyMap.ContainsKey(obj))
        {
            objectRigidbodyMap.Remove(obj);
        }
    }

    public void RecreateMissingAtoms()
    {
        List<AtomeBehaviour> missingAtoms = new List<AtomeBehaviour>();

        foreach (AtomeBehaviour atomBehaviour in atomBehaviours)
        {
            if (atomBehaviour == null)
            {
                missingAtoms.Add(atomBehaviour);
            }
        }

        foreach (AtomeBehaviour missingAtom in missingAtoms)
        {
            GameObject atomPrefab = Resources.Load<GameObject>("Assets/Prefabs/AtomPrefab.prefab");
            GameObject newAtom =
                Instantiate(atomPrefab, missingAtom.transform.position, missingAtom.transform.rotation);
            newAtom.transform.localScale = missingAtom.transform.localScale;

            RegisterAtom(newAtom.GetComponent<AtomeBehaviour>());
        }

    }
}
