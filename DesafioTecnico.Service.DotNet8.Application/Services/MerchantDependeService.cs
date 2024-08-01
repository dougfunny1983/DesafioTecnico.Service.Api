using DesafioTecnico.Service.DotNet8.Application.Interfaces.Infra;
using DesafioTecnico.Service.DotNet8.Application.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Services
{
    /// <summary>
    /// Serviço responsável por validar merchants.
    /// </summary>
    public sealed class MerchantDependeService : IMerchantDependeService
    {
        private readonly IMerchantDependeRepository _merchantDependeRepository;

        private const string pattern = @"^\S+";

        private static readonly TimeSpan matchTimeout = TimeSpan.FromMilliseconds(100);

        private static readonly RegexOptions options = RegexOptions.None;

        private static readonly Regex MerchantRegex = new(pattern, options, matchTimeout);

        /// <summary>
        /// Construtor da classe MerchantDependeService.
        /// </summary>
        /// <param name="merchantDependeRepository">Repositório de dependências de merchants.</param>
        public MerchantDependeService(IMerchantDependeRepository merchantDependeRepository)
        {
            _merchantDependeRepository = merchantDependeRepository;
        }

        /// <summary>
        /// Valida se o nome do merchant é válido com base nas transações existentes.
        /// </summary>
        /// <param name="merchantName">Nome do merchant a ser validado.</param>
        /// <returns>Retorna true se o merchant for válido, caso contrário, false.</returns>
        public async Task<bool> ValidateMerchantAsync(string merchantName)
        {
            var transactions = await _merchantDependeRepository.GetAllTransactionsAsync();

            if (transactions is null || !transactions.Any())
            {
                return false;
            }

            var match = MerchantRegex.Match(merchantName);

            if (!match.Success)
            {
                return false;
            }

            var merchantPrefix = match.Value;

            // Verifica se algum merchant no banco de dados começa com o mesmo prefixo
            return transactions.Any(t => MatchesMerchantPrefix(t.MerchantName, merchantPrefix));
        }

        /// <summary>
        /// Verifica se o nome do merchant corresponde ao prefixo do merchant.
        /// </summary>
        /// <param name="merchantName">Nome do merchant a ser verificado.</param>
        /// <param name="merchantPrefix">Prefixo do merchant a ser comparado.</param>
        /// <returns>Retorna true se o nome do merchant corresponder ao prefixo, caso contrário, false.</returns>
        private static bool MatchesMerchantPrefix(string merchantName, string merchantPrefix) =>
            MerchantRegex.Match(merchantName).Value == merchantPrefix;
    }
}
