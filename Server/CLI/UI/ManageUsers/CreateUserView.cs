using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task Show()
    {
        Console.Clear();

        Console.WriteLine("========== CREATE USER ==========");
        Console.WriteLine();

        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";

        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        User user = new User
        {
            UserName = username,
            Password = password
        };

        await userRepository.AddAsync(user);

        Console.WriteLine();
        Console.WriteLine("User created successfully!");
        Console.WriteLine($"User ID: {user.Id}");

        Console.ReadKey();
    }
}