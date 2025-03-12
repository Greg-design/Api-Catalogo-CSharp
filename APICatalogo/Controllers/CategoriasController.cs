using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Trás as categorias junto com os seus produtos relacionados
        [HttpGet("produtos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
            try
            {
                var categorias = await _context.Categorias
                    .Include(p => p.Produtos)
                    .AsNoTracking()
                    .ToListAsync();

                if (!categorias.Any())
                    return NoContent();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias com produtos");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        // Trás somente as categorias
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            try
            {
                var categorias = await _context.Categorias
                    .AsNoTracking()
                    .ToListAsync();

                if (!categorias.Any())
                    return NoContent();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as categorias");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            try
            {
                var categoria = await _context.Categorias
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.CategoriaId == id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria não encontrada para o ID: {Id}", id);
                    return NotFound($"Categoria com o id={id} não encontrada.");
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria por ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                    return BadRequest("Dados inválidos");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _context.Categorias.AddAsync(categoria);
                await _context.SaveChangesAsync();

                return CreatedAtRoute("ObterCategoria", 
                    new { id = categoria.CategoriaId }, 
                    categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar nova categoria");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                    return BadRequest("O id informado não corresponde à categoria");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var categoriaExistente = await _context.Categorias
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.CategoriaId == id);

                if (categoriaExistente is null)
                    return NotFound($"Categoria com o id={id} não encontrada");

                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar categoria ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(p => p.CategoriaId == id);

                if (categoria is null)
                {
                    _logger.LogWarning("Tentativa de deletar categoria inexistente ID: {Id}", id);
                    return NotFound($"Categoria com o id={id} não encontrada.");
                }

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar categoria ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }
    }
}
