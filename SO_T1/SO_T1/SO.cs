using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class SO
    {
        public static void Initialize(CPU cpu, Status status, int[] dados)
        {
            cpu.InitializeCPU(status); // inicializa o estado da cpu

            cpu.UpdateCPUStatus(cpu, status); // altera o estado da cpu

            cpu.SetCPUDataMemory(cpu, dados); // envia os dados para a cpu

            cpu.InterruptionManager();
        }

        public static void Execute(CPU cpu)
        {
            //cpu.ExecuteCPU(cpu); // executa as instrucoes caso a cpu esteja em estado normal
        }

        public static void IlegalHandler(CPU cpu)
        {
            Console.WriteLine("\nIlegal ended\n");
            Console.WriteLine("CPU parou na instrução " + cpu.programMemory[cpu.status.PC] + " (deve ser PARA)");
            Console.WriteLine("O valor de m[0] é " + cpu.dataMemory[0] + " (deve ser 42)");
            Environment.Exit(0);
        }

        public static void ViolationHandler(CPU cpu)
        {
            Console.WriteLine("\nViolation ended");
            Environment.Exit(0);
        }

    }
}
