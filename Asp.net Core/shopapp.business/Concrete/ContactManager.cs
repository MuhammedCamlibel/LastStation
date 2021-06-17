using System.Collections.Generic;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class ContactManager : IContactService
    {
        private  IContactRepository _contactRepository;
        public ContactManager(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        } 

        public void CreateMessage(string UserId, string message,string mail)
        {
            _contactRepository.CreateMessage(UserId,message,mail);
        }

        public List<Contact> GetAll()
        {
            return _contactRepository.GetAll();
           
        }

        
    }
}