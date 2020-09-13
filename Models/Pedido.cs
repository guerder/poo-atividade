using System;
using System.Collections.Generic;
using System.Linq;
using sgp.Models.Enums;

namespace sgp.Models
{
  public class Pedido
  {
    public DateTime Data { get; set; }
    public string NomeCliente { get; set; }
    public string NomeVendedor { get; set; }
    private List<ItemPedido> Itens { get; set; }
    public Loja Loja { get; set; }
    public Status Status { get; set; }

    public Pedido(string nomeCliente, string nomeVendedor)
    {
      this.Data = DateTime.Now;
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

    public bool AdicionarItem(ItemPedido item)
    {
      if (this.Status == Status.Pendente)
      {
        this.Itens.Add(item);
        return true;
      }
      return false;
    }

    public void ConfirmarPedido()
    {
      this.Status = Status.Recebido;
    }
  }
}