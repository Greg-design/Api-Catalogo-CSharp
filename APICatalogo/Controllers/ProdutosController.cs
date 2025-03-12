using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(AppDbContext context, ILogger<ProdutosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            try
            {
                var produtos = await _context.Produtos
                    .AsNoTracking()
                    .ToListAsync();

                if (!produtos.Any())
                    return NoContent();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os produtos");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto is null)
                {
                    _logger.LogWarning("Produto não encontrado para o ID: {Id}", id);
                    return NotFound($"Produto com id={id} não encontrado");
                }

                return Ok(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter produto por ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest("Dados inválidos");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();

                return CreatedAtRoute("ObterProduto",
                    new { id = produto.ProdutoId },
                    produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar novo produto");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                    return BadRequest("O id informado não corresponde ao produto");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var produtoExistente = await _context.Produtos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produtoExistente is null)
                    return NotFound($"Produto com id={id} não encontrado");

                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto ID: {Id}", id);
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
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto is null)
                {
                    _logger.LogWarning("Tentativa de deletar produto inexistente ID: {Id}", id);
                    return NotFound($"Produto com id={id} não encontrado");
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar produto ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar sua solicitação. Por favor, tente novamente mais tarde.");
            }
        }
    }
}
