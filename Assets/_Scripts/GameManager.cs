using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
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

    private void Start()
    {
        // Désactiver les boutons de pause et d'arrêt au démarrage
        pauseButton.interactable = false;
        stopButton.interactable = false;
    }

    public void StartSimulation()
    {
        isSimulationRunning = true;
        isSimulationPaused = false;

        // Activer les boutons de pause et d'arrêt
        pauseButton.interactable = true;
        stopButton.interactable = true;

        // Logique pour démarrer la simulation
    }

    public void PauseSimulation()
    {
        isSimulationPaused = !isSimulationPaused;

        // Changer l'état du bouton de pause en fonction de la pause/lecture
        if (isSimulationPaused)
        {
            pauseButton.GetComponentInChildren<Text>().text = "Play";
        }
        else
        {
            pauseButton.GetComponentInChildren<Text>().text = "Pause";
        }

        // Logique pour mettre en pause la simulation
    }

    public void StopSimulation()
    {
        isSimulationRunning = false;
        isSimulationPaused = false;

        // Désactiver les boutons de pause et d'arrêt
        pauseButton.interactable = false;
        stopButton.interactable = false;

        // Logique pour arrêter la simulation
    }

    public void AddEventLog(string eventMessage)
    {
        eventLog.Add(eventMessage);
        UpdateEventLogText();
    }

    private void UpdateEventLogText()
    {
        // Afficher les événements les plus récents dans le journal des événements
        string logText = "";
        int startIndex = Mathf.Max(0, eventLog.Count - 5); // Afficher les 5 derniers événements
        for (int i = startIndex; i < eventLog.Count; i++)
        {
            logText += eventLog[i] + "\n";
        }
        eventLogText.text = logText;
    }

    public void ShowObjectData(GameObject obj)
    {
        selectedObject = obj;

        // Récupérer les données de l'objet sélectionné
        string objectName = obj.name;
        string objectWeight = obj.GetComponent<AtomeBehaviour>().AtomeData.Poids.ToString();

        // Mettre à jour les textes dans le panneau d'affichage des données de l'objet
        objectNameText.text = objectName;
        objectWeightText.text = objectWeight;

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
    }
}
