namespace sgp.Models
{
  public class ItemPedido
  {
    public Produto Produto { get; set; }
    public long Quantidade { get; set; }
    public double Desconto { get; set; }

    public ItemPedido(Produto produto, long quantidade, double desconto = 0)
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