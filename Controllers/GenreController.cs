using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreService.GetAllGenresAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            if (genre == null) return NotFound(new { message = "Genre not found" });

            return Ok(genre);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] GenreDto genreDto)
        {
            await _genreService.AddGenreAsync(genreDto);
            return Ok(new { message = "Genre added successfully" });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] GenreDto genreDto)
        {
            await _genreService.UpdateGenreAsync(genreDto);
            return Ok(new { message = "Genre updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.DeleteGenreAsync(id);
            return Ok(new { message = "Genre deleted successfully" });
        }
    }
}
