using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IContactService
    {
          void CreateMessage(string UserId,string message,string mail);
          
          List<Contact> GetAll(); 
    }
}