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

        ///  O número de entradas nesse vetor deve corresponder ao tamanho máximo de um espaço de endereçamento virtual
        public static Page[] pagesTable = new Page[10]; // tabela de páginas

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

        public static int GetNextFreeFrame() // retorna o proximo quadro livre da memoria fisica
        {
            for(int i = 0; i < Memory.dataMemory.Length; i++)
            {
                if(Memory.dataMemory[i].isValid == false)
                {
                    return i; // retorna o quadro livre
                }
            }

            return -1; // Nao ha quadros livres
        }

        // converte endereco virtual para endereco fisico
        private static void AddressConveter(int newData, int index) 
        {
            int frame = index / Memory.pageSize; // numero do quadro para onde vai o dado
            int offset = index - (frame * Memory.pageSize); // deslocamento dentro do quadro

            Page pagina = pagesTable[frame]; // pagina do processo para onde vai o dado
            pagina.content[offset] = newData; // poe o novo conteudo na memoria da pagina correspondente
            Memory.dataMemory[pagina.frameNum] = pagina; // atualiza a memoria fisica
        }

        public static bool CheckPageFault(int index)
        {
            int frame = index / Memory.pageSize; // numero do quadro para onde vai o dado

            if (pagesTable[frame].isValid == false) // confere se a pagina esta mapeada
            {
                Console.WriteLine("\n\nPAGINA NAO MAPEADA: " + index + "\n\n");
                return true; // retorna erro de falta de pagina
            }

            return false;
        }

        public static bool CheckViolation(int index)
        {
            if (index > Memory.totalMemorySize || index < 0) // endereco de memoria eh invalido
            {
                Console.WriteLine("\n\nVIOLACAO: " + index + "\n\n");
                return true; // retorna violacao
            }

            return false;
        }

        // alterar um inteiro em uma posição de memória
        public static void SetDataMemoryAtIndex(int newData, int index)
        {
            Console.WriteLine("\n\nSetDataMemoryAtIndex: " + index + "\n\n");

            AddressConveter(newData, index); // converte endereco virtual para endereco fisico | retorna sucesso ou falha

            //return Memory.dataMemory[index] = newData;
        }

        // ler um inteiro de uma posição de memória
        public static int GetDataMemoryByIndex(int index)
        {
            Console.WriteLine("\n\nGetDataMemoryByIndex: " + index + "\n\n");

            int frame = index / Memory.pageSize; // numero do quadro para onde vai o dado
            int offset = index - (frame * Memory.pageSize); // deslocamento dentro do quadro

            Page pagina = pagesTable[frame]; // pagina do processo para onde vai o dado
            return pagina.content[offset]; // poe o novo conteudo na memoria da pagina correspondente
        }

        /*public static void SetDataMemory(int[] newData)
        {
            Memory.dataMemory = newData;
        }

        public static int[] GetDataMemory()
        {
            return Memory.dataMemory;
        }*/

        // A MMU deve ter ainda uma forma de permitir ao SO informar qual tabela de páginas deve ser usada.
        public static void GetPageTable()
        {

        }

        public static void SetMemoryFrameValidity(int index, bool state) // altera a validade de um quadro da memoria fisica
        {
            Memory.dataMemory[index].isValid = state;
            Console.WriteLine("Validou o quadro " + index);
            Console.WriteLine("IS VALID " + Memory.dataMemory[index].isValid);
        }

        /// A MMU deve ter ainda uma forma de permitir ao SO informar qual tabela de páginas deve ser usada
    }
}
