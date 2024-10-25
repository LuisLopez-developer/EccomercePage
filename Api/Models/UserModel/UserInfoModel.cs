namespace EccomercePage.Api.Models.UserModel
{
    public class UserInfoModel
    {
        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public Dictionary<string, string> Claims { get; set; } = [];
    }
}
