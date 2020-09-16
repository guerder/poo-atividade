using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using sgp.Models;

namespace sgp.Services
{
  public class ControleService
  {
    private static ControleEstoqueVenda _controle { get; set; }

    public ControleService()
    {
      _controle = new ControleEstoqueVenda();

      // Abre o arquivo para ler os dados
      FileStream fs = new FileStream("data.bin", FileMode.Open);
      try
      {
        // Cria um objeto BinaryFormatter para realizar a dessarialização
        BinaryFormatter bf = new BinaryFormatter();
        // Usa o objeto BinaryFormatter para desserializar os dados do arquivo
        _controle = (ControleEstoqueVenda)bf.Deserialize(fs);
      }
      catch
      {
      }
      finally
      {
        // fecha o arquivo
        fs.Close();
      }

      Console.ReadKey();
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

      string description;
      double value;
      int qtd;

      Console.Write("\n > Digite a descrição do produto: ");
      try
      {
        description = Console.ReadLine();
      }
      catch
      {
        description = "";
      }

      Console.Write("\n > Digite o valor: R$ ");
      try
      {
        value = double.Parse(Console.ReadLine());
      }
      catch
      {
        value = 0;
      }

      Console.Write("\n > Digite a quantidade em estoque: ");
      try
      {
        qtd = int.Parse(Console.ReadLine());
      }
      catch
      {
        qtd = 0;
      }

      Produto produto = new Produto(1, description, value);
      loja.Estoques.Add(new Estoque(produto, qtd));
      Save();
    }

    public void ExibirLojas()
    {
      if (_controle.GetLojas().Count == 0)
      {
        Console.WriteLine("");
        Console.WriteLine("Não existe loja cadastrada!");
        Console.ReadKey();
        return;
      }

      Console.WriteLine("\n".PadRight(100, '-'));
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

      Console.WriteLine("\n".PadRight(100, '-'));
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
      string nome;
      Console.Write("\n > Digite o nome da Loja: ");
      try
      {
        nome = Console.ReadLine();
      }
      catch
      {
        nome = "";
      }

      Loja loja = new Loja(nome);

      _controle.AdicionarLoja(loja);
      Save();
      Console.ReadKey();
    }

    public void ExibirProdutos()
    {

      Console.WriteLine("Produto \t Valor \t Qtd \t Loja");
      foreach (var loja in _controle.GetLojas())
      {
        foreach (var estoque in loja.Estoques)
        {
          Console.WriteLine($"{estoque.Produto.Nome} \t {estoque.Produto.Preco} \t {estoque.Quantidade} {loja.Nome}");
        }
      }
      Console.WriteLine("\nPressione Enter...");
      Console.ReadKey();
    }

    static void Save()
    {
      // Cria um arquivo para salvar os dados
      FileStream fs = new FileStream("data.bin", FileMode.Create);
      try
      {
        // Cria um objeto BinaryFormatter para realizar a serialização
        BinaryFormatter bf = new BinaryFormatter();
        // Usa o objeto BinaryFormatter para serializar os dados para o arquivo
        bf.Serialize(fs, _controle);
      }
      catch (Exception e)
      {
        Console.WriteLine("erro na gravação dos dados: " + e);
        Console.ReadKey();
      }
      finally
      {
        // fecha o arquivo
        fs.Close();
      }
    }
  }
}