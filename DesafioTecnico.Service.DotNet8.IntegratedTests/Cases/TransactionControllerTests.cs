namespace DesafioTecnico.Service.DotNet8.IntegratedTests.Cases
{
    public class TransactionControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly Fixture _fixture = new();
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IDatabaseContextFactory> _dbMock = new();
        private readonly Mock<IClientsContext> _clientContextMock = new();
        private readonly Mock<IMerchantDependeContext> _merchantDependentContextMock = new();

        public TransactionControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remova as implementações concretas
                    var descriptorClient = services.SingleOrDefault(d => d.ServiceType == typeof(IClientsContext));
                    if (descriptorClient is not null)
                    {
                        services.Remove(descriptorClient);
                    }

                    var descriptorMerchant = services.SingleOrDefault(d => d.ServiceType == typeof(IMerchantDependeContext));
                    if (descriptorMerchant is not null)
                    {
                        services.Remove(descriptorMerchant);
                    }

                    // Adicione os mocks
                    services.AddSingleton(_clientContextMock.Object);
                    services.AddSingleton(_merchantDependentContextMock.Object);

                    // Adicione o mock da fábrica
                    services.AddSingleton<IDatabaseContextFactory>(_dbMock.Object);
                });
            });

            _client = _factory.CreateClient();

            _dbMock
               .Setup(x => x.FactoryContext(EDataBase.Client))
               .Returns(_clientContextMock.Object);

            _dbMock
                .Setup(x => x.FactoryContext(EDataBase.MerchantDependent))
                .Returns(_merchantDependentContextMock.Object);
        }

        private void ConfigureMock(int accountId = 1)
        {
            var clientsModel = MockDataProvider.GetClientModels();
            var clientModel = clientsModel.FirstOrDefault(x => x.AccountId == accountId);
            var merchantDependentModel = MockDataProvider.GetMerchantDependentModels();

            _clientContextMock
                .Setup(x => x.RunGetAsync<ClientModel>(accountId))
                .ReturnsAsync(clientModel is null ? new List<ClientModel>() : new List<ClientModel> { clientModel });

            _clientContextMock
                .Setup(x => x.RunGetAllAsync<ClientModel>())
                .ReturnsAsync(clientsModel);

            _merchantDependentContextMock
                .Setup(x => x.RunGetAllAsync<MerchantDependentModel>())
                .ReturnsAsync(merchantDependentModel);
        }

        [Fact]
        public async Task ProcessTransaction_Test_L1_Autorizador_Simples_ReturnsOk()
        {
            // Arrange
            ConfigureMock();

            var payload = _fixture
                .Build<TransactionRequestModel>()
                .With(x => x.Account, "1")
                .With(x => x.TotalAmount, 100.0m)
                .With(x => x.Mcc, "5811")
                .With(x => x.Merchant, "PADARIA DO ZE               SAO PAULO BR")
                .Create();

            var expectedResponse = new TransactionResponseCodeModel(ETransactionResponseCode.Approved);

            var jsonResponse = JsonSerializer.Serialize(expectedResponse);

            var content = JsonSerializer.Serialize(payload);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/transaction/process", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();
            responseString.Should().Be(jsonResponse);
        }

        [Fact]
        public async Task ProcessTransaction_Test_Cliente_Nao_Encontrado_ReturnsError()
        {
            // Arrange
            ConfigureMock(accountId: 999); // ID de conta que não existe

            var payload = _fixture
                .Build<TransactionRequestModel>()
                .With(x => x.Account, "999")
                .With(x => x.TotalAmount, 100.0m)
                .With(x => x.Mcc, "5811")
                .With(x => x.Merchant, "PADARIA DO ZE               SAO PAULO BR")
                .Create();

            var expectedResponse = new TransactionResponseCodeModel(ETransactionResponseCode.OtherError);

            var jsonResponse = JsonSerializer.Serialize(expectedResponse);

            var content = JsonSerializer.Serialize(payload);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/transaction/process", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();
            responseString.Should().Be(jsonResponse);
        }

        [Fact]
        public async Task ProcessTransaction_Test_Saldo_Insuficiente_ReturnsError()
        {
            // Arrange
            ConfigureMock();

            var payload = _fixture
                .Build<TransactionRequestModel>()
                .With(x => x.Account, "1")
                .With(x => x.TotalAmount, 10000.0m) // Valor absurdo para garantir saldo insuficiente
                .With(x => x.Mcc, "5811")
                .With(x => x.Merchant, "PADARIA DO ZE               SAO PAULO BR")
                .Create();

            var expectedResponse = new TransactionResponseCodeModel(ETransactionResponseCode.InsufficientFunds);

            var jsonResponse = JsonSerializer.Serialize(expectedResponse);

            var content = JsonSerializer.Serialize(payload);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/transaction/process", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();
            responseString.Should().Be(jsonResponse);
        }

        [Fact]
        public async Task ProcessTransaction_Test_L2_Autorizador_Com_Fallback_ReturnsOk()
        {
            // Arrange
            ConfigureMock();

            var payload = _fixture
                .Build<TransactionRequestModel>()
                .With(x => x.Account, "1")
                .With(x => x.TotalAmount, 100.0m)
                .With(x => x.Mcc, "5411") // MCC que mapeia para FOOD
                .With(x => x.Merchant, "SUPERMERCADO")
                .Create();

            var expectedResponse = new TransactionResponseCodeModel(ETransactionResponseCode.Approved);

            var jsonResponse = JsonSerializer.Serialize(expectedResponse);

            var content = JsonSerializer.Serialize(payload);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/transaction/process", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();
            responseString.Should().Be(jsonResponse);
        }

        [Fact]
        public async Task ProcessTransaction_Test_L3_Dependencia_Do_Comerciante_ReturnsOk()
        {
            // Arrange
            ConfigureMock();

            var payload = _fixture
                .Build<TransactionRequestModel>()
                .With(x => x.Account, "1")
                .With(x => x.TotalAmount, 100.0m)
                .With(x => x.Mcc, "0000") // MCC incorreto
                .With(x => x.Merchant, "UBER EATS                   SAO PAULO BR") // Nome do comerciante que deve substituir o MCC
                .Create();

            var expectedResponse = new TransactionResponseCodeModel(ETransactionResponseCode.Approved);

            var jsonResponse = JsonSerializer.Serialize(expectedResponse);

            var content = JsonSerializer.Serialize(payload);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/transaction/process", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();
            responseString.Should().Be(jsonResponse);
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}