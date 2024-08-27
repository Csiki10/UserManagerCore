namespace UserManagerCore.Helpers
{
    public enum LoginState
    {
        Succes,
        Error,
        Invalid
    }
    public class LoginResult
    {
        public LoginState State { get; set; }
        public string Error{ get; set; }
    }
}
