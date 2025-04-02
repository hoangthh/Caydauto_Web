using AutoMapper;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    //private readonly IRepository<Category> _categoryRepository;
    //private readonly IRepository<Color> _colorRepository;
    private readonly FileService _fileService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWishListService _wishListService;
    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ProductService(
        IProductRepository productRepository,
        //IRepository<Category> categoryRepository,
        //IRepository<Color> colorRepository,
        FileService fileService,
        ICategoryRepository categoryRepository,
        IMapper mapper,
        IWishListService wishListService,
        ICurrentUserService currentUserService,
        ILogger<ProductService> logger
    )
    {
        _productRepository = productRepository;
        //_categoryRepository = categoryRepository;
        //_colorRepository = colorRepository;
        _fileService = fileService;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _wishListService = wishListService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public Task<ProductDetailGetDto> AddProduct(ProductCreateDto productPostDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDetailGetDto?> GetProduct(int id)
    {
        var product = await _productRepository
            .GetByIdAsync(
                id,
                q =>
                    q.IncludeMultiple(
                        q => q.Categories,
                        q => q.Colors,
                        q => q.Images,
                        q => q.Comments
                    )
            )
            .ConfigureAwait(false);
        var productGetDto = _mapper.Map<ProductDetailGetDto>(product);
        return productGetDto;
    }

    public async Task<PagedResult<ProductAllGetDto>> GetProducts(int page = 1, int pageSize = 6)
    {
        var product = await _productRepository
            .GetAllAsync(
                page,
                pageSize,
                true,
                q =>
                    q.IncludeMultiple(
                        q => q.Categories,
                        q => q.Colors,
                        q => q.Images,
                        q => q.Comments
                    )
            )
            .ConfigureAwait(false);
        var productGetDto = _mapper.Map<IEnumerable<ProductAllGetDto>>(product.Items);
        await MarkWishListForProductsAsync(productGetDto.ToList()).ConfigureAwait(false);
        return new PagedResult<ProductAllGetDto>
        {
            Items = productGetDto,
            TotalItems = product.TotalItems,
            PageNumber = product.PageNumber,
            PageSize = product.PageSize,
        };
    }

    public async Task<PagedResult<ProductAllGetDto>> GetProducts(ProductFilter productFilter)
    {
        var product = await _productRepository
            .GetAllAsync(
                productFilter.Page,
                productFilter.PageSize,
                true,
                q =>
                    q.IncludeMultiple(
                            q => q.Categories,
                            q => q.Colors,
                            q => q.Images,
                            q => q.Comments
                        )
                        .Where(p =>
                            (
                                string.IsNullOrEmpty(productFilter.Name)
                                || AlgorithmFunction.LevenshteinDistance(
                                    p.Name.RemoveDiacritics().ToLower(),
                                    productFilter.Name.RemoveDiacritics().ToLower()
                                ) <= 2
                            )
                            && (
                                productFilter.Categories == null
                                || !productFilter.Categories.Any()
                                || p.Categories.Any(c => productFilter.Categories.Contains(c.Id))
                            )
                            && (productFilter.MinPrice == null || p.Price >= productFilter.MinPrice)
                            && (productFilter.MaxPrice == null || p.Price <= productFilter.MaxPrice)
                            && (
                                productFilter.Colors == null
                                || !productFilter.Colors.Any()
                                || p.Colors.Any(c => productFilter.Colors.Contains(c.Id))
                            )
                            && (
                                productFilter.Brands == null
                                || !productFilter.Brands.Any()
                                || productFilter.Brands.Contains(p.Brand)
                            )
                        )
                        .OrderByMultiple((productFilter.OrderBy, productFilter.IsDesc))
            )
            .ConfigureAwait(false);

        var productGetDto1 = _mapper.Map<IEnumerable<ProductAllGetDto>>(product.Items);
        await MarkWishListForProductsAsync(productGetDto1.ToList()).ConfigureAwait(false);
        return new PagedResult<ProductAllGetDto>
        {
            Items = productGetDto1,
            TotalItems = product.TotalItems,
            PageNumber = product.PageNumber,
            PageSize = product.PageSize,
        };
    }

    public async Task<IEnumerable<ProductAllGetDto>> GetSimilarProducts(int id)
    {
        var categoriesId = await _categoryRepository.GetAllCategoriesAsync().ConfigureAwait(false);
        var similarProducts = await _productRepository
            .GetSimilarProductsAsync(id, categoriesId.Select(c => c.Id).ToList())
            .ConfigureAwait(false);
        var productGetDto = _mapper.Map<IEnumerable<ProductAllGetDto>>(similarProducts.Items);
        await MarkWishListForProductsAsync(productGetDto.ToList()).ConfigureAwait(false);
        return productGetDto;
    }

    public Task<bool> UpdateProduct(ProductPutDto productPutDto)
    {
        throw new NotImplementedException();
    }

    public async Task<List<string>> GetBrands()
    {
        var brands = await _productRepository.GetBrandsAsync().ConfigureAwait(false);
        return brands;
    }

    private async Task MarkWishListForProductsAsync(List<ProductAllGetDto> products)
    {
        var userId = _currentUserService.UserId;
        if (userId.HasValue && products != null && products.Any())
        {
            await _wishListService
                .MarkProductsWishList(userId.Value, products)
                .ConfigureAwait(false);
        }
    }
}
