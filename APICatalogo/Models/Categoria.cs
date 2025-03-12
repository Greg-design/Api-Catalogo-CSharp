using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria : IValidatableObject
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        [StringLength(300, ErrorMessage = "A URL da imagem deve ter no máximo 300 caracteres")]
        [Url(ErrorMessage = "A URL da imagem deve ser válida")]
        public string ImagemUrl { get; set; }

        public ICollection<Produto>? Produtos { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validação para garantir que o nome não contenha apenas espaços em branco
            if (string.IsNullOrEmpty(Nome) && Nome?.Trim().Length == 0)
            {
                yield return new ValidationResult(
                    "O nome não pode conter apenas espaços em branco",
                    new[] { nameof(Nome) }
                );
            }

            // Validação para garantir que o nome não contenha caracteres especiais
            if (Nome?.IndexOfAny(new char[] { '@', '#', '$', '%', '&', '*' }) >= 0)
            {
                yield return new ValidationResult(
                    "O nome não pode conter caracteres especiais",
                    new[] { nameof(Nome) }
                );
            }

            // Validação para garantir que a URL da imagem seja de um domínio confiável
            if (!string.IsNullOrEmpty(ImagemUrl))
            {
                var urlLowerCase = ImagemUrl.ToLower();
                var dominiosConfiaveis = new[] { "https://", "http://" };
                
                if (!dominiosConfiaveis.Any(d => urlLowerCase.StartsWith(d)))
                {
                    yield return new ValidationResult(
                        "A URL da imagem deve começar com http:// ou https://",
                        new[] { nameof(ImagemUrl) }
                    );
                }
            }
        }
    }
}
