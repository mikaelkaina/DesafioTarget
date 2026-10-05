using System.Text.Json;
using MovimentacaoProdutos;
using MovimentacaoProdutos.Models;

var caminho = Path.Combine(AppContext.BaseDirectory, "estoque.json");
var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var arquivo = JsonSerializer.Deserialize<EstoqueArquivo>(File.ReadAllText(caminho), opcoes);

if (arquivo is null || arquivo.Estoque.Count == 0)
{
    Console.WriteLine("Nenhum produto encontrado.");
    return;
}

var deposito = new Deposito(arquivo.Estoque);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Listar estoque");
    Console.WriteLine("2 - Lançar movimentação");
    Console.WriteLine("3 - Ver movimentações");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": ListarEstoque(deposito); break;
        case "2": LancarMovimentacao(deposito); break;
        case "3": ListarMovimentacoes(deposito); break;
        case "0": return;
        default: Console.WriteLine("Opção inválida."); break;
    }
}

static void ListarEstoque(Deposito deposito)
{
    foreach (var p in deposito.Produtos)
        Console.WriteLine($"{p.CodigoProduto} - {p.DescricaoProduto}: {p.Estoque}");
}

static void ListarMovimentacoes(Deposito deposito)
{
    if (deposito.Movimentacoes.Count == 0)
    {
        Console.WriteLine("Nenhuma movimentação lançada.");
        return;
    }

    foreach (var m in deposito.Movimentacoes)
        Console.WriteLine($"#{m.Id} | Produto {m.CodigoProduto} | {m.Tipo} | Qtde {m.Quantidade} | {m.Descricao}");
}

static void LancarMovimentacao(Deposito deposito)
{
    var codigo = LerInteiro("Código do produto: ");
    var produto = codigo is null ? null : deposito.BuscarProduto(codigo.Value);

    if (produto is null)
    {
        Console.WriteLine("Produto não encontrado.");
        return;
    }

    Console.Write("Tipo (E = entrada, S = saída): ");
    TipoMovimentacao? tipo = Console.ReadLine()?.Trim().ToUpperInvariant() switch
    {
        "E" => TipoMovimentacao.Entrada,
        "S" => TipoMovimentacao.Saida,
        _ => null
    };

    if (tipo is null)
    {
        Console.WriteLine("Tipo inválido.");
        return;
    }

    var quantidade = LerInteiro("Quantidade: ");
    if (quantidade is null)
    {
        Console.WriteLine("Quantidade inválida.");
        return;
    }

    var erro = deposito.Validar(produto, tipo.Value, quantidade.Value);
    if (erro is not null)
    {
        Console.WriteLine(erro);
        return;
    }

    Console.Write("Descrição da movimentação: ");
    var descricao = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(descricao))
    {
        Console.WriteLine("A descrição é obrigatória.");
        return;
    }

    var mov = deposito.Movimentar(produto, tipo.Value, quantidade.Value, descricao);
    Console.WriteLine($"Movimentação #{mov.Id} registrada ({mov.Tipo} - {mov.Descricao}).");
    Console.WriteLine($"Estoque final de {produto.DescricaoProduto}: {produto.Estoque}");
}

static int? LerInteiro(string mensagem)
{
    Console.Write(mensagem);
    return int.TryParse(Console.ReadLine(), out var n) ? n : null;
}