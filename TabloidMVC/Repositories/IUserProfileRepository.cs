using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        UserProfile GetById(int id);

        void AddUser(UserProfile user);

        List<UserProfile> GetAllUsers();
    }
}