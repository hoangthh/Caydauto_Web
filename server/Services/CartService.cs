using AutoMapper;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> AddToCart(CartItemCreateDto cartItemCreateDto)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
        var cartItemProductId = cartItemCreateDto.ProductId;
        var cartItemColorId = cartItemCreateDto.ColorId;
        var cartItemQuantity = cartItemCreateDto.Quantity;
        var cartItemEntity = _mapper.Map<CartItem>(cartItemCreateDto);
        return await _cartRepository.AddToCart(userId, cartItemEntity).ConfigureAwait(false);
    }
    public async Task<bool> RemoveFromCart(int cartItemId)
    {

        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
    
        //var cartItemEntity = _mapper.Map<CartItem>(cartItemCreateDto);
        return await _cartRepository.RemoveFromCart(userId, cartItemId).ConfigureAwait(false);
    }
    public async Task<bool> ClearCart()
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
        return await _cartRepository.ClearCart(userId).ConfigureAwait(false);
    }
    public async Task<CartGetDto> GetCart()
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
        var cart = await _cartRepository.GetCart(userId).ConfigureAwait(false);
        var cartDto = _mapper.Map<CartGetDto>(cart);
        return cartDto;
    }
}