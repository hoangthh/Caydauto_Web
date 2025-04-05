using System.Collections;
using Microsoft.AspNetCore.Mvc.RazorPages;

public interface IProductService
{
    public Task<PagedResult<ProductAllGetDto>> GetProducts(int page = 1, int pageSize = 6);
    public Task<PagedResult<ProductAllGetDto>> GetProducts(ProductFilter productFilter);
    public Task<ProductDetailGetDto?> GetProduct(int id);
    public Task<IEnumerable<ProductAllGetDto>> GetSimilarProducts(int id);
    public Task<ProductDetailGetDto?> AddProduct(ProductCreateDto productPostDto);
    public Task<bool> UpdateProduct(ProductPutDto productPutDto);
    public Task<bool> DeleteProduct(int id);
    public Task<List<string>> GetBrands();
}