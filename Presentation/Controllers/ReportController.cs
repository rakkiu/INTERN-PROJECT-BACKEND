using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // GUARD 1: Bắt buộc phải đăng nhập (có token)
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // Endpoint dùng chung: Tự động phân luồng dữ liệu theo Role trong Token
        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                // 1. Lấy thông tin User từ Token (Claims) với cơ chế fallback phòng hờ
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("id")?.Value;

                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value
                             ?? User.FindFirst("role")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(roleClaim))
                {
                    return Unauthorized(new { message = "Token không hợp lệ hoặc thiếu thông tin User/Role." });
                }

                if (!Guid.TryParse(userIdClaim, out Guid currentUserId))
                {
                    return BadRequest(new { message = "Định dạng ID người dùng trong token không hợp lệ." });
                }

                // 2. Xử lý logic ngày tháng (Fix chuẩn thứ 2 đầu tuần của .NET)
                var today = DateTime.UtcNow.Date;
                var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                var start = startDate ?? today.AddDays(-1 * diff); // Lùi về thứ 2 gần nhất
                var end = endDate ?? start.AddDays(7).AddSeconds(-1); // Kéo dài đến 23:59:59 của Chủ Nhật

                // 3. Gọi Service để lấy dữ liệu
                var report = await _reportService.GetWeeklyReportAsync(currentUserId, roleClaim, start, end);

                return Ok(new
                {
                    message = "Lấy báo cáo tuần thành công",
                    dateRange = new
                    {
                        from = start.ToString("yyyy-MM-dd"),
                        to = end.ToString("yyyy-MM-dd")
                    },
                    data = report
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Lỗi 403: Đăng nhập rồi nhưng cố tình gọi sai quyền hạn
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi 400: Các lỗi chung chung khác
                return BadRequest(new { message = ex.Message });
            }
        }

        // ---------------------------------------------------------------------------------
        // NẾU BẠN MUỐN TÁCH RỜI ENDPOINT THEO ĐÚNG TIÊU CHÍ "GUARD (CHECK ROLE)" TỪNG HÀM:
        // ---------------------------------------------------------------------------------

        [HttpGet("team-weekly")]
        [Authorize(Roles = "LEADER")] // GUARD 2: Ép cứng chỉ LEADER mới vượt qua được chốt chặn này
        public async Task<IActionResult> GetTeamWeeklyReportExplicit([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // Do đã bị chặn bởi [Authorize] ở trên, nếu chạy lọt vào trong hàm này
            // thì hệ thống chắc chắn 100% người dùng là LEADER.
            // Bạn có thể viết logic gọi Service riêng cho LEADER tại đây nếu không muốn gộp chung.

            return Ok(new { message = "Đây là API cứng dành riêng cho LEADER." });
        }
    }
}
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // GUARD 1: Bắt buộc phải đăng nhập (có token)
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // Endpoint dùng chung: Tự động phân luồng dữ liệu theo Role trong Token
        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                // 1. Lấy thông tin User từ Token (Claims) với cơ chế fallback phòng hờ
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("id")?.Value;

                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value
                             ?? User.FindFirst("role")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(roleClaim))
                {
                    return Unauthorized(new { message = "Token không hợp lệ hoặc thiếu thông tin User/Role." });
                }

                if (!Guid.TryParse(userIdClaim, out Guid currentUserId))
                {
                    return BadRequest(new { message = "Định dạng ID người dùng trong token không hợp lệ." });
                }

                // 2. Xử lý logic ngày tháng (Fix chuẩn thứ 2 đầu tuần của .NET)
                var today = DateTime.UtcNow.Date;
                var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                var start = startDate ?? today.AddDays(-1 * diff); // Lùi về thứ 2 gần nhất
                var end = endDate ?? start.AddDays(7).AddSeconds(-1); // Kéo dài đến 23:59:59 của Chủ Nhật

                // 3. Gọi Service để lấy dữ liệu
                var report = await _reportService.GetWeeklyReportAsync(currentUserId, roleClaim, start, end);

                return Ok(new
                {
                    message = "Lấy báo cáo tuần thành công",
                    dateRange = new
                    {
                        from = start.ToString("yyyy-MM-dd"),
                        to = end.ToString("yyyy-MM-dd")
                    },
                    data = report
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Lỗi 403: Đăng nhập rồi nhưng cố tình gọi sai quyền hạn
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi 400: Các lỗi chung chung khác
                return BadRequest(new { message = ex.Message });
            }
        }

        // ---------------------------------------------------------------------------------
        // NẾU BẠN MUỐN TÁCH RỜI ENDPOINT THEO ĐÚNG TIÊU CHÍ "GUARD (CHECK ROLE)" TỪNG HÀM:
        // ---------------------------------------------------------------------------------

        [HttpGet("team-weekly")]
        [Authorize(Roles = "LEADER")] // GUARD 2: Ép cứng chỉ LEADER mới vượt qua được chốt chặn này
        public async Task<IActionResult> GetTeamWeeklyReportExplicit([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // Do đã bị chặn bởi [Authorize] ở trên, nếu chạy lọt vào trong hàm này
            // thì hệ thống chắc chắn 100% người dùng là LEADER.
            // Bạn có thể viết logic gọi Service riêng cho LEADER tại đây nếu không muốn gộp chung.

            return Ok(new { message = "Đây là API cứng dành riêng cho LEADER." });
        }
    }
}