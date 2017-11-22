using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class Wens
    {
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public Object Foto { get; set; }
        public bool Gekocht { get; set; }
        public Categorie Categorie { get; set; }
    }
}
