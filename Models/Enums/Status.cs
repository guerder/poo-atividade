using System.ComponentModel.DataAnnotations;

namespace sgp.Models.Enums
{
  public enum Status
  {
    [Display(Name = "Pendente")]
    Pendente = 1,
    [Display(Name = "Recebido")]
    Recebido = 2,
    [Display(Name = "Despachado")]
    Despachado = 3,
    [Display(Name = "Entregue")]
    Entregue = 4,
  }
}