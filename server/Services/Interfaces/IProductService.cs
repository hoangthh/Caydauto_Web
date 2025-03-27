using Microsoft.AspNetCore.Mvc.RazorPages;

public interface IProductService
{
    public Task<PagedResult<ProductGetDto>> GetProducts(int page = 1, int pageSize = 6);
    public Task<PagedResult<ProductGetDto>> GetProducts(ProductFilter productFilter);
    public Task<ProductGetDto> GetProduct(int id);
    public Task<ProductGetDto> AddProduct(ProductCreateDto productPostDto);
    public Task<bool> UpdateProduct(ProductPutDto productPutDto);
    public Task<bool> DeleteProduct(int id);
}