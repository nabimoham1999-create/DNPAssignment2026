using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public ListPostsView(
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public void Show()
    {
        Console.Clear();

        Console.WriteLine("========== ALL POSTS ==========");
        Console.WriteLine();

        foreach (Post post in postRepository.GetMany())
        {
            User? user = userRepository
                .GetMany()
                .FirstOrDefault(u => u.Id == post.UserId);

            Console.WriteLine($"ID: {post.Id}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Author: {user?.UserName}");
            Console.WriteLine($"Body: {post.Body}");
            Console.WriteLine("--------------------------------");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to go back...");
        Console.ReadKey();
    }
}