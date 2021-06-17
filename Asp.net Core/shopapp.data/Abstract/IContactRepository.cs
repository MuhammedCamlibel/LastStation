using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IContactRepository:IRepository<Contact>
    {
         void CreateMessage(string UserId,string message,string mail);
         
    }
}