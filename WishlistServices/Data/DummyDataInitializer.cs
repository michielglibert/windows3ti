using System.Collections.Generic;
using WishlistServices.Models;

namespace WishlistServices.Data
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
                List<Gebruiker> gebruikers = new List<Gebruiker>();

                Gebruiker gebruiker1 = new Gebruiker("Jef","Pass");;
                gebruikers.Add(gebruiker1);

                Gebruiker gebruiker2 = new Gebruiker("Karel","Vanheede");
                gebruikers.Add(gebruiker2);
                
                //DbInit
                foreach (var gebruiker in gebruikers)
                {
                    _dbContext.Gebruikers.Add(gebruiker);
                }

                //Save
                _dbContext.SaveChanges();
            }
        }
    }
}