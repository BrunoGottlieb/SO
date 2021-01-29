using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T2
{
    class Memory
    {
        static int[] memory; // memoria fisica
        static int pageSize = 10; // tamanho da pagina
        static int totalMemorySize = 1000; // tamanho total da memoria
        static int frameSize = pageSize; // tamanho do quadro

        // obter o tamanho total da memória 
        public static int getTotalMemorySize()
        {
            return totalMemorySize;
        }

        // obter o tamanho de um quadro
        public static int getFrameSize()
        {
            return frameSize;
        }

        // ler um inteiro de uma posição da memória;
        public static int readContentFromMemoryPosition(int index)
        {
            return memory[index];
        }

        // alterar um inteiro de uma posição da memória;
        public static void updateMemoryContentAtIndex(int newData, int index)
        {
            memory[index] = newData;
        }

        // ler o conteúdo de um quadro da memória;
        public static int getContentFromMemoryPosition(int index)
        {
            return memory[index];
        }

        // alterar o conteúdo de um quadro da memória.
        public static void setMemoryFrameAtIndex(int newData, int index)
        {

        }
    }
}
