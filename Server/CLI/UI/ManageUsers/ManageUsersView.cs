using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepository;

    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task Show()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("==============================");
            Console.WriteLine("        MANAGE USERS");
            Console.WriteLine("==============================");
            Console.WriteLine();
            Console.WriteLine("1. List users");
            Console.WriteLine("2. Create user");
            Console.WriteLine("3. Delete user");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    new ListUsersView(userRepository).Show();
                    break;

                case "2":
                    await new CreateUserView(userRepository).Show();
                    break;

                case "3":
                    await DeleteUser();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task DeleteUser()
    {
        Console.Clear();

        Console.WriteLine("========== DELETE USER ==========");
        Console.WriteLine();

        Console.Write("Enter User ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            Console.ReadKey();
            return;
        }

        bool exists = userRepository
            .GetMany()
            .Any(u => u.Id == id);

        if (!exists)
        {
            Console.WriteLine("User not found.");
            Console.ReadKey();
            return;
        }

        await userRepository.DeleteAsync(id);

        Console.WriteLine("User deleted successfully.");
        Console.ReadKey();
    }
}