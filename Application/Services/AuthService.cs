using Application.Interfaces;
using Application.Model.Auth.Register;
using BCrypt.Net;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Kiểm tra trùng lặp (Optional nhưng nên có)
            var isExist = await _userRepository.CheckUsernameExistAsync(request.Username);
            if (isExist)
            {
                throw new Exception("Username đã tồn tại!");
            }

            // ĐÁP ỨNG YÊU CẦU 1: Hash password trước khi lưu DB
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Tạo Entity
            var userEntity = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = hashedPassword
            };

            // Lưu vào Database
            await _userRepository.CreateUserAsync(userEntity);

            // Map sang DTO để trả về (đã loại bỏ Password)
            return new RegisterResponseDto
            {
                Id = userEntity.Id,
                Username = userEntity.Username
            };
        }
    }
}