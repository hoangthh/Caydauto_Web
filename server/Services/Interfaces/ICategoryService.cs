public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<CategoryGetDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
    Task<bool> UpdateCategoryAsync(int id, CategoryPutDto categoryUpdateDto);
    Task<bool> DeleteCategoryAsync(int id);
}