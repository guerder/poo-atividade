using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using sgp.Models;

namespace sgp.Data
{
  public class Persistence
  {
    private static Persistence _instance;
    private ControleEstoqueVenda _controle;

    protected Persistence()
    {
      _controle = new ControleEstoqueVenda();
      try
      {
        FileStream fs = new FileStream("data.bin", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        _controle = (ControleEstoqueVenda)bf.Deserialize(fs);
        fs.Close();
      }
      catch { }
    }

    public static Persistence GetInstance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new Persistence();
        }
        return _instance;
      }
    }

    public ControleEstoqueVenda GetControle()
    {
      return _controle;
    }
    public void Save()
    {
      try
      {
        FileStream fs = new FileStream("data.bin", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, _controle);
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