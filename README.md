# Desafios

## L1. Autorizador Simples
- Recebe a transação.
- Usa apenas o MCC para mapear a transação para uma categoria de benefícios.
- Aprova ou rejeita a transação.
- Caso a transação seja aprovada, o saldo da categoria mapeada deverá ser diminuído em `totalAmount`.

## L2. Autorizador com Fallback
- Se o MCC não puder ser mapeado para uma categoria de benefícios ou se o saldo da categoria fornecida não for suficiente para pagar a transação inteira, verifica o saldo de CASH e, se for suficiente, debita esse saldo.

## L3. Dependente do Comerciante
- Cria um mecanismo para substituir MCCs com base no nome do comerciante. O nome do comerciante tem maior precedência sobre os MCCs.

## L4. Questão Aberta
- Transações simultâneas: dado que o mesmo cartão de crédito pode ser utilizado em diferentes serviços online, existe uma pequena mas existente probabilidade de ocorrerem duas transações ao mesmo tempo. O que você faria para garantir que apenas uma transação por conta fosse processada em um determinado momento?

## Solução

### Estrutura do Projeto
- **DesafioTecnico.Service.Api**: Contém os controladores da API.
- **DesafioTecnico.Service.Application**: Contém os serviços e modelos de aplicação.
- **DesafioTecnico.Service.IntegratedTests**: Contém os testes integrados.

### Implementação

#### L1. Autorizador Simples
- Implementado no método `ProcessTransactionAsync` da classe `TransactionService`.
- Verifica o saldo da categoria mapeada pelo MCC e aprova ou rejeita a transação.

#### L2. Autorizador com Fallback
- Adicionada verificação do saldo de CASH no método `ProcessTransactionAsync` caso o saldo da categoria mapeada não seja suficiente.

#### L3. Dependente do Comerciante
- Implementado no método `DetermineBalanceType` da classe `TransactionModel`.
- Substitui o MCC com base no nome do comerciante.

#### L4. Questão Aberta
- Utilização de semáforos (`SemaphoreSlim`) para garantir que apenas uma transação por conta seja processada em um determinado momento.

## Testes
- Testes integrados foram implementados na classe `TransactionControllerTests` para validar os diferentes cenários de autorização de transações.

## Conclusão
A solução desenvolvida implementa um autorizador de transações de cartão de crédito que atende aos desafios propostos (L1 a L3) e considera a questão aberta (L4). A seguir, destacam-se os principais pontos da implementação:
- **Autorizador Simples (L1)**: Mapeamento de transações para categorias de benefícios com base no MCC e aprovação ou rejeição de transações conforme o saldo disponível.
- **Autorizador com Fallback (L2)**: Verificação do saldo de CASH como fallback quando o saldo da categoria mapeada não é suficiente.
- **Dependente do Comerciante (L3)**: Substituição de MCCs com base no nome do comerciante, garantindo maior precisão na categorização das transações.
- **Questão Aberta (L4)**: Utilização de semáforos (`SemaphoreSlim`) para garantir que apenas uma transação por conta seja processada em um determinado momento, evitando conflitos em transações simultâneas.

A estrutura do projeto é modular e organizada, facilitando a manutenção e a escalabilidade do sistema. Além disso, foram implementados testes integrados para validar os diferentes cenários de autorização de transações, garantindo a robustez e a confiabilidade da solução.

Com essa abordagem, o sistema de autorização de transações é capaz de processar transações de forma eficiente e precisa, atendendo aos requisitos de negócio e garantindo uma experiência confiável para os usuários.

