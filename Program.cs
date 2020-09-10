using System;
using sgp.Models;

namespace sgp
{
  class Program
  {
    static void Main(string[] args)
    {
      Produto prod1 = new Produto(1, "Smartphone", 1005);
      Produto prod2 = new Produto(2, "Mouse", 199.50);
      Produto prod3 = new Produto(3, "SmartTV", 2475.2);
      Produto prod4 = new Produto(4, "Grill", 99.99);
      Produto prod5 = new Produto(5, "Kit de sabonetes", 14.99);

      Pedido pedido = new Pedido("Guerder Vieira", "Vendedor1");

      Console.WriteLine(pedido.ObterTotal());

      ItemPedido item1 = new ItemPedido(prod1, 1);
      ItemPedido item2 = new ItemPedido(prod2, 1);
      ItemPedido item3 = new ItemPedido(prod3, 1);
      ItemPedido item4 = new ItemPedido(prod4, 1);
      ItemPedido item5 = new ItemPedido(prod5, 1);

      pedido.Itens.Add(item1);
      pedido.Itens.Add(item2);
      pedido.Itens.Add(item3);
      pedido.Itens.Add(item4);
      pedido.Itens.Add(item5);

      Console.WriteLine(pedido.ObterTotal());
    }
  }
}
