public interface ICurrentUserService
{
    int? UserId { get; }
    void SetUserId(int userId);
    Task<User?> GetUser();
}
