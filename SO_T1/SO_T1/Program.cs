using System;
using System.Collections.Generic;
using System.IO;

namespace SO_T1
{
    class Program
    {
        public const int normal = 0;
        public const int ilegal = 1;
        public const int violacao = 2;

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
                API.SetCPUProgramMemory(cpu, array); // manda as instrucoes para a cpu
            }
            else
            {
                Console.WriteLine("Path " + path + " was not found.");
            }

            for (int i = 0; i < cpu.programMemory.Length; i++) // exibe o conteudo da memoria de programa [instrucoes]
            {
                Console.WriteLine(i + " : " + cpu.programMemory[i]);
            }

            API.InitializeCPU(status); // inicializa o estado da cpu

            API.UpdateCPUStatus(cpu, status); // altera o estado da cpu

            API.SetCPUDataMemory(cpu, dados); // envia os dados para a cpu

            while (API.GetCPUInterruptionCode(cpu) == normal)
            {
                API.ExecuteCPU(cpu); // executa as instrucoes caso a cpu esteja em estado normal
            }

            if(debugMode)
            {
                Console.WriteLine("----------");

                Console.WriteLine("CPU parou na instrução " + cpu.programMemory[cpu.status.PC] + " (deve ser PARA)");
                Console.WriteLine("O valor de m[0] é " + cpu.dataMemory[0] + " (deve ser 42)");
            }
        }

    }
}
