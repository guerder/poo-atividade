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
        Console.WriteLine("");
        Console.WriteLine("Não existem lojas cadastradas!");
        Console.ReadKey();
        return;
      }

      Console.WriteLine("");
      Console.WriteLine("".PadRight(100, '-'));
      for (int i = 0; i < _controle.GetLojas().Count; i++)
      {
        Console.WriteLine($"{i + 1}. {_controle.GetLojas()[i].Nome}");
      }
      Console.WriteLine("\nPressione Enter...");
      Console.ReadKey();
    }

    public Loja SelecionarLoja()
    {
      if (_controle.GetLojas().Count == 0)
      {
        Console.WriteLine("");
        Console.WriteLine("Não existe loja cadastrada!");
        Console.ReadKey();
        return null;
      }

      Console.WriteLine("");
      Console.WriteLine("".PadRight(100, '-'));
      for (int i = 0; i < _controle.GetLojas().Count; i++)
      {
        Console.WriteLine($"{i + 1}. {_controle.GetLojas()[i].Nome}");
      }

      Console.Write("\n > Digite o número da Loja para atualizar o estoque: ");
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
        Console.WriteLine("");
        Console.WriteLine("Não existem produtos cadastrados!");
        Console.ReadKey();
        return;
      }
      Console.WriteLine("");
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "CÓDIGO".PadRight(20, ' ') +
          "PRODUTO".PadRight(20, ' ') +
          "VALOR".PadRight(20, ' ') +
          "QUANTIDADE".PadRight(20, ' ') +
          "LOJA".PadRight(20, ' ')
        );
      foreach (var loja in _controle.GetLojas())
      {
        foreach (var estoque in loja.Estoques)
        {
          Console.WriteLine(
              $"{estoque.Produto.Codigo}".PadRight(20, '.') +
              $"{estoque.Produto.Nome}".PadRight(20, '.') +
              $"R$ {estoque.Produto.Preco}".PadRight(20, '.') +
              $"{estoque.Quantidade}".PadRight(20, '.') +
              $"{loja.Nome}".PadRight(20, '.')
            );
        }
      }
      Console.WriteLine("\nPressione Enter...");
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
      Console.WriteLine("");
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "CÓDIGO".PadRight(20, ' ') +
          "PRODUTO".PadRight(20, ' ') +
          "VALOR".PadRight(20, ' ')
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
        Console.ReadKey();
        return;
      }

      string nomeCliente = "";
      Console.Write("Digite o nome do 'cliente': ");
      try
      {
        nomeCliente = Console.ReadLine();
      }
      catch { }

      string nomeVendedor = "";
      Console.Write("Digite o nome do 'vendedor': ");
      try
      {
        nomeVendedor = Console.ReadLine();
      }
      catch { }

      Pedido pedido = new Pedido(nomeCliente, nomeVendedor);

      Produto produto;
      int quantidade;
      double desconto;
      string continuar = "";
      ItemPedido item;
      do
      {
        produto = SelecionarProduto();
        if (produto == null)
        {
          Console.WriteLine("Produto inválido!");
        }
        else
        {
          Console.Write("Digite a quantidade desejada: ");
          try
          {
            quantidade = int.Parse(Console.ReadLine());
          }
          catch { quantidade = 1; }

          Console.Write("Aplicar desconto para este produto? (S/N): ");
          if (Console.ReadLine().ToUpper() == "S")
          {
            Console.Write("Digite a porcentagem de desconto (ex. 15): ");
            try
            {
              desconto = double.Parse(Console.ReadLine());
            }
            catch { desconto = 0; }

            item = new ItemPedido(produto, quantidade, desconto);
          }
          else
          {
            item = new ItemPedido(produto, quantidade);
          }

          pedido.AdicionarItem(item);
        }

        Console.Write("Deseja adicionar outro produto ao pedido? (S/N): ");
        continuar = Console.ReadLine().ToUpper();
      } while (continuar == "S");

      ExibirDetalhesPedido(pedido);

      Console.WriteLine("Confirma o pedido? S/N");
      string confirmarPedido = "";
      try
      {
        confirmarPedido = Console.ReadLine().ToUpper();
      }
      catch { }

      if (confirmarPedido == "S")
      {
        pedido.ConfirmarPedido();
        loja.Pedidos.Add(pedido);
        Save();
      }
    }

    private void ExibirDetalhesPedido(Pedido pedido)
    {
      Console.WriteLine("");
      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "Data:".PadRight(33, '.') + pedido.Data.ToString("dd/MM/yyyy HH:mm") + " " +
          "Cliente:".PadRight(50 - pedido.NomeCliente.Length, '.') + pedido.NomeCliente
        );
      Console.WriteLine(
          "Status:".PadRight(49 - pedido.Status.ToString().Length, '.') + pedido.Status + " " +
          "Vendedor:".PadRight(50 - pedido.NomeVendedor.Length, '.') + pedido.NomeVendedor
        );

      Console.WriteLine("".PadRight(100, '_'));
      Console.WriteLine(
          "CÓDIGO".PadRight(9, '.') + " " +
          "PRODUTO".PadRight(29, '.') + " " +
          "VALOR UNIT".PadRight(19, '.') + " " +
          "QUANT".PadRight(9, '.') + " " +
          "DESC (%)".PadRight(9, '.') + " " +
          "VALOR TOTAL".PadRight(20, '.')
        );

      Console.WriteLine("".PadRight(100, '-'));
      foreach (var item in pedido.Itens)
      {
        Console.WriteLine(
          $"{item.Produto.Codigo}".PadLeft(9, '0') + " " +
          $"{item.Produto.Nome}".PadRight(29, '.') + " " +
          $"R$ {item.Produto.Preco}".PadRight(19, '.') + " " +
          $"{item.Quantidade}".PadRight(9, '.') + " " +
          $"{item.Desconto}%".PadRight(9, '.') + " " +
          $"R$ {item.ObterTotalItem()}".PadRight(20, '.')
        );
        Console.WriteLine("".PadRight(100, '-'));
      }
      Console.WriteLine(($"R$ {pedido.ObterTotal()}".PadRight(20, ' ')).PadLeft(100, ' '));

      Console.WriteLine("\nPressione Enter...");
      Console.ReadKey();
    }

    // TODO - Falta implementar
    public void ExibirPedidos()
    {
      Console.WriteLine("".PadRight(100, '-'));
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