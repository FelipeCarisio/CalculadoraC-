using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18181_18185_PojetoIIED
{
    public interface IQueue<Dado>
    {
        void Enfileirar(Dado elemento); // inclui objeto “elemento” da classe Dado
        Dado Retirar(); // devolve objeto do início e o
                        // retira da fila
        Dado OInicio(); // devolve objeto do início
                        // sem retirá-lo da fila
        Dado OFim(); // devolve objeto do fim
                     // sem retirá-lo da fila
        int Tamanho(); // devolve número de elementos da fila
        bool EstaVazia(); // informa se a fila está vazia ou não
    }
}
