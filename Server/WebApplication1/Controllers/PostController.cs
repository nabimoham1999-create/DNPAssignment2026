using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    
    public PostController(IPostRepository postRepo, 
        IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;   
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts(
        [FromQuery]string? title,
        [FromQuery] int? userId,
        [FromQuery] string? userName)
    {
        IQueryable<Post> query = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(post =>
                post.Title.Contains(
                    title,
                    StringComparison.OrdinalIgnoreCase));
        }
        if (userId.HasValue)
        {
            query = query.Where(post =>
                post.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            IQueryable<int> matchingUserIds = userRepo
                .GetMany()
                .Where(user =>
                    user.UserName.Contains(
                        userName,
                        StringComparison.OrdinalIgnoreCase))
                .Select(user => user.Id);

            query = query.Where(post =>
                matchingUserIds.Contains(post.UserId));
        }

        
        IEnumerable<PostDto> posts = query
            .Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            });

        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PostDto> GetPost(int id)
    {
        Post? post = postRepo
            .GetMany()
            .FirstOrDefault(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        PostDto dto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        };

        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto dto)
    {
        Post post = new Post
        {
            Title = dto.Title,
            Body = dto.Body,
            UserId = dto.UserId
        };

        Post createdPost = await postRepo.AddAsync(post);

        PostDto postDto = new PostDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            UserId = createdPost.UserId
        };

        return CreatedAtAction(
            nameof(GetPost),
            new { id = createdPost.Id },
            postDto);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost(int id, CreatePostDto dto)
    {
        Post post = new Post
        {
            Id = id,
            Title = dto.Title,
            Body = dto.Body,
            UserId = dto.UserId
        };

        await postRepo.UpdateAsync(post);

        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        await postRepo.DeleteAsync(id);

        return NoContent();
    }
    
}