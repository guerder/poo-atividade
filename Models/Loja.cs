using System;
using System.Collections.Generic;

namespace sgp.Models
{
  [Serializable]
  public class Loja
  {
    public string Nome { get; set; }
    public List<Estoque> Estoques { get; set; }
    public List<Pedido> Pedidos { get; set; }

    public Loja(string nome)
    {
      this.Nome = nome;
      this.Estoques = new List<Estoque>();
      this.Pedidos = new List<Pedido>();
    }

    public bool hasOrder()
    {
      return this.Pedidos.Count != 0;
    }
  }
}