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
        new Menu(4, "Resumo Geral", 0),
        new Menu(5, "Cadastrar Loja", 1),
        new Menu(6, "Lojas Cadastradas", 1),
        new Menu(7, "Cadastrar Produto", 2),
        new Menu(8, "Produtos Cadastrados", 2),
        new Menu(9, "Excluir Produto", 2),
        new Menu(10, "Realizar Pedido", 3),
        new Menu(11, "Despachar Pedido", 3),
        new Menu(12, "Pedidos Realizados", 3),
        new Menu(13, "Resumo de Vendas", 3),
      };

      BuilderMenu builder = new BuilderMenu(menus);

      do
      {
        var idMenu = builder.Build();
        switch (idMenu)
        {
          case 5:
            controleService.CadastrarLoja();
            break;
          case 6:
            controleService.ExibirLojas();
            break;
          case 7:
            controleService.CadastrarProduto();
            break;
          case 8:
            controleService.ExibirProdutos();
            break;
          case 10:
            controleService.RealizarPedido();
            break;
          case 12:
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
