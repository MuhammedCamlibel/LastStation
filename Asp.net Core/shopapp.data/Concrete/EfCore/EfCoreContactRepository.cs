using System.Collections.Generic;
using System.Linq;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreContactRepository : EfCoreGenericRepository<Contact, ShopContext>, IContactRepository
    {
        public void CreateMessage(string UserId, string message,string mail)
        {
            using(var contex = new ShopContext())
            {
                contex.Contacts.Add(new Contact(){Message=message,UserId=UserId,Mail=mail});
                contex.SaveChanges();
            }
        }

        
    }
}