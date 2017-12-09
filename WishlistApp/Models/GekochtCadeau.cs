using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistApp.Models
{
    public class GekochtCadeau
    {
        public int Id { get; set; }

        public Gebruiker Koper { get; set; }

        //ENKEL EN ALLEEN ENTITY FRAMEWORK ANDERS DIKKE BUGZZ
        public Wens Wens { get; set; }


    }
}
