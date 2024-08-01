using DesafioTecnico.Service.DotNet8.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.IntegratedTests.Helper
{
    public static class MockDataProvider
    {
        public static IEnumerable<ClientModel> GetClientModels()
        {
            return new List<ClientModel>
                {
                    new ClientModel
                    {
                        AccountId = 1,
                        TotalAmount = 1000.00m,
                        Balances = new BalancesModel
                        {
                            FOOD = 400.00m,
                            MEAL = 300.00m,
                            CASH = 300.00m
                        }
                    },
                    new ClientModel
                    {
                        AccountId = 2,
                        TotalAmount = 1500.00m,
                        Balances = new BalancesModel
                        {
                            FOOD = 600.00m,
                            MEAL = 500.00m,
                            CASH = 400.00m
                        }
                    },
                    new ClientModel
                    {
                        AccountId = 3,
                        TotalAmount = 1200.00m,
                        Balances = new BalancesModel
                        {
                            FOOD = 500.00m,
                            MEAL = 400.00m,
                            CASH = 300.00m
                        }
                    },
                    new ClientModel
                    {
                        AccountId = 4,
                        TotalAmount = 9,
                        Balances = new BalancesModel
                        {
                            FOOD = 4,
                            MEAL = 2,
                            CASH = 3
                        }
                    }
                };
        }

        public static IEnumerable<MerchantDependentModel> GetMerchantDependentModels()
        {
            return new List<MerchantDependentModel>
                {
                    new MerchantDependentModel
                    {
                        Id = 1,
                        MerchantName = "UBER TRIP                   SAO PAULO BR"
                    },
                    new MerchantDependentModel
                    {
                        Id = 2,
                        MerchantName = "UBER EATS                   SAO PAULO BR"
                    },
                    new MerchantDependentModel
                    {
                        Id = 3,
                        MerchantName = "PAG*JoseDaSilva          RIO DE JANEI BR"
                    },
                    new MerchantDependentModel
                    {
                        Id = 4,
                        MerchantName = "PICPAY*BILHETEUNICO           GOIANIA BR"
                    }
                };
        }
    }
}
