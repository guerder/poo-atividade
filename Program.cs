﻿using System;
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
        new Menu(4, "Setor de Entregas", 0),
        new Menu(5, "Resumo Geral", 0),
        new Menu(6, "Cadastrar Loja", 1),
        new Menu(7, "Lojas Cadastradas", 1),
        new Menu(8, "Cadastrar Produto", 2),
        new Menu(9, "Produtos Cadastrados", 2),
        new Menu(10, "Excluir Produto", 2),
        new Menu(11, "Realizar Pedido", 3),
        new Menu(12, "Despachar Pedido", 3),
        new Menu(13, "Buscar Pedido", 3),
        new Menu(14, "Pedidos Realizados", 3),
        new Menu(15, "Resumo de Vendas", 3),
        new Menu(16, "Listar Entregas", 4),
        new Menu(17, "Confirmar entrega", 4),
      };

      BuilderMenu builder = new BuilderMenu(menus);

      do
      {
        var idMenu = builder.Build();
        switch (idMenu)
        {
          case 6:
            controleService.CadastrarLoja();
            break;
          case 7:
            controleService.ExibirLojas();
            break;
          case 8:
            controleService.CadastrarProduto();
            break;
          case 9:
            controleService.ExibirProdutos();
            break;
          case 11:
            controleService.RealizarPedido();
            break;
          case 12:
            controleService.DespacharPedido();
            break;
          case 13:
            controleService.VisualizarPedido();
            break;
          case 14:
            controleService.ExibirPedidos();
            break;
          case 16:
            controleService.ExibirEntregas();
            break;

          default:
            Console.WriteLine("\n Opção não implementada.");
            Console.Write("\nPressione Enter...");
            Console.ReadKey();
            break;
        }
      } while (true);
    }
  }
}
