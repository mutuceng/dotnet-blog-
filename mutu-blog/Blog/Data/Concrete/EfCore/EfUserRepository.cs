using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Entity;

namespace Blog.Data.Concrete.EfCore
{
    public class EfUserRepository : IUserRepository
    {
        private BlogContext _context;

        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void DeleteUser(User user)
        {

        }
        public void EditUser(User user)
        {

        }
        public void EditUserRole(User user)
        {
            
        }
        
    }
}