using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;

    public CommentController(
        ICommentRepository commentRepo,
        IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
    }

    
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? userId,
        [FromQuery] string? userName,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> query = commentRepo.GetMany();

        // Filter by user ID
        if (userId.HasValue)
        {
            query = query.Where(comment =>
                comment.UserId == userId.Value);
        }

        // Filter by username
        if (!string.IsNullOrWhiteSpace(userName))
        {
            IQueryable<int> matchingUserIds = userRepo
                .GetMany()
                .Where(user =>
                    user.UserName.Contains(
                        userName,
                        StringComparison.OrdinalIgnoreCase))
                .Select(user => user.Id);

            query = query.Where(comment =>
                matchingUserIds.Contains(comment.UserId));
        }

        // Filter by post ID
        if (postId.HasValue)
        {
            query = query.Where(comment =>
                comment.PostId == postId.Value);
        }

        IEnumerable<CommentDto> comments = query
            .Select(comment => new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            });

        return Ok(comments);
    }

    // GET /Comment/{id}
    [HttpGet("{id:int}")]
    public ActionResult<CommentDto> GetComment(int id)
    {
        Comment? comment = commentRepo
            .GetMany()
            .FirstOrDefault(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        CommentDto dto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId
        };

        return Ok(dto);
    }

    // POST /Comment
    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(
        CreateCommentDto dto)
    {
        Comment comment = new Comment
        {
            Body = dto.Body,
            UserId = dto.UserId,
            PostId = dto.PostId
        };

        Comment createdComment =
            await commentRepo.AddAsync(comment);

        CommentDto commentDto = new CommentDto
        {
            Id = createdComment.Id,
            Body = createdComment.Body,
            UserId = createdComment.UserId,
            PostId = createdComment.PostId
        };

        return CreatedAtAction(
            nameof(GetComment),
            new { id = createdComment.Id },
            commentDto);
    }

    // PUT /Comment/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(
        int id,
        CreateCommentDto dto)
    {
        Comment comment = new Comment
        {
            Id = id,
            Body = dto.Body,
            UserId = dto.UserId,
            PostId = dto.PostId
        };

        await commentRepo.UpdateAsync(comment);

        return NoContent();
    }

    // DELETE /Comment/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        await commentRepo.DeleteAsync(id);

        return NoContent();
    }
}