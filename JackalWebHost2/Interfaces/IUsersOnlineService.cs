namespace JackalWebHost2.Interfaces
{
    public interface IUsersOnlineService
    {
        List<long> AddUser(long userId);

        List<long> RemoveUser(long userId);
    }
}
