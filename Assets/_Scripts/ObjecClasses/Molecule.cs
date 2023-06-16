public class Molecule
{
    public string Nom { get; set; }
    public Atome Atome1 { get; set; }
    public Atome Atome2 { get; set; }

    // Molecule class constructor
    public Molecule(Atome atome1, Atome atome2)
    {
        Nom = "Default Molecule";
        Atome1 = atome1;
        Atome2 = atome2;
    }

    /// <summary>
    /// Add mass of both atoms to compute overall molecule mass
    /// </summary>
    /// <returns>return </returns>
    public float ComputeMass()
    {
        return Atome1.Poids + Atome2.Poids;
    }
}
