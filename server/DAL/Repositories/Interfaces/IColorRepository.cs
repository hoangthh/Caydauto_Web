public interface IColorRepository : IRepository<Color>
{
    Task<IEnumerable<Color>> GetAllAsync();

    Task<bool> IsColorCreatedAsync(string colorName, int? id = null);

    
}
