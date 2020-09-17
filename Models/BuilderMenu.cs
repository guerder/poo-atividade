using System;
using System.Collections.Generic;
using System.Linq;

namespace sgp.Models
{
  public class BuilderMenu
  {
    public List<Menu> Menus { get; set; }

    public BuilderMenu(List<Menu> menus)
    {
      this.Menus = menus;
    }
    private void Titulo(string titulo)
    {
      titulo = "".PadRight((100 - titulo.Length) / 2, ' ') + titulo.ToUpper();
      Console.WriteLine("".PadRight(100, ' '));
      Console.WriteLine(titulo);
      Console.WriteLine("".PadRight(100, ' '));
    }
    public int Build(int parent = 0)
    {
      int idReturn;

      int option = 0;

      Console.Clear();

      var title = parent == 0 ? "MENU PRINCIPAL" : Menus.Find(x => x.Id == parent).Name;
      Titulo(title);

      /*
        key = nº relativo do menu
        value = id do menu
      */
      var numbersOption = new Dictionary<int, int>();
      numbersOption.Add(0, 0);

      int number = 1;
      foreach (var item in Menus)
      {
        if (item.Parent == parent)
        {
          Console.WriteLine($"{number}. {item.Name}");
          numbersOption.Add(number, item.Id);
          number++;
        }
      }

      if (parent != 0)
      {
        Console.WriteLine("\n0. Voltar para o início");
      }
      Console.Write("\n > Digite o número correspondente: ");
      try
      {
        option = int.Parse(Console.ReadLine());
      }
      catch
      {
        option = -1;
      }

      var isOptionValid = numbersOption.ContainsKey(option);

      if (!isOptionValid)
      {
        Console.WriteLine("\n Opção inválida! Pressione Enter e tente novamente.");
        Console.ReadKey();

        idReturn = Build(parent);
      }
      else
      {
        int idMenu = numbersOption[option];

        if (Menus.Exists(x => x.Parent == idMenu))
        {
          idReturn = Build(idMenu);
        }
        else
        {
          idReturn = idMenu;
        }
      }

      Console.Clear();
      Titulo(Menus.Find(x => x.Id == idReturn).Name);
      return idReturn;
    }
  }
}