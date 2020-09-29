using System.Collections.Generic;
using sgp.Models;
using sgp.Repository;

namespace sgp.Services
{
  public class LojaService
  {
    public LojaRepository LojaRepository { get; set; }

    public List<Loja> ListarLojas()
    {
      return LojaRepository.GetInstance.GetControle().GetLojas();
    }
  }
}