using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18181_18185_PojetoIIED
{
    class FilaVaziaException : Exception
    {
        public FilaVaziaException(string erro) : base(erro)
        { }
    }
}
