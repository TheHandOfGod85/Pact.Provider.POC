using Pact.Provider.POC.Models;

namespace Pact.Provider.POC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private List<User> Users = [
            new User(1, "John Doe"),
            new User(2, "Dan Smith"),
            ];
        public User? Get(int id)
        {
            return Users.Find(u => u.Id == id);
        }

        public void SetState(List<User> users)
        {
            Users = users;
        }
    }
}
