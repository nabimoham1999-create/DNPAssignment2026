using Entities;
using InMemoryRepositories;
using RepositoryContracts;

IPostRepository postRepository = new PostInMemoryRepository();
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

Console.WriteLine("Forum Application");
Console.WriteLine();

while (true)
{
    Console.WriteLine("1. Create user");
    Console.WriteLine("2. Create post");
    Console.WriteLine("3. Create comment");
    Console.WriteLine("4. View posts");
    Console.WriteLine("5. Exit");

    Console.Write("Choose an option: ");
    string? choice = Console.ReadLine();

    Console.WriteLine();

    switch (choice)
    {
        case "1":
            await CreateUser(userRepository);
            break;

        case "2":
            await CreatePost(postRepository, userRepository);
            break;

        case "3":
            await CreateComment(
                commentRepository,
                postRepository,
                userRepository);
            break;

        case "4":
            await ViewPosts(postRepository);
            break;

        case "5":
            return;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }

    Console.WriteLine();
}

static async Task CreateUser(IUserRepository userRepository)
{
    Console.Write("Username: ");
    string? username = Console.ReadLine();

    Console.Write("Password: ");
    string? password = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(username) ||
        string.IsNullOrWhiteSpace(password))
    {
        Console.WriteLine("Username and password are required.");
        return;
    }

    User user = new User
    {
        UserName = username,
        Password = password
    };

    User createdUser = await userRepository.AddAsync(user);

    Console.WriteLine(
        $"User created with ID: {createdUser.Id}");
}

static async Task CreatePost(
    IPostRepository postRepository,
    IUserRepository userRepository)
{
    Console.Write("User ID: ");
    string? userInput = Console.ReadLine();

    if (!int.TryParse(userInput, out int userId))
    {
        Console.WriteLine("Invalid user ID.");
        return;
    }

    try
    {
        await userRepository.GetSingleAsync(userId);
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("User does not exist.");
        return;
    }

    Console.Write("Title: ");
    string? title = Console.ReadLine();

    Console.Write("Body: ");
    string? body = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title) ||
        string.IsNullOrWhiteSpace(body))
    {
        Console.WriteLine("Title and body are required.");
        return;
    }

    Post post = new Post
    {
        Title = title,
        Body = body,
        UserId = userId
    };

    Post createdPost = await postRepository.AddAsync(post);

    Console.WriteLine(
        $"Post created with ID: {createdPost.Id}");
}

static async Task ViewPosts(IPostRepository postRepository)
{
    IQueryable<Post> posts = postRepository.GetMany();

    if (!posts.Any())
    {
        Console.WriteLine("There are no posts.");
        return;
    }

    foreach (Post post in posts)
    {
        Console.WriteLine($"ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine($"User ID: {post.UserId}");
        Console.WriteLine("--------------------");
    }

    await Task.CompletedTask;
}

static async Task CreateComment(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IUserRepository userRepository)
{
    Console.Write("User ID: ");
    string? userInput = Console.ReadLine();

    if (!int.TryParse(userInput, out int userId))
    {
        Console.WriteLine("Invalid user ID.");
        return;
    }

    try
    {
        await userRepository.GetSingleAsync(userId);
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("User does not exist.");
        return;
    }

    Console.Write("Post ID: ");
    string? postInput = Console.ReadLine();

    if (!int.TryParse(postInput, out int postId))
    {
        Console.WriteLine("Invalid post ID.");
        return;
    }

    try
    {
        await postRepository.GetSingleAsync(postId);
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("Post does not exist.");
        return;
    }

    Console.Write("Comment: ");
    string? body = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(body))
    {
        Console.WriteLine("Comment cannot be empty.");
        return;
    }

    Comment comment = new Comment
    {
        Body = body,
        UserId = userId,
        PostId = postId
    };

    Comment createdComment =
        await commentRepository.AddAsync(comment);

    Console.WriteLine(
        $"Comment created with ID: {createdComment.Id}");
}