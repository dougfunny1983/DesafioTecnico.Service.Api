using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.Application.Enuns
{
    public enum EBalanceType
    {
        [Description("Se o mcc for '5411' ou '5412', deve-se utilizar o saldo de FOOD.")]
        FOOD,

        [Description("Se o mcc for '5811' ou '5812', deve-se utilizar o saldo de MEAL.")]
        MEAL,

        [Description("Para quaisquer outros valores do mcc, deve-se utilizar o saldo de CASH.")]
        CASH
    }
}
