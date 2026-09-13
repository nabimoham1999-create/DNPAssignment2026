using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class AddCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public AddCommentView(
        ICommentRepository commentRepository,
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task Show()
    {
        Console.Clear();

        Console.WriteLine("========== ADD COMMENT ==========");
        Console.WriteLine();

        // Show posts
        Console.WriteLine("Available Posts:");

        foreach (Post post in postRepository.GetMany())
        {
            Console.WriteLine($"{post.Id}. {post.Title}");
        }

        Console.WriteLine();

        Console.Write("Post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid Post ID.");
            Console.ReadKey();
            return;
        }

        bool postExists = postRepository
            .GetMany()
            .Any(p => p.Id == postId);

        if (!postExists)
        {
            Console.WriteLine("Post not found.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();

        // Show users
        Console.WriteLine("Available Users:");

        foreach (User user in userRepository.GetMany())
        {
            Console.WriteLine($"{user.Id}. {user.UserName}");
        }

        Console.WriteLine();

        Console.Write("User ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid User ID.");
            Console.ReadKey();
            return;
        }

        bool userExists = userRepository
            .GetMany()
            .Any(u => u.Id == userId);

        if (!userExists)
        {
            Console.WriteLine("User not found.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine();

        Console.Write("Comment: ");
        string body = Console.ReadLine() ?? "";

        Comment comment = new Comment
        {
            Body = body,
            UserId = userId,
            PostId = postId
        };

        await commentRepository.AddAsync(comment);

        Console.WriteLine();
        Console.WriteLine("Comment added successfully!");
        Console.WriteLine($"Comment ID: {comment.Id}");

        Console.ReadKey();
    }
}