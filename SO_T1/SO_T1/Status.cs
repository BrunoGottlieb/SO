using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class Status
    {
        public int PC { get; set; } // registrador | o contador de programa
        public int A { get; set; } // registrador | acumulador
        public int InterruptionCode { get; set; }// codigo de interrupcao // 0 normal | 1 instrucao ilegal | 2 violacao de memoria
    }
}
