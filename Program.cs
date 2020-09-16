using System;
using System.Collections.Generic;
using System.Linq;
using sgp.Models;
using sgp.Models.Enums;

namespace sgp
{
  class Program
  {
    static void Main(string[] args)
    {

      var menus = new List<Menu>(){
        new Menu(1, "Lojas", 0),
        new Menu(2, "Produtos", 0),
        new Menu(3, "Pedidos", 0),
        new Menu(4, "Cadastrar Loja", 1),
        new Menu(5, "Visualizar Lojas", 1),
        new Menu(6, "Cadastrar Produto", 2),
        new Menu(7, "Visualizar Produtos", 2),
        new Menu(8, "Realizar Pedido", 3),
        new Menu(9, "Despachar Pedido", 3),
      };


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

      pedido.AdicionarItem(item1);
      pedido.AdicionarItem(item2);
      pedido.AdicionarItem(item3);
      pedido.AdicionarItem(item4);
      pedido.AdicionarItem(item5);

      Console.WriteLine((Status)1);

      BuilderMenu builder = new BuilderMenu(menus);

      builder.Build();
    }
  }
}
