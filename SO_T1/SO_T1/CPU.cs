using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class CPU
    {
        public Status status = new Status(); // estado da CPU

        public string[] programMemory; // memoria de programa
        public int[] dataMemory; // memoria de dados
    }
}
