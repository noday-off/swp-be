namespace SWP391_PreCookingPackage.ModelsDTO
{
    public partial class UserModel
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int? Role { get; set; }
        public List<OrderModel> Orders { get; set; } = new List<OrderModel>(); 
    }

    public partial class UserRegisterModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int? Role { get; set; }
    }

    public class UserLoginModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}