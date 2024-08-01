using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Models
{
    public sealed class BalancesModel
    {
        [JsonPropertyName("FOOD")]
        public decimal FOOD { get; set; }

        [JsonPropertyName("MEAL")]
        public decimal MEAL { get; set; }

        [JsonPropertyName("CASH")]
        public decimal CASH { get; set; }
    }
}
