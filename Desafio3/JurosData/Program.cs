using System.Globalization;
using JurosData;

var pt = new CultureInfo("pt-BR");

Console.Write("Valor (ex.: 1000,50): ");
if (!decimal.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, pt, out var valor) || valor <= 0)
{
    Console.WriteLine("Valor inválido. Informe um número maior que zero, usando vírgula para os centavos.");
    return;
}

Console.Write("Data de vencimento (dd/MM/yyyy): ");
if (!DateOnly.TryParseExact(Console.ReadLine()?.Trim(), "dd/MM/yyyy", pt, DateTimeStyles.None, out var vencimento))
{
    Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy.");
    return;
}

var hoje = DateOnly.FromDateTime(DateTime.Today);
var resultado = CalculadoraJuros.Calcular(valor, vencimento, hoje);

Console.WriteLine();

if (resultado.DiasAtraso == 0)
{
    Console.WriteLine("A conta não está vencida. Não há juros a cobrar.");
}
else
{
    Console.WriteLine($"Dias de atraso: {resultado.DiasAtraso}");
    Console.WriteLine($"Juros: {resultado.Juros.ToString("C", pt)}");
}

Console.WriteLine($"Total a pagar: {resultado.TotalAPagar.ToString("C", pt)}");
