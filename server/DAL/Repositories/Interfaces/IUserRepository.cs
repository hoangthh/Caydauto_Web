public interface IUserRepostory : IRepository<User>{
    Task<User?> GetUserWithRoleByIdAsync(int id);
    Task<bool> AddProductIntoWishList(User user, Product product);
    Task<bool> RemoveProductFromWishList(User user, Product product);
}