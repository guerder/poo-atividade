using System.Collections.Generic;

namespace sgp.Models
{
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

  }
}