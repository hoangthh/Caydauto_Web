using AutoMapper;

public interface IColorService {
    Task<IEnumerable<ColorGetDto>> GetAllColorsAsync();
    Task<ColorGetDto> CreateColorAsync(ColorCreateDto colorCreateDto);
    Task<bool> UpdateColorAsync(int id, ColorPutDto colorUpdateDto);
    Task<bool> DeleteColorAsync(int id);
    Task<bool> IsColorUsedAsync(int id);
}

