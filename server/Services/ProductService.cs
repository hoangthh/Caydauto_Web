using AutoMapper;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    //private readonly IRepository<Category> _categoryRepository;
    //private readonly IRepository<Color> _colorRepository;
    private readonly FileService _fileService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWishListService _wishListService;
    private readonly IColorRepository _colorRepository;
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
        ILogger<ProductService> logger,
        IColorRepository colorRepository
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
        _colorRepository = colorRepository;
    }

    public async Task<ProductDetailGetDto?> AddProduct(ProductCreateDto productDto)
    {
        if (productDto == null)
            throw new ArgumentNullException(nameof(productDto));

        // Step 1: Validate the DTO (already handled by data annotations, but we can add additional checks if needed)
        if (productDto.Price <= 0)
            throw new ArgumentException("Price must be greater than 0.", nameof(productDto.Price));

        if (productDto.StockQuantity < 0)
            throw new ArgumentException(
                "Stock quantity cannot be negative.",
                nameof(productDto.StockQuantity)
            );

        // Step 2: Validate Categories
        if (productDto.CategoryIds == null || !productDto.CategoryIds.Any())
            throw new ArgumentException(
                "At least one category must be specified.",
                nameof(productDto.CategoryIds)
            );

        var categorieEntity = await _categoryRepository
            .GetAllCategoriesAsync()
            .ConfigureAwait(false);
        var categories = categorieEntity.Where(c => productDto.CategoryIds.Contains(c.Id)).ToList();
        if (categories.Count != productDto.CategoryIds.Count)
        {
            var invalidCategoryIds = productDto
                .CategoryIds.Except(categories.Select(c => c.Id))
                .ToList();
            throw new ArgumentException(
                $"Invalid category IDs: {string.Join(", ", invalidCategoryIds)}"
            );
        }

        // Step 3: Validate Colors
        if (productDto.ColorIds == null || !productDto.ColorIds.Any())
            throw new ArgumentException(
                "At least one color must be specified.",
                nameof(productDto.ColorIds)
            );

        var colorEntities = await _colorRepository.GetAllAsync().ConfigureAwait(false);
        var colors = colorEntities.Where(c => productDto.ColorIds.Contains(c.Id)).ToList();
        if (colors.Count != productDto.ColorIds.Count)
        {
            var invalidColorIds = productDto.ColorIds.Except(colors.Select(c => c.Id)).ToList();
            throw new ArgumentException($"Invalid color IDs: {string.Join(", ", invalidColorIds)}");
        }

        // Step 4: Validate Images
        if (productDto.Images == null || !productDto.Images.Any())
            throw new ArgumentException(
                "At least one image must be provided.",
                nameof(productDto.Images)
            );

        var imageFiles = productDto
            .Images.Select(i => i.File)
            .Where(f => f != null && f.Length > 0)
            .ToList();
        if (imageFiles == null || !imageFiles.Any())
            throw new ArgumentException(
                "No valid image files provided.",
                nameof(productDto.Images)
            );

        // Step 5: Map ProductCreateDto to Product
        var product = _mapper.Map<Product>(productDto);

        // Set CreatedDate and UpdatedDate (from IDateTracking)
        product.CreatedDate = DateTime.Now;
        product.UpdatedDate = DateTime.Now;

        // Step 6: Associate Categories and Colors
        product.Categories = categories;
        product.Colors = colors;

        // Step 7: Add the Product to the database (without images for now)
        var addedProduct = await _productRepository.AddAsync(product).ConfigureAwait(false);
        if (addedProduct == null)
            throw new Exception("Failed to add product to the database.");
        // Step 8: Save Images using FileService
        try
        {
            var imageUrls = await _fileService
                .SaveImagesAsync(imageFiles!, "Products", product.Id)
                .ConfigureAwait(false);

            // Create Image entities and associate them with the Product
            product.Images = imageUrls
                .Select(url => new Image { Url = url, ProductId = product.Id })
                .ToList();

            // Update the Product with the images
            addedProduct.Images = product.Images;
            await _productRepository.UpdateAsync(addedProduct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Rollback: Delete the product if image saving fails
            await _productRepository.DeleteAsync(addedProduct.Id).ConfigureAwait(false);
            throw new IOException("Failed to save product images: " + ex.Message, ex);
        }

        // Step 9: Return the created Product (including navigation properties if needed)
        return await GetProduct(addedProduct.Id).ConfigureAwait(false);
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
