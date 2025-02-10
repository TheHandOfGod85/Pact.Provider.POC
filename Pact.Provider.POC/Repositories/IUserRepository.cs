using Pact.Provider.POC.Models;

namespace Pact.Provider.POC.Repositories
{
    public interface IUserRepository
    {
        public User? Get(int id);
        public void SetState(List<User> users);
    }
}
