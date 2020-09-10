using System;
using System.Collections.Generic;
using System.Linq;
using sgp.Models;

namespace sgp.Models
{
  public class Pedido
  {
    public DateTime Data { get; set; }
    public string NomeCliente { get; set; }
    public string NomeVendedor { get; set; }
    public List<ItemPedido> Itens { get; set; }

    public Pedido(string nomeCliente, string nomeVendedor)
    {
      this.Data = DateTime.Now;
      this.NomeCliente = nomeCliente;
      this.NomeVendedor = nomeVendedor;
      this.Itens = new List<ItemPedido>();
    }

    public double ObterTotal()
    {
      var total = Itens.Aggregate((double)0, (acc, current) => acc + current.ObterTotalItem());
      return total;
    }
  }
}