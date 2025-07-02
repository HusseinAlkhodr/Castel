using Castel.Models.Authentication;

namespace Castel.DTO
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; } = true;
    }
    public class AddUserDTO : UserDTO
    {
        public AccountStatus Status { get; set; }
        public AccountType Type { get; set; }
        public string Password { get; set; }
        public List<StoreRole> Roles { get; set; } = new List<StoreRole>();
    }
    public class UpdateUserStatus : UserDTO
    {
        public AccountStatus Status { get; set; }
    }
    public class GetUserDTO : UserDTO
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<GetRoleDTO>? Roles { get; set; }
    }
    /*public class GetAccountWithTokenDTO : UserDTO
    {
        public long Id { get; set; }
        public string AccountType { get; set; }
        public TokenDTO Token { get; set; }
        public ICollection<GetRoleDTO>? Roles { get; set; }
    }*/
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    /*public class TokenDTO
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshTokenValue { get; set; }
        public RefreshToken RefreshTokenModel { get; set; }
        public bool Success { get; set; }
        public string Errors { get; set; }
    }*/
    public class RefreshFirebaseTokenDTO
    {
        public string? OldFirebaseToken { get; set; }
        public string? FirebaseToken { get; set; }
    }
    public class RoleDTO
    {
        public string RoleName { get; set; }
    }
    public class GetRoleDTO : RoleDTO
    {
        public long RoleId { get; set; }
    }
}
