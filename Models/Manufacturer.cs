using System.Collections.Generic;


public class Manufacturer
{

    public int Id { get; set; }


    public string Name { get; set; } = string.Empty;

    public ICollection<Smartphone> Smartphones { get; set; } = new List<Smartphone>();
}