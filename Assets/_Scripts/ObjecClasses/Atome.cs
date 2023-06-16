[System.Serializable]
public class Atome 
{
    public string Nom { get; set; }
    public float Poids { get; set; }
    
    public Atome(string nom, float poids)
    {
        Nom = nom;
        Poids = poids;
    }
}

