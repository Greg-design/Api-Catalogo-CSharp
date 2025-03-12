using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do produto é obrigatória")]
        [StringLength(300, ErrorMessage = "A descrição deve ter entre 5 e 300 caracteres", MinimumLength = 5)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço do produto é obrigatório")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        [StringLength(300, ErrorMessage = "A URL da imagem deve ter no máximo 300 caracteres")]
        [Url(ErrorMessage = "A URL da imagem deve ser válida")]
        public string ImagemUrl { get; set; }

        [Range(0, 10000, ErrorMessage = "O estoque deve estar entre 0 e 10000")]
        public float Estoque { get; set; }

        public DateTime DataCadastro { get; set; }

        [Required(ErrorMessage = "A categoria do produto é obrigatória")]
        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataCadastro == DateTime.MinValue)
            {
                yield return new ValidationResult(
                    "Data de cadastro inválida",
                    new[] { nameof(DataCadastro) }
                );
            }

            if (string.IsNullOrEmpty(Nome) && Nome?.Trim().Length == 0)
            {
                yield return new ValidationResult(
                    "O nome não pode conter apenas espaços em branco",
                    new[] { nameof(Nome) }
                );
            }

            // Validação customizada para garantir que o preço seja compatível com o mercado
            if (Preco > 100000)
            {
                yield return new ValidationResult(
                    "O preço parece estar muito alto. Verifique o valor informado",
                    new[] { nameof(Preco) }
                );
            }
        }
    }
}
