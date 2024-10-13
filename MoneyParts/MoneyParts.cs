using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

public class MoneyParts
{
    private readonly decimal[] denominaciones = { 200, 100, 50, 20, 10, 5, 2, 1, 0.5m, 0.2m, 0.1m, 0.05m };

    public ConcurrentBag<List<decimal>> Build(decimal monto)
    {
        ConcurrentBag<List<decimal>> resultado = new ConcurrentBag<List<decimal>>();
        GenerarCombinaciones(monto, new List<decimal>(), 0, resultado);
        return resultado;
    }

    private void GenerarCombinaciones(decimal monto, List<decimal> combinacionActual, int inicio, ConcurrentBag<List<decimal>> resultado)
    {
        if (monto == 0)
        {
            resultado.Add(new List<decimal>(combinacionActual)); 
            return;
        }

        Parallel.For(inicio, denominaciones.Length, i =>
        {
            if (denominaciones[i] <= monto)
            {
                List<decimal> nuevaCombinacion = new List<decimal>(combinacionActual);
                nuevaCombinacion.Add(denominaciones[i]);

                GenerarCombinaciones(monto - denominaciones[i], nuevaCombinacion, i, resultado);
            }
        });
    }

    public void ShowResult(decimal monto, List<List<decimal>> resultado)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        for (int i = 0; i < resultado.Count; i++) {
            List<decimal> combinacion = resultado[i];
            stringBuilder.Append(@$"[{string.Join(", ", combinacion)}]");
            if (i < resultado.Count - 1)
                stringBuilder.Append(", ");

        }
        stringBuilder.Append("]");

        Console.WriteLine(@$"Entrada: '{monto}' Salida: {stringBuilder.ToString()}");
    }

    public static void Main()
    {
        MoneyParts moneyParts = new MoneyParts();

        /*Ejemplo 1*/
        decimal monto = 0.1m;
        List<List<decimal>> resultado = moneyParts.Build(monto).ToList();

        moneyParts.ShowResult(monto, resultado);

        /*Ejemplo 2*/
        monto = 0.5m;
        resultado = moneyParts.Build(monto).ToList();

        moneyParts.ShowResult(monto, resultado);
    }
}
