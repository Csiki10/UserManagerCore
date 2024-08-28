namespace UserManagerCore.Helpers
{
    public enum LoginState
    {
        Succes = 0,
        InvalidPassword = 1,
        InvalidUserName = 2
    }
    public class LoginResult
    {
        public LoginState State { get; set; }
        public string Error{ get; set; }
    }
}
