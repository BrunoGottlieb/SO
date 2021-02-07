using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T2
{
    class MMU
    {
        /// Para todo acesso válido a uma página, a MMU deve alterar o indicador de acesso correspondente;
        /// se o acesso for de alteração, deve também alterar o indicador de alteração.Essas são as únicas
        /// alterações que a MMU pode fazer na tabela de páginas.

        public const int ilegal = -1;
        public const int violacao = -2;

        ///  O número de entradas nesse vetor deve corresponder ao tamanho máximo de um espaço de endereçamento virtual
        public static List<int> pagesTable; // tabela de páginas

        // obter o tamanho total da memória 
        public static int GetTotalMemorySize()
        {
            return Memory.totalMemorySize;
        }

        // obter o tamanho de um quadro
        public static int GetFrameSize()
        {
            return Memory.frameSize;
        }

        // ler um inteiro de uma posição de memória
        public static int GetDataMemoryByIndex(int index)
        {
            if(index < GetTotalMemorySize() || index < 0)
            {
                return Memory.dataMemory[index];
            }
            else
            {
                return violacao;
            }

            /// deve poder retornar um erro (página inválida ou acesso inválido)
        }

        // alterar um inteiro em uma posição de memória
        public static int SetDataMemoryAtIndex(int newData, int index)
        {
            if (index < GetTotalMemorySize() || index < 0)
            {
                return Memory.dataMemory[index] = newData;
            }
            else
            {
                return violacao;
            }

            /// deve poder retornar um erro (página inválida ou acesso inválido)
        }

        public static void SetDataMemory(int[] newData)
        {
            Memory.dataMemory = newData;
        }

        public static int[] GetDataMemory()
        {
            return Memory.dataMemory;
        }

        // A MMU deve ter ainda uma forma de permitir ao SO informar qual tabela de páginas deve ser usada.
        public static void GetPageTable()
        {

        }

        /// A MMU deve ter ainda uma forma de permitir ao SO informar qual tabela de páginas deve ser usada
    }
}
