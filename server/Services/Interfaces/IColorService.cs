using AutoMapper;

public interface IColorService {
    Task<IEnumerable<Color>> GetAllColorsAsync();
    Task<ColorGetDto> CreateColorAsync(ColorCreateDto colorCreateDto);
    Task<bool> UpdateColorAsync(int id, ColorPutDto colorUpdateDto);
    Task<bool> DeleteColorAsync(int id);
    Task<bool> IsColorUsedAsync(int id);
}

public class ColorService {
    private readonly IColorRepository _colorRepository;
    private readonly IMapper _mapper;

    public ColorService(IColorRepository colorRepository, IMapper mapper) {
        _colorRepository = colorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ColorGetDto>> GetAllColorsAsync() {
        var colors = await _colorRepository.GetAllAsync().ConfigureAwait(false);
        var colorDtos = _mapper.Map<IEnumerable<ColorGetDto>>(colors);
        return colorDtos;

    }
    public async Task<ColorGetDto> CreateColorAsync(ColorCreateDto colorCreateDto) {
        var isColorCreated = await _colorRepository.IsColorCreatedAsync(colorCreateDto.Name).ConfigureAwait(false);
        if (isColorCreated) {
            throw new Exception($"Color with name {colorCreateDto.Name} already exists.");
        }
        var color = _mapper.Map<Color>(colorCreateDto);
        var result = await _colorRepository.AddAsync(color).ConfigureAwait(false);
        if (result == null) {
            throw new Exception("Failed to create color.");
        }
        return _mapper.Map<ColorGetDto>(color);
    }
    public async Task<bool> UpdateColorAsync(int id, ColorPutDto colorUpdateDto) {
        var color = await _colorRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (color == null) {
            throw new KeyNotFoundException($"Color with ID {id} not found.");
        }
        var isColorCreated = await _colorRepository.IsColorCreatedAsync(colorUpdateDto.Name, id).ConfigureAwait(false);
        if (isColorCreated) {
            throw new Exception($"Color with name {colorUpdateDto.Name} already exists.");
        }
        color.Name = colorUpdateDto.Name;
        var result = await _colorRepository.UpdateAsync(color).ConfigureAwait(false);
        if (result == false) {
            throw new Exception("Failed to update color.");
        }
        return true;
    }

}