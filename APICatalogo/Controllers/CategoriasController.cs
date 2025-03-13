using APICatalogo.Context;
using APICatalogo.Filters;
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
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
           
            _logger.LogInformation("================ GET categorias/produtos ================");

            var categorias = await _context.Categorias
                 .Include(p => p.Produtos)
                 .AsNoTracking()
                 .ToListAsync();

             if (!categorias.Any())
                 return NoContent();

             return Ok(categorias);
            
        }

        // Trás somente as categorias
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }

        // obtém uma única categoria
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(p => p.CategoriaId == id);

            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            _context.Entry(categoria).State = EntityState.Modified;
             await _context.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(p => p.CategoriaId == id);

            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }
    }
}
