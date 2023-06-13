using UnityEngine;

public class AtomeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject MoleculePrefab;
    [SerializeField] private string Nom;
    [SerializeField] private float Poids;

    public Atome AtomeData { get; set; }

    private void Start()
    {
        AtomeData = new Atome(Nom, Poids);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            float forceMagnitude = 15f; // Change this to whatever value you like
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
                float forceMagnitude = 15f; // Change this to whatever value you like
                Vector3 randomDirection = Random.onUnitSphere;
                rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
            }

            // Déclencher l'action OnMoleculeSpawn
            CustomActionManager.Instance.TriggerMoleculeSpawn(newMoleculeObject.GetComponent<MoleculeBehaviour>().MoleculeData);
        }
    }

    private void OnDestroy()
    {
        // Déclencher l'action OnAtomDies
        CustomActionManager.Instance.TriggerAtomDies(this);
    }
}
