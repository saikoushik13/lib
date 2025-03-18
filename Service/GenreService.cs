using AutoMapper;
using Database.Entities;
using DatabaseOperations.Interface;
using Models.DTO;
using Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreDatabaseOperations _genreDbOperations;
        private readonly IMapper _mapper;

        public GenreService(IGenreDatabaseOperations genreDbOperations, IMapper mapper)
        {
            _genreDbOperations = genreDbOperations;
            _mapper = mapper;
        }

        public async Task<List<GenreDto>> GetAllGenresAsync()
        {
            var genres = await _genreDbOperations.GetAllAsync();
            return _mapper.Map<List<GenreDto>>(genres);
        }

        public async Task<GenreDto> GetGenreByIdAsync(int id)
        {
            var genre = await _genreDbOperations.GetByIdAsync(id);
            return _mapper.Map<GenreDto>(genre);
        }

        public async Task AddGenreAsync(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            await _genreDbOperations.AddAsync(genre);
            await _genreDbOperations.SaveChangesAsync();
        }

        public async Task UpdateGenreAsync(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            await _genreDbOperations.UpdateAsync(genre);
            await _genreDbOperations.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(int id)
        {
            await _genreDbOperations.DeleteAsync(id);
            await _genreDbOperations.SaveChangesAsync();
        }
    }
}
