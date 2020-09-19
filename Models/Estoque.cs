using System;

namespace sgp.Models
{
  [Serializable]
  public class Estoque
  {
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public Loja Loja { get; set; }

    public Estoque(Produto produto, int quantidade, Loja loja)
    {
      this.Produto = produto;
      this.Quantidade = quantidade;
      this.Loja = loja;
    }
  }
}