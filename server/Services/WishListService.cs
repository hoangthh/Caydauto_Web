using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class WishListService : IWishListService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepostory _userRepository;
    private readonly IMapper _mapper;

    public WishListService(
        ICurrentUserService currentUserService,
        IProductRepository productRepository,
        IUserRepostory userRepository,
        IMapper mapper
    )
    {
        _currentUserService = currentUserService;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<WishListResponse> AddProductToWishList(int productId)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
            return new WishListResponse
            {
                IsSuccess = false,
                Message = "You must be logged in to add products to your wish list",
            };
        var userEntities = await _userRepository
            .GetByIdAsync(userId.Value, q => q.Include(q => q.WishList))
            .ConfigureAwait(false);
        if (userEntities == null)
            return new WishListResponse { IsSuccess = false, Message = "User not found" };
        var productEntity = await _productRepository
            .GetByIdAsync(productId, q => q.Include(p => p.Images))
            .ConfigureAwait(false);
        if (productEntity == null)
            return new WishListResponse { IsSuccess = false, Message = "Product not found" };
        var result = await _userRepository
            .AddProductIntoWishList(userEntities, productEntity)
            .ConfigureAwait(false);
        if (!result)
        {
            return new WishListResponse
            {
                IsSuccess = false,
                Message = "Failed to add product to your wish ",
            };
        }
        return new WishListResponse
        {
            IsSuccess = true,
            wishListItemDto = _mapper.Map<WishListItemDto>(productEntity),
            Message = "Product added to your wish list",
        };
    }

    public async Task<PagedResult<WishListItemDto>> GetUserWishList()
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            return new PagedResult<WishListItemDto>
            {
                Items = new List<WishListItemDto>(),
                TotalItems = 0,
                PageNumber = 1,
                PageSize = 10,
            };
        }

        var user = await _userRepository
            .GetByIdAsync(userId.Value, q => q.Include(u => u.WishList).ThenInclude(p => p.Images))
            .ConfigureAwait(false);

        if (user == null || user.WishList == null)
        {
            return new PagedResult<WishListItemDto>
            {
                Items = new List<WishListItemDto>(),
                TotalItems = 0,
                PageNumber = 1,
                PageSize = 10,
            };
        }

        var wishListItems = _mapper.Map<List<WishListItemDto>>(user.WishList);

        return new PagedResult<WishListItemDto>
        {
            Items = wishListItems,
            TotalItems = wishListItems.Count,
            PageNumber = 1,
            PageSize = wishListItems.Count,
        };
    }

    public async Task<bool> MarkProductsWishList(int userId, List<ProductAllGetDto> products)
    {
        if (products == null || !products.Any())
            return false;

        var getUserWishList = await _userRepository
            .GetByIdAsync(userId, q => q.Include(q => q.WishList))
            .ConfigureAwait(false);

        if (getUserWishList == null)
            return false;

        var wishListIds = getUserWishList.WishList.Select(w => w.Id).ToList();

        foreach (var product in products)
        {
            product.IsWished = wishListIds.Contains(product.Id);
        }

        return true;
    }

    public async Task<bool> RemoveProductFromWishList(int productId)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            return false;
        }

        var user = await _userRepository
            .GetByIdAsync(userId.Value, q => q.Include(u => u.WishList))
            .ConfigureAwait(false);

        if (user == null)
        {
            return false;
        }

        var product = user.WishList.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            return false;
        }

        var result = await _userRepository
            .RemoveProductFromWishList(user, product)
            .ConfigureAwait(false);

        return result;
    }
}

public class WishListResponse
{
    public bool IsSuccess;
    public WishListItemDto? wishListItemDto;
    public string? Message;
}
