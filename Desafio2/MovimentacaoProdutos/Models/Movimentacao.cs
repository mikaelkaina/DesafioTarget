namespace MovimentacaoProdutos.Models;

record Movimentacao(int Id, int CodigoProduto, TipoMovimentacao Tipo, int Quantidade, string Descricao);
