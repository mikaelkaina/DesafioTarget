namespace JurosData;

static class CalculadoraJuros
{
    public const decimal TaxaDiaria = 0.025m;

    public static ResultadoJuros Calcular(decimal valor, DateOnly vencimento, DateOnly hoje)
    {
        var diasAtraso = Math.Max(0, hoje.DayNumber - vencimento.DayNumber);

        var juros = Math.Round(valor * TaxaDiaria * diasAtraso, 2, MidpointRounding.AwayFromZero);

        return new ResultadoJuros(diasAtraso, juros, valor + juros);
    }
}