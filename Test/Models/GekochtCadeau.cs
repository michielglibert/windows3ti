using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class GekochtCadeau
    {
        public int Id { get; set; }
        public string Wat { get; set; }
        public double Prijs { get; set; }

        public Gebruiker Koper { get; set; }

        public GekochtCadeau()
        {
        }
    }
}
