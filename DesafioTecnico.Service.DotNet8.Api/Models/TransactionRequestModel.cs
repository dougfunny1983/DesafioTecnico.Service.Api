using System.ComponentModel.DataAnnotations;

namespace DesafioTecnico.Service.DotNet8.Api.Models
{
    public sealed class TransactionRequestModel
    {
        [Required(ErrorMessage = "Account é obrigatório.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "TotalAmount é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount deve ser maior que zero.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Mcc é obrigatório.")]
        public string Mcc { get; set; }

        [Required(ErrorMessage = "Merchant é obrigatório.")]
        public string Merchant { get; set; }
    }
}
