using CLI;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

// -------------------------
// Dummy Users
// -------------------------

await userRepository.AddAsync(new User
{
    UserName = "Nabi",
    Password = "1234"
});

await userRepository.AddAsync(new User
{
    UserName = "John",
    Password = "1234"
});

await userRepository.AddAsync(new User
{
    UserName = "Maria",
    Password = "1234"
});

await userRepository.AddAsync(new User
{
    UserName = "Peter",
    Password = "1234"
});

// -------------------------
// Dummy Posts
// -------------------------

await postRepository.AddAsync(new Post
{
    Title = "Welcome to VIA",
    Body = "This is my first post about VIA.",
    UserId = 1
});

await postRepository.AddAsync(new Post
{
    Title = "My first week",
    Body = "I am enjoying my first week at university.",
    UserId = 2
});

await postRepository.AddAsync(new Post
{
    Title = "Learning C#",
    Body = "C# is becoming more interesting as I practice it.",
    UserId = 3
});

await postRepository.AddAsync(new Post
{
    Title = "Project Work",
    Body = "Today we are working on our project.",
    UserId = 4
});

// -------------------------
// Dummy Comments
// -------------------------

await commentRepository.AddAsync(new Comment
{
    Body = "This is a great post!",
    UserId = 2,
    PostId = 1
});

await commentRepository.AddAsync(new Comment
{
    Body = "I also like VIA.",
    UserId = 3,
    PostId = 1
});

await commentRepository.AddAsync(new Comment
{
    Body = "Good luck with your project!",
    UserId = 1,
    PostId = 4
});

await commentRepository.AddAsync(new Comment
{
    Body = "C# is really useful.",
    UserId = 4,
    PostId = 3
});

// -------------------------
// Start CLI
// -------------------------

CLIApp app = new CLIApp(
    postRepository,
    userRepository,
    commentRepository
);

await app.Run();