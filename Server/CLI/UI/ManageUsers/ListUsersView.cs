using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void Show()
    {
        Console.Clear();

        Console.WriteLine("========== ALL USERS ==========");
        Console.WriteLine();

        foreach (User user in userRepository.GetMany())
        {
            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"Username: {user.UserName}");
            Console.WriteLine("--------------------------------");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to go back...");
        Console.ReadKey();
    }
}