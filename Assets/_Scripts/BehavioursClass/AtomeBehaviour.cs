using UnityEngine;

public class AtomeBehaviour : MonoBehaviour
{
    [Header("Paramètres de l'atome")]
    [Tooltip("Nom de l'atome.")]
    [SerializeField] private string Nom;

    [Tooltip("Poids de l'atome.")]
    [SerializeField] private float Poids;

    [Tooltip("Préfabriqué de la molécule.")]
    public GameObject MoleculePrefab;

    private Atome AtomeData;

    private void Start()
    {
        AtomeData = new Atome(Nom, Poids);

        // Ajouter une force aléatoire à l'atome
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            float forceMagnitude = 15f; // Changez cette valeur selon vos préférences
            Vector3 randomDirection = Random.onUnitSphere;
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AtomeBehaviour otherAtome = collision.gameObject.GetComponent<AtomeBehaviour>();
        if (otherAtome != null && !otherAtome.gameObject.CompareTag("In Collision"))
        {
            Molecule molecule = new Molecule(AtomeData, otherAtome.AtomeData);

            // Instancier le préfabriqué de la molécule au point de collision
            GameObject newMoleculeObject = Instantiate(MoleculePrefab, collision.GetContact(0).point, Quaternion.identity);

            // Définir la MoleculeData du script MoleculeBehaviour sur la nouvelle molécule
            MoleculeBehaviour moleculeBehaviour = newMoleculeObject.GetComponent<MoleculeBehaviour>();
            moleculeBehaviour.MoleculeData = molecule;

            // Marquer les deux atomes comme étant "In Collision"
            gameObject.tag = "In Collision";
            otherAtome.gameObject.tag = "In Collision";

            // Supprimer les objets Atom originaux si nécessaire
            Destroy(gameObject, 0.1f);
            Destroy(otherAtome.gameObject, 0.1f);

            // Ajouter une force aléatoire à la nouvelle molécule
            Rigidbody rb = newMoleculeObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float forceMagnitude = 15f; // Changez cette valeur selon vos préférences
                Vector3 randomDirection = Random.onUnitSphere;
                rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
