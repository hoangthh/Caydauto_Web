using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class CurrentUserService : ICurrentUserService
{
    public int? UserId { get; private set; }
    private readonly IUserRepostory _userRepository;

    public CurrentUserService(IUserRepostory userRepostory)
    {
        _userRepository = userRepostory;
    }

    public void SetUserId(int userId)
    {
        UserId = userId;
    }

    public async Task<User?> GetUser()
    {
        if (UserId == null)
            return null;
        return await _userRepository.GetByIdAsync(UserId.Value).ConfigureAwait(false);
    }
}
