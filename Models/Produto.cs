using System;

namespace sgp.Models
{
  [Serializable]
  public class Produto
  {
    public int Codigo { get; set; }
    public string Nome { get; set; }
    public double Preco { get; set; }

    public Produto(int codigo, string nome, double preco)
    {
      this.Codigo = codigo;
      this.Nome = nome;
      this.Preco = preco;
    }
  }
}