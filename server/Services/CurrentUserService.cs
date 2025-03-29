public class CurrentUserService : ICurrentUserService {
    public int? UserId { get; private set; }

    public void SetUserId(int userId)
    {
        UserId = userId;
    }
}