using Application.Interfaces;
using Application.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private readonly ITaskRepository _taskRepository;

        public ReportService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<WeeklyReportResponseDto> GetWeeklyReportAsync(Guid currentUserId, string role, DateTime startDate, DateTime endDate)
        {
            IEnumerable<Domain.Entities.TaskEntity> tasks;

            // Xử lý phân quyền xem dữ liệu theo Role
            if (role.ToUpper() == "LEADER")
            {
                // LEADER: Xem report team (Lấy tất cả task của team trong khoảng thời gian)
                // Lưu ý: Nếu hệ thống có nhiều team, bạn cần truyền thêm TeamId vào hàm này.
                tasks = await _taskRepository.GetTeamTasksByDateRangeAsync(startDate, endDate);
            }
            else if (role.ToUpper() == "MEMBER")
            {
                // MEMBER: Chỉ xem report của mình
                tasks = await _taskRepository.GetUserTasksByDateRangeAsync(currentUserId, startDate, endDate);
            }
            else
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xem báo cáo này.");
            }

            // Tính tự động: Số task hoàn thành & Tổng giờ
            // Giả định TaskEntity có Status == "Completed" hoặc IsCompleted == true
            // và trường LoggedHours (double/int) lưu số giờ làm việc.
            var completedTasks = tasks.Where(t => t.Status == "Completed").ToList();

            var report = new WeeklyReportResponseDto
            {
                TotalCompletedTasks = completedTasks.Count,
                TotalHoursLogged = tasks.Sum(t => t.LoggedHours) // Tổng giờ của TẤT CẢ task (hoặc chỉ task hoàn thành tùy logic của bạn)
            };

            return report;
        }
    }
}