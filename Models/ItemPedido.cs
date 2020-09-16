using System;

namespace sgp.Models
{
  [Serializable]
  public class ItemPedido
  {
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public double Desconto { get; set; }

    public ItemPedido(Produto produto, int quantidade, double desconto = 0)
    {
      this.Produto = produto;
      this.Quantidade = quantidade;
      this.Desconto = desconto;
    }

    public double ObterTotalItem()
    {
      var total = (Quantidade * Produto.Preco) * ((100 - Desconto) / 100);
      return total;
    }
  }
}