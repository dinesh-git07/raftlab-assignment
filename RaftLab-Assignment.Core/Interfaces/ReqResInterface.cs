using RaftLab_Assignment.Models;

namespace RaftLab_Assignment.Interfaces
{
    public interface IReqResInterface
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync(int page);
    }
}
