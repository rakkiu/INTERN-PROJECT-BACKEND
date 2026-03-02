using MediatR;

namespace Application.Usecase.Role.Delete
{
    /// <summary>
    /// Command to delete a role.
    /// </summary>
    public class DeleteRoleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteRoleCommand(Guid id)
        {
            Id = id;
        }
    }
}
