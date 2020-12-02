using System;
using System.Collections.Generic;
using System.IO;

namespace SO_T1
{
    class Program
    {
        public const bool debugMode = true;

        static void Main(string[] args)
        {
            string path = "C://teste/SO.txt"; // diretorio
            int[] dados = new int[4]; // // dados do programa

            CPU cpu = new CPU(); // instancia da CPU
            Status status = new Status(); // instancia dos status do CPU

            if (File.Exists(path)) // confere se o diretorio existe e le o seu conteudo
            {
                string content = File.ReadAllText(path);
                string[] array = content.Split("\n");
                cpu.SetCPUProgramMemory(cpu, array); // manda as instrucoes para a cpu
            }
            else
            {
                Console.WriteLine("Path " + path + " was not found.");
            }

            for (int i = 0; i < cpu.programMemory.Length; i++) // exibe o conteudo da memoria de programa [instrucoes]
            {
                Console.WriteLine(i + " : " + cpu.programMemory[i]);
            }

            SO.Initialize(cpu, status, dados);
        }
    }
}
