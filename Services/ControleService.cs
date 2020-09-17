using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using sgp.Models;
using System.Linq;

namespace sgp.Services
{
  public class ControleService
  {
    private static ControleEstoqueVenda _controle { get; set; }

    public ControleService()
    {
      _controle = new ControleEstoqueVenda();

      // Abre o arquivo para ler os dados
      try
      {
        FileStream fs = new FileStream("data.bin", FileMode.Open);
        // Cria um objeto BinaryFormatter para realizar a dessarialização
        BinaryFormatter bf = new BinaryFormatter();
        // Usa o objeto BinaryFormatter para desserializar os dados do arquivo
        _controle = (ControleEstoqueVenda)bf.Deserialize(fs);
        // fecha o arquivo
        fs.Close();
      }
      catch { }
    }

    public void CadastrarProduto()
    {
      if (!_controle.ExisteLoja())
      {
        Console.WriteLine("");
        Console.WriteLine("Não existe loja no sistema para cadastrar produto!");
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
      loja.Estoques.Add(new Estoque(produto, qtd));
      Save();
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
      Save();
    }

    public void ExibirProdutos()
    {
      if (_controle.ListarProdutos().Count == 0)
      {
        Console.WriteLine("Não existem produtos cadastrados!");
        Console.Write("\nPressione Enter...");
        Console.ReadKey();
        return;
      }
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "CÓDIGO".PadRight(19, ' ') + " " +
          "PRODUTO".PadRight(29, ' ') + " " +
          "VALOR".PadRight(19, ' ') + " " +
          "QUANT.".PadRight(9, ' ') + " " +
          "LOJA".PadRight(20, ' ')
        );
      foreach (var loja in _controle.GetLojas())
      {
        foreach (var estoque in loja.Estoques)
        {
          Console.WriteLine(
              $"{estoque.Produto.Codigo}".PadRight(19, '.') + " " +
              $"{estoque.Produto.Nome}".PadRight(29, '.') + " " +
              $"{estoque.Produto.Preco.ToString("C")}".PadRight(19, '.') + " " +
              $"{estoque.Quantidade}".PadRight(9, '.') + " " +
              $"{loja.Nome}".PadRight(20, '.')
            );
        }
      }
      Console.Write("\nPressione Enter...");
      Console.ReadKey();
    }

    public Produto SelecionarProduto()
    {
      if (_controle.ListarProdutos().Count == 0)
      {
        Console.WriteLine("");
        Console.WriteLine("Não existem produtos cadastrados!");
        Console.ReadKey();
        return null;
      }
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "CÓDIGO".PadRight(33, ' ') +
          "PRODUTO".PadRight(33, ' ') +
          "VALOR".PadRight(33, ' ')
        );
      foreach (var prod in _controle.ListarProdutos())
      {
        Console.WriteLine(
            $"{prod.Codigo}".PadRight(33, '.') +
            $"{prod.Nome}".PadRight(33, '.') +
            $"R$ {prod.Preco}".PadRight(33, '.')
          );
      }

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
      Produto produto = _controle.ListarProdutos().FirstOrDefault(x => x.Codigo == codigo);
      return produto;
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

        pedido.AdicionarItem(itemPedido);

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
        Save();
      }
    }

    private void ExibirDetalhesPedido(Pedido pedido)
    {
      Console.Clear();
      Console.WriteLine("");
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "Data:".PadRight(33, ' ') + pedido.Data.ToString("dd/MM/yyyy HH:mm") + "|" +
          "Cliente:".PadRight(50 - pedido.NomeCliente.Length, ' ') + pedido.NomeCliente
        );
      Console.WriteLine(
          "Status:".PadRight(49 - pedido.Status.ToString().Length, ' ') + pedido.Status + "|" +
          "Vendedor:".PadRight(50 - pedido.NomeVendedor.Length, ' ') + pedido.NomeVendedor
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
          $"R$ {item.Produto.Preco}".PadRight(19, '.') + " " +
          $"{item.Quantidade}".PadRight(9, '.') + " " +
          $"{item.Desconto}%".PadRight(9, '.') + " " +
          $"{item.ObterTotalItem().ToString("C")}".PadRight(20, '.')
        );
      }
      Console.WriteLine("");
      Console.WriteLine("TOTAL".PadRight(79, '.') + " " + $"{pedido.ObterTotal().ToString("C")}".PadRight(20, '.'));

      Console.Write("\nPressione Enter...");
      Console.ReadKey();
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

      foreach (var pedido in _controle.ListarPedidos())
      {
        Console.WriteLine(
          $"{pedido.Codigo}".PadLeft(4, '0') + " " +
          $"{pedido.Data.ToString("dd/MM/yyyy HH:mm")}".PadRight(16, '.') + " " +
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
    static void Save()
    {
      try
      {
        // Cria um arquivo para salvar os dados
        FileStream fs = new FileStream("data.bin", FileMode.Create);
        // Cria um objeto BinaryFormatter para realizar a serialização
        BinaryFormatter bf = new BinaryFormatter();
        // Usa o objeto BinaryFormatter para serializar os dados para o arquivo
        bf.Serialize(fs, _controle);
        // fecha o arquivo
        fs.Close();
      }
      catch (Exception e)
      {
        Console.WriteLine("erro na gravação dos dados: " + e);
        Console.ReadKey();
      }
    }
  }
}