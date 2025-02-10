using Provider.POC.Models;

namespace Provider.POC.Repositories
{
    public interface IUserRepository
    {
        public User? Get(int id);
        public void SetState(List<User> users);
    }
}
