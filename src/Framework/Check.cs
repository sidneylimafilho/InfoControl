using System;
using System.Collections.Generic;
using System.Text;

namespace InfoControl
{
    public class Check
    {
#if !CompactFramework
        /// <summary>
        /// Função que verifica se um objeto independente do formato pode ser convertido para numérico
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns>Se a conversão for possivel então retorna True. Caso contrário retorna False.</returns>
        public static bool IsNumeric(object Expression)
        {
            // Define uma variável que coletara o resultado da função TryParse.
            // Se a conversão falhar essa variável terá zero.
            double retNum;

            // O TryParse não gera exceção quando a conversão falha.
            return Double.TryParse(
                Convert.ToString(Expression),
                System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.InvariantInfo,
                out retNum);
        }
#endif

        /// <summary>
        /// Verifica se um objeto não é nulo
        /// </summary>
        /// <param name="valueToCheck">Valor a ser verificado se é nulo</param>
        /// <returns>Se o objeto não for nulo então retorna True, caso contrário retorna False</returns>
        public static bool IsNotNull(object valueToCheck)
        {
            return (valueToCheck != null);
        }
    }
}
