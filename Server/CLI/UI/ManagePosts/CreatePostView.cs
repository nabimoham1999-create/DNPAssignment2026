using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task Show()
    {
        Console.Clear();

        Console.WriteLine("========== CREATE POST ==========");
        Console.WriteLine();

        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Body: ");
        string body = Console.ReadLine() ?? "";

        Console.WriteLine();
        Console.WriteLine("Available users:");

        foreach (User user in userRepository.GetMany())
        {
            Console.WriteLine($"{user.Id}. {user.UserName}");
        }

        Console.WriteLine();
        Console.Write("User ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            Console.ReadKey();
            return;
        }

        bool userExists = userRepository
            .GetMany()
            .Any(u => u.Id == userId);

        if (!userExists)
        {
            Console.WriteLine("User does not exist.");
            Console.ReadKey();
            return;
        }

        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        await postRepository.AddAsync(post);

        Console.WriteLine();
        Console.WriteLine("Post created successfully!");
        Console.WriteLine($"Post ID: {post.Id}");

        Console.ReadKey();
    }
}