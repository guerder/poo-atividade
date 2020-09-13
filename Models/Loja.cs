using System.Collections.Generic;

namespace sgp.Models
{
  public class Loja
  {
    public string Nome { get; set; }
    public List<Estoque> Estoques { get; set; }
    public List<Pedido> Pedidos { get; set; }
  }
}