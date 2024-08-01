using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Configurations
{
    public static class Configurations
    {
        public const string StringErrorMessage = "A variável de ambiente '{0}' não pode ser nula ou vazia.";

        #region [Variaveis de Ambiente - paths]

        private static readonly string PATTERN = ValidateEnvironmentVariable("PATTERN");
        private static readonly string PATH_INITIAL = ValidateEnvironmentVariable("PATH_INITIAL");
        private static readonly string PATH_CLIENTS = ValidateEnvironmentVariable("PATH_CLIENTS");
        private static readonly string PATH_MERCHANT_DEPENDENT = ValidateEnvironmentVariable("PATH_MERCHANT_DEPENDENT");

        #endregion [Variaveis de Ambiente - paths]

        #region [Encapsuladores]

        public static string PathClients => ResolvePath(PATH_INITIAL, PATH_CLIENTS);
        public static string PathMerchantDepende => ResolvePath(PATH_INITIAL, PATH_MERCHANT_DEPENDENT);

        #endregion [Encapsuladores]

        #region [Conversores]

        private static string ResolvePath(string pathInitial, string path)
        {
            var basePath = GetProjectDirectory();

            string relativePath = pathInitial + path;

            string pathFinal = Path.Combine(basePath, relativePath);

            return pathFinal;
        }

        private static string GetProjectDirectory()
        {
            var basePath = Directory.GetCurrentDirectory();

            int index = basePath.IndexOf(PATTERN);

            if (index != -1)
            {
                basePath = basePath.Remove(index, PATTERN.Length);
            }

            return basePath;
        }

        private static string ValidateEnvironmentVariable(string variableName)
        {
            var value = Environment.GetEnvironmentVariable(variableName) ?? string.Empty;
            return value;
        }

        #endregion [Conversores]
    }
}
