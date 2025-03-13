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
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos.ToListAsync();
            if (produtos is null)
            {
                return NotFound();
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return produto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                return NotFound("Produto não localizado...");
            }
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }
    }
}
