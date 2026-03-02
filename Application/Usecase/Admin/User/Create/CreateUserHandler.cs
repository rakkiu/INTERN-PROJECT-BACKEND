using Application.Interfaces;
using Application.Model.User;
using Application.Utilities;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace Application.Usecase.Admin.User.Create
{
    /// <summary>
    /// Handler for CreateUserCommand.
    /// Automatically generates password and sends it to user via email.
    /// </summary>
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IEmailService _emailService;

        public CreateUserHandler(
            IUserRepository userRepository, 
            IRoleRepository roleRepository, 
            IEncryptionService encryptionService, 
            IPasswordHashService passwordHashService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _encryptionService = encryptionService;
            _passwordHashService = passwordHashService;
            _emailService = emailService;
        }

        public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email cannot be empty");
            }

            // Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with email '{request.Email}' already exists");
            }

            // Check if role exists
            var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id '{request.RoleId}' not found");
            }

            // Generate secure password
            var generatedPassword = PasswordGenerator.GenerateSecurePassword();
            var passwordHash = _passwordHashService.HashPassword(generatedPassword);

            var encryptedEmail = _encryptionService.EncryptDeterministic(request.Email.ToLower().Trim());

            var user = new Domain.Entity.User
            {
                Id = Guid.NewGuid(),
                Email = encryptedEmail,
                PasswordHash = passwordHash,
                FullName = request.FullName?.Trim(),
                RoleId = request.RoleId,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            // Send password to user email
            await SendPasswordEmailAsync(request.Email, generatedPassword, request.FullName, cancellationToken);

            return new UserResponseDto
            {
                Id = user.Id,
                Email = request.Email.ToLower().Trim(),
                FullName = user.FullName,
                IsActive = user.IsActive,
                RoleName = role.Name,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        /// <summary>
        /// Sends password to user email.
        /// </summary>
        private async System.Threading.Tasks.Task SendPasswordEmailAsync(string email, string password, string? fullName, CancellationToken cancellationToken)
        {
            var userName = fullName ?? email.Split('@')[0];
            var subject = "Your Account Has Been Created";
            var body = BuildPasswordEmailBody(userName, email, password);

            try
            {
                await _emailService.SendAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the user creation
                // In production, you might want to implement proper logging
                System.Diagnostics.Debug.WriteLine($"Failed to send password email to {email}: {ex.Message}");
            }
        }

        /// <summary>
        /// Builds the email body with password and login instructions.
        /// </summary>
        private string BuildPasswordEmailBody(string userName, string email, string password)
        {
            return $@"
<html>
    <body style='font-family: Arial, sans-serif;'>
        <h2>Welcome to Our System, {userName}!</h2>
        
        <p>Your account has been created by an administrator. Here are your login credentials:</p>
        
        <div style='background-color: #f0f0f0; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p><strong>Email:</strong> {email}</p>
            <p><strong>Password:</strong> <code style='background-color: #e0e0e0; padding: 5px; border-radius: 3px;'>{password}</code></p>
        </div>
        
        <p><strong>Important:</strong> Please change your password on your first login for security reasons.</p>
        
        <h3>Login Instructions:</h3>
        <ol>
            <li>Visit our application login page</li>
            <li>Enter your email: <strong>{email}</strong></li>
            <li>Enter your temporary password: <strong>{password}</strong></li>
            <li>Change your password immediately upon first login</li>
        </ol>
        
        <p style='color: #666; font-size: 12px; margin-top: 30px;'>
            This is an automated email. Please do not reply to this message.
            <br/>
            If you did not create an account or have questions, please contact our support team.
        </p>
    </body>
</html>";
        }
    }
}

