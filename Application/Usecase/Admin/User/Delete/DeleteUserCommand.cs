using MediatR;

namespace Application.Usecase.Admin.User.Delete
{
    /// <summary>
    /// Command to delete a user (Admin only).
    /// </summary>
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }
    }
}
