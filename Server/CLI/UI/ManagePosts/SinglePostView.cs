using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;

    public SinglePostView(
        IPostRepository postRepository,
        IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task Show()
    {
        Console.Clear();

        Console.WriteLine("========== SINGLE POST ==========");
        Console.WriteLine();

        Console.Write("Enter Post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid ID.");
            Console.ReadKey();
            return;
        }

        Post? post = postRepository
            .GetMany()
            .FirstOrDefault(p => p.Id == postId);

        if (post == null)
        {
            Console.WriteLine("Post not found.");
            Console.ReadKey();
            return;
        }

        User? author = userRepository
            .GetMany()
            .FirstOrDefault(u => u.Id == post.UserId);

        Console.WriteLine();
        Console.WriteLine($"ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Author: {author?.UserName}");
        Console.WriteLine();
        Console.WriteLine(post.Body);

        Console.WriteLine();
        Console.WriteLine("========== COMMENTS ==========");

        var comments = commentRepository
            .GetMany()
            .Where(c => c.PostId == postId);

        foreach (Comment comment in comments)
        {
            User? user = userRepository
                .GetMany()
                .FirstOrDefault(u => u.Id == comment.UserId);

            Console.WriteLine();
            Console.WriteLine($"{user?.UserName}: {comment.Body}");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to go back...");
        Console.ReadKey();

        await Task.CompletedTask;
    }
}