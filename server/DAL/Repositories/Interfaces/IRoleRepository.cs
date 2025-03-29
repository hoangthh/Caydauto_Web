public interface IRoleRepository : IRepository<Role>
{
    Task<Role> GetUserRole ();
    Task<Role> GetAdminRole();
}