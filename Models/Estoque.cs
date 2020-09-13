namespace sgp.Models
{
  public class Estoque
  {
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public Loja Loja { get; set; }
  }
}