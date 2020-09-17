using System;
using System.Collections.Generic;
using sgp.Models;
using sgp.Services;

namespace sgp
{
  class Program
  {
    static ControleService controleService = new ControleService();
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
        new Menu(8, "Excluir Produto", 2),
        new Menu(9, "Realizar Pedido", 3),
        new Menu(10, "Despachar Pedido", 3),
        new Menu(11, "Listar Pedidos", 3),
      };

      BuilderMenu builder = new BuilderMenu(menus);

      do
      {
        var idMenu = builder.Build();
        switch (idMenu)
        {
          case 4:
            controleService.CadastrarLoja();
            break;
          case 5:
            controleService.ExibirLojas();
            break;
          case 6:
            controleService.CadastrarProduto();
            break;
          case 7:
            controleService.ExibirProdutos();
            break;
          case 9:
            controleService.RealizarPedido();
            break;
          case 11:
            controleService.ExibirPedidos();
            break;

          default:
            Console.WriteLine("\n Opção não implementada.");
            Console.ReadKey();
            break;
        }
      } while (true);
    }
  }
}
