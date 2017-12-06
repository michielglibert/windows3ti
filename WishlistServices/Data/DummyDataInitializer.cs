using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishlistServices.Models;

namespace Server.Data
{
    public class DummyDataInitializer
    {
        private readonly WishlistDbContext _dbContext;

        public DummyDataInitializer(WishlistDbContext context)
        {
            _dbContext = context;
        }

        public void InitData()
        {
            _dbContext.Database.EnsureDeleted();

            if (_dbContext.Database.EnsureCreated())
            {
                //Gebruikers
                Gebruiker gebruiker1 = new Gebruiker("Jef","Pass");

                _dbContext.Gebruikers.Add(gebruiker1);

                _dbContext.SaveChanges();
            }
        }
    }
}