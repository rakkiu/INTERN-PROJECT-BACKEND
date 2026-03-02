using Domain.Interface;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Task.DeleteTask
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");

            return await _taskRepository.DeleteAsync(request.TaskId);
        }
    }
}
