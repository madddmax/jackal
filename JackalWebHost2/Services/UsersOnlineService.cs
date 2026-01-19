using JackalWebHost2.Interfaces;
using System.Collections.Concurrent;

namespace JackalWebHost2.Services
{
    public class UsersOnlineService : IUsersOnlineService
    {
        private readonly ConcurrentBag<long> _onlineUsers;

        public UsersOnlineService()
        {
            _onlineUsers = new ConcurrentBag<long>();
        }


        public List<long> AddUser(long userId)
        {
            _onlineUsers.Add(userId);
            return _onlineUsers.ToList();
        }
        public List<long> RemoveUser(long userId)
        {
            _onlineUsers.TryTake(out long item);
            return _onlineUsers.ToList();
        }
    }
}
