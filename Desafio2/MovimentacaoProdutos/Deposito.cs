using MovimentacaoProdutos.Models;

namespace MovimentacaoProdutos;

class Deposito
{
    private readonly List<Produto> _produtos;
    private readonly List<Movimentacao> _movimentacoes = new();
    private int _proximoId = 1;

    public Deposito(List<Produto> produtos) => _produtos = produtos;

    public IReadOnlyList<Produto> Produtos => _produtos;
    public IReadOnlyList<Movimentacao> Movimentacoes => _movimentacoes;

    public Produto? BuscarProduto(int codigo) =>
        _produtos.FirstOrDefault(p => p.CodigoProduto == codigo);

    public string? Validar(Produto produto, TipoMovimentacao tipo, int quantidade)
    {
        if (quantidade <= 0)
            return "A quantidade deve ser maior que zero.";

        if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
            return $"Estoque insuficiente. Disponível: {produto.Estoque}.";

        return null;
    }

    public Movimentacao Movimentar(Produto produto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        var erro = Validar(produto, tipo, quantidade);
        if (erro is not null)
            throw new InvalidOperationException(erro);

        produto.Estoque += tipo == TipoMovimentacao.Entrada ? quantidade : -quantidade;

        var movimentacao = new Movimentacao(_proximoId++, produto.CodigoProduto, tipo, quantidade, descricao);
        _movimentacoes.Add(movimentacao);
        return movimentacao;
    }
}
