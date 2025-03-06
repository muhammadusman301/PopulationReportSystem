public class Country
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Continent { get; set; }
    public string Region { get; set; }
    public int Population { get; set; }
    public int Capital { get; set; }

    // Ensure there is a parameterless constructor
    public Country() { }
}
