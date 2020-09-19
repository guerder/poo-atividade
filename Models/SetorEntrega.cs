using System;
using sgp.Models.Enums;

namespace sgp.Models
{
  [Serializable]
  public class SetorEntrega
  {
    public DateTime DataDespacho { get; set; }
    public DateTime DataEntrega { get; set; }
    public Pedido Pedido { get; set; }

    public SetorEntrega(Pedido pedido)
    {
      this.Pedido = pedido;
      this.DataDespacho = DateTime.Now;
    }

    public void Entregar()
    {
      this.Pedido.Status = Status.Entregue;
      this.DataEntrega = DateTime.Now;
    }
  }
}