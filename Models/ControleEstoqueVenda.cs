using System;
using System.Collections.Generic;
using System.Linq;

namespace sgp.Models
{
  [Serializable]
  public class ControleEstoqueVenda
  {
    private List<Loja> Lojas { get; set; }
    private List<SetorEntrega> SetorEntregas { get; set; }

    public ControleEstoqueVenda()
    {
      this.Lojas = new List<Loja>();
      this.SetorEntregas = new List<SetorEntrega>();
    }

    public void AdicionarNovaEntrega(SetorEntrega entrega)
    {
      this.SetorEntregas.Add(entrega);
    }

    public void AdicionarLoja(Loja loja)
    {
      this.Lojas.Add(loja);
    }

    public bool ExisteLoja()
    {
      return Lojas.Count != 0;
    }

    public List<Loja> GetLojas()
    {
      return Lojas;
    }

    public int nextIdProduto()
    {
      int lastId = 0;
      try
      {
        lastId = ListarProdutos().Select(x => x.Codigo).Max();

      }
      catch { }
      return ++lastId;
    }

    public List<Produto> ListarProdutos()
    {
      List<Produto> lista = new List<Produto>();

      foreach (var loja in Lojas)
      {
        foreach (var estoque in loja.Estoques)
        {
          lista.Add(estoque.Produto);
        }
      }
      return lista;
    }

  }
}