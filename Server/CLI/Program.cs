using CLI;
using FileRepositories;
using RepositoryContracts;

IUserRepository userRepository = new UserFileRepository();
IPostRepository postRepository = new PostFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

CLIApp app = new CLIApp(
    postRepository,
    userRepository,
    commentRepository
);

await app.Run();