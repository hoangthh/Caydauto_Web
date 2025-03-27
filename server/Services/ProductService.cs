using System.Linq;
using AutoMapper;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    //private readonly IRepository<Category> _categoryRepository;
    //private readonly IRepository<Color> _colorRepository;
    private readonly FileService _fileService;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        //IRepository<Category> categoryRepository,
        //IRepository<Color> colorRepository,
        FileService fileService,
        IMapper mapper
    )
    {
        _productRepository = productRepository;
        //_categoryRepository = categoryRepository;
        //_colorRepository = colorRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    public Task<ProductGetDto> AddProduct(ProductCreateDto productPostDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductGetDto> GetProduct(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<ProductGetDto>> GetProducts(int page = 1, int pageSize = 6)
    {
        var product = await _productRepository.GetAllAsync(
            page,
            pageSize,
            true,
            q => q.IncludeMultiple(q => q.Categories, q => q.Colors, q => q.Images, q => q.Comments)
        );
        var productGetDto = _mapper.Map<IEnumerable<ProductGetDto>>(product.Items);
        return new PagedResult<ProductGetDto>
        {
            Items = productGetDto,
            TotalItems = product.TotalItems,
            PageNumber = product.PageNumber,
            PageSize = product.PageSize,
        };  
    }
    public async Task<PagedResult<ProductGetDto>> GetProducts(ProductFilter productFilter)
    {
        var product = await _productRepository.GetAllAsync(
            productFilter.Page,
            productFilter.PageSize,
            true,
            q => q.IncludeMultiple(q => q.Categories, q => q.Colors, q => q.Images, q => q.Comments).Where(p =>
                (string.IsNullOrEmpty(productFilter.Name) || p.Name.Contains(productFilter.Name, StringComparison.OrdinalIgnoreCase)) &&
                (productFilter.Categories == null || p.Categories.Any(c => productFilter.Categories.Contains(c.Name))) &&
                (productFilter.MinPrice == null || p.Price >= productFilter.MinPrice) &&
                (productFilter.MaxPrice == null || p.Price <= productFilter.MaxPrice) &&
                (productFilter.Colors == null || p.Colors.Any(c => productFilter.Colors.Contains(c.Name))) &&
                (productFilter.Brands == null || productFilter.Brands.Contains(p.Brand))).OrderByMultiple((productFilter.OrderBy, productFilter.IsDesc))
        );
        
        var productGetDto = _mapper.Map<IEnumerable<ProductGetDto>>(product.Items);
        return new PagedResult<ProductGetDto>
        {
            Items = productGetDto,
            TotalItems = product.TotalItems,
            PageNumber = product.PageNumber,
            PageSize = product.PageSize,
        };
    }
    public Task<bool> UpdateProduct(ProductPutDto productPutDto)
    {
        throw new NotImplementedException();
    }

}
