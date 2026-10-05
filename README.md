# Desafio Técnico

## Como executar
Requisitos: .NET 10 SDK.

    dotnet run --project Desafio1/RegistrosVendas
    dotnet run --project Desafio2/MovimentacaoProdutos
    dotnet run --project Desafio3/JurosData

## Desafio 1 - Comissão de vendedores
Lê o arquivo `vendas.json` e calcula a comissão total de cada vendedor.

Regra aplicada, por venda:
- abaixo de R$ 100,00: sem comissão
- de R$ 100,00 até R$ 499,99: 1%
- a partir de R$ 500,00: 5%

Decisões:
- Uso de `decimal` para valores monetários, evitando erros de ponto flutuante.
- Limites das faixas tratados explicitamente (R$ 500,00 exatos entram nos 5%).
- A comissão é arredondada a 2 casas por venda (como seria o lançamento de cada uma) e depois somada por vendedor.
- A regra de cálculo fica isolada em uma função, separada da leitura e da exibição dos dados.

## Desafio 2 - Movimentação de estoque
Programa de console com menu para lançar entradas e saídas de mercadoria. Os produtos são carregados do arquivo `estoque.json`, e ao final de cada movimentação o programa exibe a quantidade final em estoque do produto movimentado.

Cada movimentação possui:
- um identificador numérico único (sequencial);
- o tipo (entrada ou saída), que define se o saldo aumenta ou diminui;
- uma descrição informada pelo usuário (ex.: "Compra de fornecedor", "Venda balcão").

Regras aplicadas:
- a quantidade deve ser maior que zero;
- o produto precisa existir;
- uma saída não pode ser maior que o estoque disponível (não há estoque negativo).

Decisões:
- As regras de negócio ficam na classe `Deposito`, separadas do menu em `Program.cs`, que só lê e exibe dados.
- A validação de estoque acontece logo após a quantidade ser informada, para não pedir a descrição de uma movimentação que já se sabe inválida.
- O ID é um contador sequencial, já que o enunciado pede um número único e os dados ficam apenas em memória. Por isso, o contador reinicia a cada execução. Se as movimentações fossem persistidas, o ID seria gerado pelo banco de dados.
- O estoque não é gravado de volta no JSON, pois o enunciado não pede persistência.

## Desafio 3 - Juros por atraso
Programa de console que recebe um valor e uma data de vencimento e calcula os juros de atraso até a data de hoje.

Entrada:
- valor, com vírgula para os centavos (ex.: `1000,50`);
- data de vencimento no formato `dd/MM/yyyy`.

Regra aplicada:
- 2,5% de juros por dia de atraso, sobre o valor original (juros simples);
- se a conta vence hoje ou no futuro, não há atraso e os juros são zero.

Suposições e decisões:
- O enunciado fala em "multa de 2,5% ao dia" e pede o valor dos juros. Interpretei como uma taxa de juros diária de 2,5% por dia de atraso.
- Juros simples, e não compostos, já que o enunciado não menciona capitalização.
- O cálculo fica em uma função separada que recebe a data de hoje por parâmetro, o que a torna previsível e fácil de testar. O `Program.cs` só trata a entrada e a saída.
- Uso de `decimal` para valores e `DateOnly` para datas, evitando erros de ponto flutuante e de horário.
- O valor é arredondado a 2 casas, da mesma forma que no desafio 1.
