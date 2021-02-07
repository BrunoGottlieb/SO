using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T2
{
    class Memory
    {
        public static int[] dataMemory; // memoria fisica
        public static int pageSize = 10; // tamanho da pagina
        public static int totalMemorySize = 1000; // tamanho total da memoria
        public static int frameSize = pageSize; // tamanho do quadro
    }
}
