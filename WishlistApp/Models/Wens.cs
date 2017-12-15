using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistApp.Models
{
    public class Wens
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public Object Foto { get; set; }
        public bool Gekocht { get; set; }

        public Categorie Categorie { get; set; }
        public GekochtCadeau GekochtCadeau { get; set; }

        public List<Categorie> CategorieLijst { get; set; }

        public Wens(string titel, string omschrijving, Categorie categorie)
        {
            Titel = titel;
            Omschrijving = omschrijving;
            Categorie = categorie;
            CategorieLijst = Enum.GetValues(typeof(Categorie)).Cast<Categorie>().ToList();
        }

    }
}
