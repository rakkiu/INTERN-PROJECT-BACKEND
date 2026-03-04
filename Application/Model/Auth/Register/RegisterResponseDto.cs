using System;

namespace Application.Model.Auth.Register
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;

        // ĐÁP ỨNG YÊU CẦU 2: KHÔNG CÓ TRƯỜNG PASSWORD Ở ĐÂY
    }
}