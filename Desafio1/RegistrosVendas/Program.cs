using System.Globalization;
using System.Text.Json;

var caminho = Path.Combine(AppContext.BaseDirectory, "vendas.json");
var json = File.ReadAllText(caminho);

var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var dados = JsonSerializer.Deserialize<VendasArquivo>(json, opcoes);

if (dados is null || dados.Vendas.Count == 0)
{
    Console.WriteLine("Nenhuma venda encontrada.");
    return;
}

var pt = new CultureInfo("pt-BR");

var comissoes = dados.Vendas
    .GroupBy(v => v.Vendedor)
    .Select(g => new
    {
        Vendedor = g.Key,
        Comissao = g.Sum(v => CalcularComissao(v.Valor))
    });

foreach (var c in comissoes)
{
    Console.WriteLine($"{c.Vendedor}: {c.Comissao.ToString("C", pt)}");
}

static decimal CalcularComissao(decimal valor)
{
    var percentual = valor switch
    {
        < 100m => 0m,
        < 500m => 0.01m,
        _ => 0.05m
    };

    return Math.Round(valor * percentual, 2, MidpointRounding.AwayFromZero);
}

record Venda(string Vendedor, decimal Valor);
record VendasArquivo(List<Venda> Vendas);