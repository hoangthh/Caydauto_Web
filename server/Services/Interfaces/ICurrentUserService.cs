public interface ICurrentUserService
{
    int? UserId { get; }
    void SetUserId(int userId);
}
