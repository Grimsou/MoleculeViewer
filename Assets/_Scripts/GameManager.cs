using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private CustomActionManager eventManager;

    [Header("Variables d'événement")]
    public int eventLogSize;
    private bool isUpdatingEventLog = false; // Ajout d'un indicateur pour empêcher les mises à jour simultanées
    private List<string> eventLog = new List<string>();

    [Header("Boutons")]
    public Button startButton;
    public Button pauseButton;
    public Button stopButton;
    public Button quitButton;

    [Header("Panneaux")]
    public GameObject objectDataPanel;
    public GameObject menuPanel;

    [Header("Textes")]
    [Tooltip("Texte pour le nom de l'objet")]
    public Text objectNameText;
    [Tooltip("Texte pour le journal d'événements")]
    public Text eventLogText;
    [Tooltip("Texte pour le poids de l'objet")]
    public Text objectWeightText;
    [Tooltip("Texte pour les contrôles de caméra")]
    public Text cameraControlsText;

    private bool isSimulationRunning = false;
    private bool isSimulationPaused = false;
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
        DontDestroyOnLoad(this);
        eventManager = CustomActionManager.Instance;

        // Désactiver les boutons de pause et d'arrêt au démarrage
        pauseButton.interactable = false;
        stopButton.interactable = false;
        quitButton.interactable = false;

        // Assigner les callbacks aux boutons
        startButton.onClick.AddListener(StartSimulation);
        pauseButton.onClick.AddListener(PauseSimulation);
        stopButton.onClick.AddListener(StopSimulation);
        quitButton.onClick.AddListener(QuitApplication);

        // Désactiver le panneau du menu au démarrage
        menuPanel.SetActive(false);

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

    /// <summary>
    /// Démarre la simulation.
    /// </summary>
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

    /// <summary>
    /// Met en pause ou reprend la simulation.
    /// </summary>
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

    /// <summary>
    /// Arrête la simulation.
    /// </summary>
    public void StopSimulation()
    {
        isSimulationRunning = false;
        isSimulationPaused = false;

        // Désactiver les boutons de pause et d'arrêt
        pauseButton.interactable = false;
        stopButton.interactable = false;

        // Logique pour arrêter la simulation
        eventManager.TriggerSimulationEnd();

        // Recharger la scène complète pour réinitialiser la simulation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Ajoute un événement au journal d'événements.
    /// </summary>
    /// <param name="eventMessage">Le message de l'événement.</param>
    public void AddEventLog(string eventMessage)
    {
        eventLog.Add(eventMessage);

        // Limite la taille du journal à x événements
        if (eventLog.Count > eventLogSize)
        {
            eventLog.RemoveAt(0);
        }

        // Mettre à jour le texte du journal uniquement si aucune mise à jour n'est déjà en cours
        if (!isUpdatingEventLog)
        {
            UpdateEventLogText();
        }
    }

    private void UpdateEventLogText()
    {
        if(eventLogText != null)
        {
            // Vérifier si une mise à jour du journal d'événements est déjà en cours
            if (isUpdatingEventLog)
            {
                return;
            }

            // Marquer la mise à jour du journal d'événements comme en cours
            isUpdatingEventLog = true;

            // Effacer le texte précédent
            eventLogText.text = "";

            // Parcourir la liste des événements enregistrés dans le GameManager
            foreach (string eventText in eventLog)
            {
                // Ajouter le texte de chaque événement au journal
                eventLogText.text += eventText + "\n";
            }

            // Marquer la mise à jour du journal d'événements comme terminée
            isUpdatingEventLog = false;
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

        // Vérifier si l'utilisateur a appuyé sur la touche ESC pour afficher le menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
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
            GameObject atomPrefab = Resources.Load<GameObject>("Prefabs/AtomPrefab");
            GameObject newAtom = Instantiate(atomPrefab, missingAtom.transform.position, missingAtom.transform.rotation);
            newAtom.transform.localScale = missingAtom.transform.localScale;

            RegisterAtom(newAtom.GetComponent<AtomeBehaviour>());
        }
    }

    private void ToggleMenu()
    {
        // Inverser l'état d'activation du panneau du menu
        menuPanel.SetActive(!menuPanel.activeSelf);

        // Afficher les touches du contrôleur de caméra
        // Remplacez le texte suivant par vos propres instructions
        cameraControlsText.text = "Contrôles de la caméra :\n\n" +
                                  "Z : Avancer\n\n" +
                                  "S : Reculer\n\n" +
                                  "Q : Aller à gauche\n\n" +
                                  "D : Aller à droite\n\n" +
                                  "Clic droit de la souris : Rotation de la caméra\n\n" +
                                  "Scroll de la souris : Zoom in/out\n\n"+
                                  "LeftShit et LeftCtrl : Elevation de la caméra";


        // Si le panneau du menu est activé, afficher les contrôles de la caméra et désactiver les interactions du jeu
        if (menuPanel.activeSelf)
        {
            Time.timeScale = 0f; // Mettre en pause le jeu
            cameraControlsText.gameObject.SetActive(true);
            quitButton.interactable = true;
        }
        else
        {
            Time.timeScale = 1f; // Reprendre le jeu
            cameraControlsText.gameObject.SetActive(false);
            quitButton.interactable = false;
        }


        // Mettre en pause la simulation si elle est en cours
        if (isSimulationRunning && !isSimulationPaused)
        {
            PauseSimulation();
        }
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
