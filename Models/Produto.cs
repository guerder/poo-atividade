namespace sgp.Models
{
  public class Produto
  {
    public long Codigo { get; set; }
    public string Nome { get; set; }
    public double Preco { get; set; }

    public Produto(long codigo, string nome, double preco)
    {
      this.Codigo = codigo;
      this.Nome = nome;
      this.Preco = preco;
    }
  }
}