using System.ComponentModel.DataAnnotations;

namespace CadastroClienteService.Models
{
    public class Cliente : IValidatableObject
    {
        public Cliente()
        {
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo nome/razão social é obrigatório")]
        public string? NomeRazaoSocial { get; set; }

        public string? CpfCnpj { get; set; }
        public string? RgInscEst { get; set; }

        [Required(ErrorMessage = "Campo logradouro é obrigatório")]
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Campo bairro é obrigatório")]
        public string? Bairro { get; set; }

        [Required(ErrorMessage = "Campo cidade é obrigatório")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "Campo cep é obrigatório")]
        public string? Cep { get; set; }

        [Required(ErrorMessage = "Campo estado é obrigatório")]
        public string? Estado { get; set; }
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Campo celular é obrigatório")]
        public string? Celular { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Cliente cliente &&
                   Id.Equals(cliente.Id) &&
                   NomeRazaoSocial == cliente.NomeRazaoSocial &&
                   CpfCnpj == cliente.CpfCnpj &&
                   RgInscEst == cliente.RgInscEst &&
                   Logradouro == cliente.Logradouro &&
                   Numero == cliente.Numero &&
                   Complemento == cliente.Complemento &&
                   Bairro == cliente.Bairro &&
                   Cidade == cliente.Cidade &&
                   Cep == cliente.Cep &&
                   Estado == cliente.Estado &&
                   Telefone == cliente.Telefone &&
                   Celular == cliente.Celular;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.Equals(NomeRazaoSocial, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                yield return new("Campo nome é obrigatório");
            }

            if (string.Equals(Logradouro, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                yield return new("Campo nome é obrigatório");
            }

            if (string.Equals(Bairro, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                yield return new("Campo nome é obrigatório");
            }

            if (string.Equals(Cidade, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                yield return new("Campo nome é obrigatório");
            }

            if (string.Equals(Cep, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                yield return new("Campo nome é obrigatório");
            }

            if (string.Equals(Estado, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                yield return new("Campo nome é obrigatório");
            }

            if (string.Equals(Celular, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                yield return new("Campo nome é obrigatório");
            }
        }
    }
}