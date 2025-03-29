using Microsoft.Identity.Client;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllCategoriesAsync().ConfigureAwait(false);
    }

    public async Task<CategoryGetDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
    {
        var category = new Category
        {
            Name = categoryCreateDto.Name,
            Description = categoryCreateDto.Description,
        };

        await _categoryRepository.AddAsync(category).ConfigureAwait(false);

        return new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };
    }

    public async Task<bool> UpdateCategoryAsync(int id, CategoryPutDto categoryUpdateDto)
    {
        var category = await _categoryRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (category == null)
            return false;

        category.Name = categoryUpdateDto.Name;
        category.Description = categoryUpdateDto.Description;

        return await _categoryRepository.UpdateAsync(category).ConfigureAwait(false);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        return await _categoryRepository.DeleteAsync(id).ConfigureAwait(false);
    }
}
