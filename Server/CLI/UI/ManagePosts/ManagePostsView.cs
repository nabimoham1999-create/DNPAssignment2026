using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;

    public ManagePostsView(
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
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("==============================");
            Console.WriteLine("        MANAGE POSTS");
            Console.WriteLine("==============================");
            Console.WriteLine();
            Console.WriteLine("1. List posts");
            Console.WriteLine("2. Create post");
            Console.WriteLine("3. View single post");
            Console.WriteLine("4. Delete post");
            Console.WriteLine("0. Back");
            Console.WriteLine();

            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    ListPostsView listView = new ListPostsView(
                        postRepository,
                        userRepository
                    );

                    listView.Show();
                    break;

                case "2":
                    CreatePostView createView = new CreatePostView(
                        postRepository,
                        userRepository
                    );

                    await createView.Show();
                    break;

                case "3":
                    SinglePostView singleView = new SinglePostView(
                        postRepository,
                        userRepository,
                        commentRepository
                    );

                    await singleView.Show();
                    break;

                case "4":
                    await DeletePost();
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

    private async Task DeletePost()
    {
        Console.Clear();

        Console.WriteLine("========== DELETE POST ==========");
        Console.WriteLine();

        Console.Write("Enter Post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            Console.ReadKey();
            return;
        }

        bool exists = postRepository
            .GetMany()
            .Any(p => p.Id == id);

        if (!exists)
        {
            Console.WriteLine("Post not found.");
            Console.ReadKey();
            return;
        }

        await postRepository.DeleteAsync(id);

        Console.WriteLine("Post deleted successfully.");
        Console.ReadKey();
    }
}