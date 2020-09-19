using System;
using sgp.Models.Enums;

namespace sgp.Models
{
  [Serializable]
  public class SetorEntrega
  {
    public Pedido Pedido { get; set; }

    public SetorEntrega(Pedido pedido)
    {
      this.Pedido = pedido;
    }

    public void Entregar()
    {
      this.Pedido.Status = Status.Entregue;
    }
  }
}