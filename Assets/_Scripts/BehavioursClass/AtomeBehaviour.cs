using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomeBehaviour : MonoBehaviour
{
    [SerializeField]
    public string Nom;
    [SerializeField]
    public float Poids;

    public GameObject MoleculePrefab; // The Molecule prefab

    private Atome AtomeData;

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

            // Instantiate the Molecule prefab at the collision point
            GameObject newMoleculeObject = Instantiate(MoleculePrefab, collision.GetContact(0).point, Quaternion.identity);

            // Set the MoleculeBehaviour's MoleculeData to the new molecule
            MoleculeBehaviour moleculeBehaviour = newMoleculeObject.GetComponent<MoleculeBehaviour>();
            moleculeBehaviour.MoleculeData = molecule;

            // Tag both atoms as "In Collision"
            gameObject.tag = "In Collision";
            otherAtome.gameObject.tag = "In Collision";

            // Optionally, destroy the original Atom objects
            Destroy(gameObject, 0.1f);
            Destroy(otherAtome.gameObject, 0.1f);

            // Add a random force to the new Molecule
            Rigidbody rb = newMoleculeObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float forceMagnitude = 15f; // Change this to whatever value you like
                Vector3 randomDirection = Random.onUnitSphere;
                rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }


}



