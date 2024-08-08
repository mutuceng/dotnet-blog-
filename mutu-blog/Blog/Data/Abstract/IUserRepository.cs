using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entity;

namespace Blog.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void CreateUser(User User);
        void EditUser(User User);
        void DeleteUser(User User);
        void EditUserRole(User User);

        
    }
}