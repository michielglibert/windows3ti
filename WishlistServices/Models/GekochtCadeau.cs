using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class GekochtCadeau
    {
        public int Id { get; set; }

        public Gebruiker Koper { get; set; }

        //ENKEL EN ALLEEN ENTITY FRAMEWORK ANDERS DIKKE BUGZZ
        public Wens Wens { get; set; }

        public GekochtCadeau()
        {
        }
        public GekochtCadeau(Gebruiker koper, Wens wens)
        {
            Koper = koper;
            Wens = wens;
        }
    }
}
