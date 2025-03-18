using Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IGenreService
    {
        Task<List<GenreDto>> GetAllGenresAsync();
        Task<GenreDto> GetGenreByIdAsync(int id);
        Task AddGenreAsync(GenreDto genreDto);
        Task UpdateGenreAsync(GenreDto genreDto);
        Task DeleteGenreAsync(int id);
    }
}
