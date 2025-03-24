using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class CategoriaDTO
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        [StringLength(300, ErrorMessage = "A URL da imagem deve ter no máximo 300 caracteres")]
        [Url(ErrorMessage = "A URL da imagem deve ser válida")]
        public string ImagemUrl { get; set; }
    }
}
