namespace SimplePocketBase.Auth
{
    public class AuthResponse<T>
    {
        public string Token { get; set; }
        public T Record { get; set; }
    }
}
