public interface IRepositoryUser
{
    public void Add(User user);
    public User GetCredentials(string name);
    public List<User> GetAll();
    public void EditUserRole(string Username, string Role);
    public void DeleteUser(string Username);
}