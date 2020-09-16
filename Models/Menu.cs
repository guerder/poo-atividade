namespace sgp.Models
{
  public class Menu
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Parent { get; set; }

    public Menu(int id, string name, int parent)
    {
      this.Id = id;
      this.Name = name;
      this.Parent = parent;
    }
  }
}