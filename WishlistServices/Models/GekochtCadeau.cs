﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishlistServices.Models
{
    public class GekochtCadeau
    {
        public int Id { get; set; }

        public Gebruiker Koper { get; set; }

        public GekochtCadeau()
        {
        }
        public GekochtCadeau(Gebruiker koper)
        {
            Koper = koper;
        }
    }
}
