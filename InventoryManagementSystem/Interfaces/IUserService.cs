public interface IUserService
{
    public void registerUser();
    public User LogIn();
    public void BasicUserOptions(User user);
    public void AdminOptions(User user);
    public void DisplayUsers();
    public Tuple<string, string> GeneratePassword();
}