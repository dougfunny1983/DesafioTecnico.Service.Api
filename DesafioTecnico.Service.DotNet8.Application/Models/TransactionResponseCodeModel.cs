using DesafioTecnico.Service.DotNet8.Application.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Models
{
    public class TransactionResponseCodeModel
    {
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        public TransactionResponseCodeModel(ETransactionResponseCode code)
        {
            int codeInt = (int)code;
            Code = codeInt.ToString("D2");
        }
    }
}
