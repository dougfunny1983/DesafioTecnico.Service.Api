using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Models
{
    public sealed class MerchantDependentModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("merchant_name")]
        public string MerchantName { get; set; }
    }
}
