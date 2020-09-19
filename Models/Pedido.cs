using System;
using System.Collections.Generic;
using System.Linq;
using sgp.Models.Enums;

namespace sgp.Models
{
  [Serializable]
  public class Pedido
  {
    public int Codigo { get; }
    public DateTime DataPedido { get; set; }
    public DateTime? DataDespacho { get; set; } = null;
    public DateTime? DataEntrega { get; set; } = null;
    public string NomeCliente { get; set; }
    public string NomeVendedor { get; set; }
    public List<ItemPedido> Itens { get; }
    public Loja Loja { get; set; }
    public Status Status { get; set; }

    public Pedido(int codigo, string nomeCliente, string nomeVendedor)
    {
      this.Codigo = codigo;
      this.DataPedido = DateTime.Now;
      this.NomeCliente = nomeCliente;
      this.NomeVendedor = nomeVendedor;
      this.Itens = new List<ItemPedido>();
      this.Status = Status.Pendente;
    }

    public double ObterTotal()
    {
      var total = Itens.Aggregate((double)0, (acc, current) => acc + current.ObterTotalItem());
      return total;
    }

    public void ConfirmarPedido()
    {
      this.Status = Status.Recebido;
    }

    public void DespacharPedido()
    {
      this.Status = Status.Despachado;
      this.DataDespacho = DateTime.Now;
    }

    public void FinalizarPedido()
    {
      this.Status = Status.Entregue;
      this.DataEntrega = DateTime.Now;
    }
  }
}