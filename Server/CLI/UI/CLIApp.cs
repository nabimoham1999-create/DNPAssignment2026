using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI;

public class CLIApp
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;

    public CLIApp(
        IPostRepository postRepository,
        IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task Run()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("==============================");
            Console.WriteLine("        SOCIAL MEDIA CLI");
            Console.WriteLine("==============================");
            Console.WriteLine();
            Console.WriteLine("1. Manage Posts");
            Console.WriteLine("2. Manage Users");
            Console.WriteLine("0. Exit");
            Console.WriteLine();

            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    ManagePostsView postsView = new ManagePostsView(
                        postRepository,
                        userRepository,
                        commentRepository
                    );

                    await postsView.Show();
                    break;

                case "2":
                    ManageUsersView usersView = new ManageUsersView(
                        userRepository
                    );

                    await usersView.Show();
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

        Console.WriteLine("Goodbye!");
    }
}