using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using sgp.Models;
using System.Linq;
using sgp.Models.Enums;
using sgp.Data;

namespace sgp.Services
{
  public class ControleService
  {
    private static ControleEstoqueVenda _controle { get; set; }
    Persistence persistence = Persistence.GetInstance;

    public ControleService()
    {
      _controle = persistence.GetControle();
    }

    public void CadastrarProduto()
    {
      if (!_controle.ExisteLoja())
      {
        Console.WriteLine("");
        Console.WriteLine("Não existe loja no sistema para cadastrar produto!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      var loja = SelecionarLoja();
      if (loja == null)
      {
        Console.WriteLine("");
        Console.WriteLine("Loja não selecionada");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      string description = "";
      double value = 0.0;
      int qtd = 0;

      Console.Write("\n > Digite a descrição do produto: ");
      try
      {
        description = Console.ReadLine();
      }
      catch { }

      Console.Write("\n > Digite o valor: R$ ");
      try
      {
        value = double.Parse(Console.ReadLine());
      }
      catch { }

      Console.Write("\n > Digite a quantidade em estoque: ");
      try
      {
        qtd = int.Parse(Console.ReadLine());
      }
      catch { }

      int nextId = _controle.nextIdProduto();
      Produto produto = new Produto(nextId, description, value);
      loja.Estoques.Add(new Estoque(produto, qtd, loja));
      persistence.Save();
    }

    public void ExibirProdutos()
    {
      ListarProdutos();

      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    private void ListarProdutos()
    {
      if (_controle.ListarEstoques().Count == 0)
      {
        Console.WriteLine("Não existem produtos cadastrados!");
        return;
      }

      var ListaEstoque = _controle.ListarEstoques().OrderBy(x => x.Produto.Codigo);
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "CÓDIGO".PadRight(9, ' ') + " " +
          "PRODUTO".PadRight(29, ' ') + " " +
          "VALOR".PadRight(19, ' ') + " " +
          "QUANT.".PadRight(9, ' ') + " " +
          "LOJA".PadRight(30, ' ')
        );
      foreach (var estoque in ListaEstoque)
      {
        Console.WriteLine(
            $"{estoque.Produto.Codigo}".PadLeft(9, '0') + " " +
            $"{estoque.Produto.Nome}".PadRight(29, '.') + " " +
            $"{estoque.Produto.Preco.ToString("C")}".PadRight(19, '.') + " " +
            $"{estoque.Quantidade}".PadRight(9, '.') + " " +
            $"{estoque.Loja.Nome}".PadRight(30, '.')
          );
      }
    }

    public Produto SelecionarProduto()
    {
      ListarProdutos();

      int codigo;
      Console.Write("\n > Digite o código do produto: ");
      try
      {
        codigo = int.Parse(Console.ReadLine());
      }
      catch
      {
        codigo = -1;
      }
      Produto produto = _controle.ListarEstoques().FirstOrDefault(x => x.Produto.Codigo == codigo).Produto;
      return produto;
    }

    public void ExibirLojas()
    {
      if (_controle.GetLojas().Count == 0)
      {
        Console.WriteLine("Não existem lojas cadastradas!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      for (int i = 0; i < _controle.GetLojas().Count; i++)
      {
        Console.WriteLine($"{i + 1}. {_controle.GetLojas()[i].Nome}");
      }
      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    public Loja SelecionarLoja()
    {
      if (_controle.GetLojas().Count == 0)
      {
        Console.WriteLine("Não existe loja cadastrada!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return null;
      }

      for (int i = 0; i < _controle.GetLojas().Count; i++)
      {
        Console.WriteLine($"{i + 1}. {_controle.GetLojas()[i].Nome}");
      }

      Console.Write("\n > Digite o número da Loja: ");
      int option;
      try
      {
        option = int.Parse(Console.ReadLine());
      }
      catch
      {
        option = 0;
      }

      int index = option - 1;
      if (index >= 0 && index < _controle.GetLojas().Count)
      {
        return _controle.GetLojas()[index];
      }
      return null;
    }

    public void CadastrarLoja()
    {
      string nome = "";
      Console.Write("\n > Digite o nome da Loja: ");
      try
      {
        nome = Console.ReadLine();
      }
      catch { }

      Loja loja = new Loja(nome);
      _controle.AdicionarLoja(loja);
      persistence.Save();
    }

    public void RealizarPedido()
    {
      var loja = SelecionarLoja();
      if (loja == null)
      {
        Console.WriteLine("");
        Console.WriteLine("Loja não selecionada");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      string nomeCliente = "";
      Console.Write("\n > Digite o nome do 'Cliente': ");
      try
      {
        nomeCliente = Console.ReadLine();
      }
      catch { }

      string nomeVendedor = "";
      Console.Write("\n > Digite o nome do 'Vendedor': ");
      try
      {
        nomeVendedor = Console.ReadLine();
      }
      catch { }

      int nextId = _controle.nextIdPedido();
      Pedido pedido = new Pedido(nextId, nomeCliente, nomeVendedor);

      Produto produto;
      int quantidade;
      double desconto;
      string novoItem = "";
      ItemPedido itemPedido;
      do
      {

        produto = SelecionarProduto();
        if (produto == null)
        {
          Console.WriteLine("");
          Console.WriteLine("Produto inválido!");
          Console.Write("\nPressione Enter...");
          Console.ReadKey();
          return;
        }
        int quantidadeEmEstoque = _controle.BuscarEstoque(produto.Codigo).Quantidade;

        if (quantidadeEmEstoque == 0)
        {
          Console.WriteLine("");
          Console.WriteLine("Sem estoque do produto informado!");
          Console.Write("\nPressione Enter...");
          Console.ReadKey();
          return;
        }

        string msg = quantidadeEmEstoque == 1 ? $"Existe apenas {quantidadeEmEstoque} unidade " : $"Existem {quantidadeEmEstoque} unidades ";
        Console.WriteLine($"\n > {msg}do produto informado.");

        Console.Write("\n > Digite a quantidade desejada: ");
        try
        {
          quantidade = int.Parse(Console.ReadLine());
        }
        catch { quantidade = 1; }

        if (quantidade > quantidadeEmEstoque)
        {
          Console.WriteLine("");
          Console.WriteLine($"Quantidade informada '{quantidade}' excede o estoque de '{quantidadeEmEstoque}' unidade (s).");
          Console.Write("\nPressione Enter...");
          Console.ReadKey();
          return;
        }

        Console.Write("\n > Aplicar desconto para este produto? (S/N): ");
        if (Console.ReadLine().ToUpper() == "S")
        {
          Console.Write("\n > Digite a porcentagem de desconto (ex. 15): ");
          try
          {
            desconto = double.Parse(Console.ReadLine());
          }
          catch { desconto = 0; }

          itemPedido = new ItemPedido(produto, quantidade, desconto);
        }
        else
        {
          itemPedido = new ItemPedido(produto, quantidade);
        }

        pedido.Itens.Add(itemPedido);

        Console.Write("\n > Deseja adicionar outro produto ao pedido? (S/N): ");
        novoItem = Console.ReadLine().ToUpper();
      } while (novoItem == "S");

      ExibirDetalhesPedido(pedido);

      Console.Write("\n > Confirma o pedido? (S/N): ");
      string confirmarPedido = "";
      try
      {
        confirmarPedido = Console.ReadLine().ToUpper();
      }
      catch { }

      if (confirmarPedido == "S")
      {
        foreach (var item in pedido.Itens)
        {
          var estoque = _controle.BuscarEstoque(item.Produto.Codigo);
          estoque.Quantidade -= item.Quantidade;
        }
        pedido.ConfirmarPedido();
        pedido.Loja = loja;
        loja.Pedidos.Add(pedido);
        persistence.Save();
      }
    }

    public void VisualizarPedido()
    {
      var pedido = BuscarPedido();

      if (pedido == null)
      {
        Console.WriteLine("");
        Console.WriteLine("Pedido não localizado");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      ExibirDetalhesPedido(pedido);

      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    private Pedido BuscarPedido()
    {

      int codigoPedido;
      Console.Write("\n > Digite o Código do pedido: ");
      try
      {
        codigoPedido = int.Parse(Console.ReadLine());
      }
      catch
      {
        codigoPedido = 0;
      }

      Pedido pedido = _controle.ListarPedidos().FirstOrDefault(x => x.Codigo == codigoPedido);
      return pedido;
    }

    private void ExibirDetalhesPedido(Pedido pedido)
    {
      Console.Clear();
      Console.WriteLine("");
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "Status:".PadRight(49 - pedido.Status.ToString().Length, ' ') + pedido.Status + "|" +
          "Data do pedido:".PadRight(34, ' ') + FormatDate(pedido.DataPedido)
        );
      Console.WriteLine(
          "Cliente:".PadRight(49 - pedido.NomeCliente.Length, ' ') + pedido.NomeCliente + "|" +
          "Data do despacho:".PadRight(34, ' ') + FormatDate(pedido.DataDespacho)
        );
      Console.WriteLine(
          "Vendedor:".PadRight(49 - pedido.NomeVendedor.Length, ' ') + pedido.NomeVendedor + "|" +
          "Data da entrega:".PadRight(34, ' ') + FormatDate(pedido.DataEntrega)
        );

      Console.WriteLine("".PadRight(100, '-'));
      Console.WriteLine(
          "CÓDIGO".PadRight(9, ' ') + " " +
          "PRODUTO".PadRight(29, ' ') + " " +
          "VALOR UNIT".PadRight(19, ' ') + " " +
          "QUANT".PadRight(9, ' ') + " " +
          "DESC (%)".PadRight(9, ' ') + " " +
          "SUB-TOTAL".PadRight(20, ' ')
        );

      foreach (var item in pedido.Itens)
      {
        Console.WriteLine(
          $"{item.Produto.Codigo}".PadLeft(9, '0') + " " +
          $"{item.Produto.Nome}".PadRight(29, '.') + " " +
          $"{item.Produto.Preco.ToString("C")}".PadRight(19, '.') + " " +
          $"{item.Quantidade}".PadRight(9, '.') + " " +
          $"{item.Desconto}%".PadRight(9, '.') + " " +
          $"{item.ObterTotalItem().ToString("C")}".PadRight(20, '.')
        );
      }
      Console.WriteLine("");
      Console.WriteLine("TOTAL".PadRight(79, '.') + " " + $"{pedido.ObterTotal().ToString("C")}".PadRight(20, '.'));

    }

    public void ExibirPedidos()
    {
      if (!_controle.ExistemPedidos())
      {
        Console.WriteLine("Não existem pedidos cadastrados!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      var ListaPedidos = _controle.ListarPedidos().OrderBy(x => x.Codigo);
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "COD.".PadRight(4, ' ') + " " +
          "DATA".PadRight(16, ' ') + " " +
          "CLIENTE".PadRight(24, ' ') + " " +
          "ITENS".PadRight(7, ' ') + " " +
          "LOJA".PadRight(19, ' ') + " " +
          "STATUS".PadRight(10, ' ') + " " +
          "TOTAL".PadRight(14, ' ')
        );

      foreach (var pedido in ListaPedidos)
      {
        Console.WriteLine(
          $"{pedido.Codigo}".PadLeft(4, '0') + " " +
          $"{pedido.DataPedido.ToString("dd/MM/yyyy HH:mm")}".PadRight(16, '.') + " " +
          $"{pedido.NomeCliente}".PadRight(24, '.') + " " +
          $"{pedido.Itens.Count}".PadRight(7, '.') + " " +
          $"{pedido.Loja.Nome}".PadRight(19, '.') + " " +
          $"{pedido.Status}".PadRight(10, '.') + " " +
          $"{pedido.ObterTotal().ToString("C")}".PadRight(14, '.')
        );
      }
      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    public void DespacharPedido()
    {
      var pedido = BuscarPedido();

      if (pedido == null)
      {
        Console.WriteLine("");
        Console.WriteLine("Pedido não localizado");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      ExibirDetalhesPedido(pedido);

      if (pedido.Status >= Status.Despachado)
      {
        Console.WriteLine("");
        Console.WriteLine($"Pedido já consta como {pedido.Status}.");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      Console.Write("\n > Confirma o encaminhamento para o setor de entrega? (S/N): ");
      string confirmarPedido = "";
      try
      {
        confirmarPedido = Console.ReadLine().ToUpper();
      }
      catch { }

      if (confirmarPedido == "S")
      {
        pedido.DespacharPedido();
        _controle.AdicionarNovaEntrega(new SetorEntrega(pedido));
        persistence.Save();

        Console.WriteLine("");
        Console.WriteLine("Despacho realizado com sucesso!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
      }
    }

    public void ExibirEntregas()
    {
      if (!_controle.ExistemEntregas())
      {
        Console.WriteLine("Não existem entregas cadastradas!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "COD.".PadRight(4, ' ') + " " +
          "DATA".PadRight(16, ' ') + " " +
          "CLIENTE".PadRight(24, ' ') + " " +
          "ITENS".PadRight(7, ' ') + " " +
          "LOJA".PadRight(19, ' ') + " " +
          "STATUS".PadRight(10, ' ') + " " +
          "TOTAL".PadRight(14, ' ')
        );

      foreach (var pedido in _controle.ListarEntregas())
      {
        Console.WriteLine(
          $"{pedido.Codigo}".PadLeft(4, '0') + " " +
          $"{pedido.DataPedido.ToString("dd/MM/yyyy HH:mm")}".PadRight(16, '.') + " " +
          $"{pedido.NomeCliente}".PadRight(24, '.') + " " +
          $"{pedido.Itens.Count}".PadRight(7, '.') + " " +
          $"{pedido.Loja.Nome}".PadRight(19, '.') + " " +
          $"{pedido.Status}".PadRight(10, '.') + " " +
          $"{pedido.ObterTotal().ToString("C")}".PadRight(14, '.')
        );
      }
      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    private Pedido BuscarEntrega()
    {

      int codigoPedido;
      Console.Write("\n > Digite o Código do pedido: ");
      try
      {
        codigoPedido = int.Parse(Console.ReadLine());
      }
      catch
      {
        codigoPedido = 0;
      }

      Pedido pedido = _controle.ListarEntregas().FirstOrDefault(x => x.Codigo == codigoPedido);
      return pedido;
    }

    public void ConfirmarEntrega()
    {
      var entrega = BuscarEntrega();

      if (entrega == null)
      {
        Console.WriteLine("");
        Console.WriteLine("Entrega não localizada");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      ExibirDetalhesPedido(entrega);

      if (entrega.Status == Status.Entregue)
      {
        Console.WriteLine("");
        Console.WriteLine($"Pedido já consta como {entrega.Status}.");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }

      Console.Write("\n > Confirma a entrega do pedido? (S/N): ");
      string confirmarEntrega = "";
      try
      {
        confirmarEntrega = Console.ReadLine().ToUpper();
      }
      catch { }

      if (confirmarEntrega == "S")
      {
        entrega.FinalizarPedido();
        persistence.Save();

        Console.WriteLine("");
        Console.WriteLine("Entrega realizada com sucesso!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
      }
    }

    public void ResumoGeral()
    {
      int numeroLojas = _controle.Lojas.Count;
      int numeroProdutosCadastrados = _controle.ListarEstoques().Count;
      int numeroItens = _controle.ListarEstoques().Aggregate((int)0, (acc, current) => acc + current.Quantidade);
      int numeroPedidos = _controle.ListarPedidos().Count;

      var recebido = _controle.ListarPedidos().Where(x => x.Status == Status.Recebido).Count();
      int despachado = _controle.ListarPedidos().Where(x => x.Status == Status.Despachado).Count();
      int entregue = _controle.ListarPedidos().Where(x => x.Status == Status.Entregue).Count();

      var recebidoPercentagem = Math.Round(((double)recebido / numeroPedidos) * 100);
      recebidoPercentagem = double.IsNaN(recebidoPercentagem) ? 0 : recebidoPercentagem;

      var despachadoPercentagem = Math.Round(((double)despachado / numeroPedidos) * 100);
      despachadoPercentagem = double.IsNaN(despachadoPercentagem) ? 0 : despachadoPercentagem;

      var entreguePercentagem = Math.Round(((double)entregue / numeroPedidos) * 100);
      entreguePercentagem = double.IsNaN(entreguePercentagem) ? 0 : entreguePercentagem;

      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.WriteLine("".PadLeft(30, ' ') +
        $"Nº de lojas:".PadRight(20, ' ') +
        $"{numeroLojas}".PadLeft(20, ' '));
      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.WriteLine("".PadLeft(30, ' ') +
        $"Nº de produtos:".PadRight(20, ' ') +
        $"{numeroProdutosCadastrados}".PadLeft(20, ' '));
      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.WriteLine("".PadLeft(30, ' ') +
        $"Nº de itens total:".PadRight(20, ' ') +
        $"{numeroItens}".PadLeft(20, ' '));
      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));


      // Número de pedidos + Descrição em porcentagem por status do pedido.
      Console.WriteLine("".PadLeft(30, ' ') +
        $"Nº de pedidos:".PadRight(20, ' ') +
        $"{numeroPedidos}".PadLeft(20, ' '));

      Console.WriteLine("");
      Console.WriteLine("".PadLeft(30, ' ') +
        "RECEBIDO:".PadRight(20, '.') +
        $"({recebidoPercentagem}%) {recebido}".PadLeft(20, '.')
      );

      Console.WriteLine("".PadLeft(30, ' ') +
       "DESPACHADO:".PadRight(20, '.') +
       $"({despachadoPercentagem}%) {despachado}".PadLeft(20, '.')
      );

      Console.WriteLine("".PadLeft(30, ' ') +
       "ENTREGUE:".PadRight(20, '.') +
       $"({entreguePercentagem}%) {entregue}".PadLeft(20, '.')
      );

      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    public void ResumoVendas()
    {
      double valorEmEstoque = _controle.ListarEstoques()
        .Aggregate((double)0, (acc, current) =>
          acc + (current.Produto.Preco * current.Quantidade)
        );
      double faturamento = _controle.ListarPedidos()
        .Aggregate((double)0, (acc, current) =>
          acc + current.ObterTotal()
        );

      int numeroPedidos = _controle.ListarPedidos().Count;

      double ticketMedio = faturamento / numeroPedidos;
      ticketMedio = double.IsNaN(ticketMedio) ? 0 : ticketMedio;

      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.WriteLine("".PadLeft(30, ' ') +
        $"Valor em estoque:".PadRight(20, ' ') +
        $"{valorEmEstoque.ToString("C")}".PadLeft(20, ' '));
      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.WriteLine("".PadLeft(30, ' ') +
        $"Faturamento:".PadRight(20, ' ') +
        $"{faturamento.ToString("C")}".PadLeft(20, ' '));
      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.WriteLine("".PadLeft(30, ' ') +
        $"Nº de pedidos:".PadRight(20, ' ') +
        $"{numeroPedidos}".PadLeft(20, ' '));
      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.WriteLine("".PadLeft(30, ' ') +
            $"Ticket Médio:".PadRight(20, ' ') +
            $"{ticketMedio.ToString("C")}".PadLeft(20, ' '));
      Console.WriteLine("".PadRight(30, ' ') + "".PadRight(40, '-') + "".PadRight(30, ' '));

      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    private string FormatDate(DateTime? date)
    {
      if (!date.HasValue)
      {
        return "";
      }
      return date.Value.ToString("dd/MM/yyyy HH:mm");
    }

    public void MockarDados()
    {
      _controle = GerarDados();
      persistence.Save();

      Console.WriteLine("Populado base de dados com sucesso");
      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    private ControleEstoqueVenda GerarDados()
    {
      ControleEstoqueVenda novoControle = new ControleEstoqueVenda();
      Random random = new Random();

      string[] arrayNomesLoja = { "Rosa do Deserto", "Franks Jeans", "Konik", "Taco", "Indus", "Missbela" };

      string[] arrayNomesProduto = {
        "Bermuda Infantil",
        "Camisa Social",
        "Shorts Jeans",
        "Regata Masculina",
        "Tênis de Couro",
        "Vestido Infantil",
        "Biquine Adulto",
        "Tanga Praia",
        "Meia Listrada",
        "Kit Cueca 10 uni",
        "Boné Básico",
        "Leg Lisa",
        "Jaqueta de Couro",
        "Bermuda Adulto"
      };

      string[] arrayNomesCliente = {
        "Keyla Miguel Brás",
        "Cleide Castanheira",
        "Elisa Pegado Vaz",
        "Suely Abreu Bilhalva",
        "Louis Lira Uchoa"
      };

      string[] arrayNomesVendedor = {
        "Anaisa Inácio Angelim",
        "Clarisse Caldas Brião",
        "Manuel Palha Nolasco",
      };

      // Instancia novas Lojas.
      foreach (var nome in arrayNomesLoja)
      {
        novoControle.Lojas.Add(new Loja(nome));
      }

      // Instancia novos Produtos.
      Produto produto;
      Loja loja;
      string nomeLoja;
      foreach (var currentNameProduct in arrayNomesProduto)
      {
        produto = new Produto(novoControle.nextIdProduto(), currentNameProduct, random.Next(10, 150));

        bool novoSorteioLoja = false;
        do
        {
          nomeLoja = arrayNomesLoja[random.Next(0, arrayNomesLoja.Count())];

          loja = novoControle.Lojas.Find(x => x.Nome.Equals(nomeLoja));
          if (loja.Estoques.Any(x => x.Produto.Nome.Equals(currentNameProduct)))
          {
            novoSorteioLoja = true;
          }
        } while (novoSorteioLoja);

        loja.Estoques.Add(new Estoque(produto, random.Next(5, 50), loja));
      }

      // Instancia novos pedidos, que contém vários itens, um cliente e um vendedor aleatório.
      Pedido pedido;
      string nomeCliente;
      string nomeVendedor;
      string nomeProduto;
      int numeroPedidos = random.Next(50, 80);
      int numeroItens;
      int numeroQuantidade;
      double percentagemDesconto;
      ItemPedido item;
      Estoque estoque;
      Status status;
      for (var i = 0; i < numeroPedidos; i++)
      {
        nomeCliente = arrayNomesCliente[random.Next(0, arrayNomesCliente.Count())];
        nomeVendedor = arrayNomesVendedor[random.Next(0, arrayNomesVendedor.Count())];
        nomeLoja = arrayNomesLoja[random.Next(0, arrayNomesLoja.Count())];
        numeroItens = random.Next(1, 5);
        loja = novoControle.Lojas.Find(x => x.Nome.Equals(nomeLoja));
        pedido = new Pedido(novoControle.nextIdPedido(), nomeCliente, nomeVendedor);

        for (var j = 0; j < numeroItens; j++)
        {
          nomeProduto = arrayNomesProduto[random.Next(0, arrayNomesProduto.Count())];
          numeroQuantidade = random.Next(1, 5);
          percentagemDesconto = random.Next(0, 8);
          produto = novoControle.ListarEstoques().Find(x => x.Produto.Nome.Equals(nomeProduto)).Produto;
          item = new ItemPedido(produto, numeroQuantidade, percentagemDesconto);

          pedido.Itens.Add(item);
          estoque = novoControle.BuscarEstoque(item.Produto.Codigo);

          if (estoque.Quantidade - item.Quantidade < 0)
          {
            estoque.Quantidade = 0;
          }
          else
          {
            estoque.Quantidade -= item.Quantidade;
          }
        }

        pedido.ConfirmarPedido();
        pedido.Loja = loja;
        loja.Pedidos.Add(pedido);

        status = (Status)random.Next(2, 5);
        if (status != Status.Recebido)
        {
          pedido.DespacharPedido();
          novoControle.AdicionarNovaEntrega(new SetorEntrega(pedido));
        }
        if (status == Status.Entregue)
        {
          pedido.FinalizarPedido();
        }
      }

      return novoControle;
    }
  }
}