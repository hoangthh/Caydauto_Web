public interface IUserRepostory : IRepository<User>{
    Task<User?> GetUserWithRoleByIdAsync(int id);
}