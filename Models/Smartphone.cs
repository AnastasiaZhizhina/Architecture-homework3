public class Smartphone
{

    public int Id { get; set; }


    public int ManufacturerId { get; set; }


    public Manufacturer? Manufacturer { get; set; }

    public string Model { get; set; } = string.Empty;


    public decimal Price { get; set; }
}