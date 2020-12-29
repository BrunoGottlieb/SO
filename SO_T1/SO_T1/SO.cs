using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SO_T1
{
    class SO
    {
        static string path = "C://teste/ES/"; // diretorio

        public static void Initialize(CPU cpu, Status status, int[] dados)
        {
            cpu.InitializeCPU(status); // inicializa o estado da cpu

            cpu.UpdateCPUStatus(cpu, status); // altera o estado da cpu

            cpu.SetCPUDataMemory(cpu, dados); // envia os dados para a cpu

            InterruptionManager interruptionManager = new InterruptionManager();

            interruptionManager.Execute(cpu);
        }

        public static void Execute(CPU cpu)
        {
            //cpu.ExecuteCPU(cpu); // executa as instrucoes caso a cpu esteja em estado normal
        }

        public static void IlegalHandler(CPU cpu)
        {
            Status status = cpu.GetCPUStatus(cpu);

            string origem = cpu.GetPCInstruction(cpu); // retorna a instrucao atual do PC

            string[] instrucao = origem.Split(' ');
            string instruction = instrucao[0];

            string valueTmp = origem.Remove(0, origem.IndexOf(' ') + 1);

            int value = 0;
            if (Char.IsDigit(valueTmp[0]))
                value = Convert.ToInt32(valueTmp);

            // pede ao SO para fazer a leitura de um dado (inteiro) do dispositivo de E/S n; o dado será colocado no acumulador
            if (instruction == "LE")
            {
                Console.WriteLine("LE on SO");
                // salva o estado da CPU
                Status s = cpu.GetCPUStatus(cpu);
                // coloca a CPU em estado dormindo
                s.InterruptionCode = 3;
                cpu.UpdateCPUStatus(cpu, s);
                // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna
                //Timer.NewInterruption('P', 10, 1);
                // realiza a operação 
                string fullPath = path + cpu.value.ToString() + ".txt";
                string content = File.ReadAllText(fullPath);
                s.A = Int32.Parse(content);
                // recupera o estado da CPU
                s.InterruptionCode = 1;
                cpu.UpdatePC(cpu);
                cpu.UpdateCPUStatus(cpu, s);
                // chama o retorno de interrupção da CPU e retorna

            }
            else // pede ao SO gravar o valor do acumulador no dispositivo de E/S n
            if (instruction == "GRAVA")
            {
                Console.WriteLine("GRAVA on SO");

                // salva o estado da CPU
                Status s = cpu.GetCPUStatus(cpu);
                // coloca a CPU em estado dormindo
                s.InterruptionCode = 3;
                cpu.UpdateCPUStatus(cpu, s);
                // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna
                Timer.NewInterruption('P', 10, 1);
                // realiza a operação 
                string fullPath = path + cpu.value.ToString() + ".txt";
                File.WriteAllText(fullPath, s.A.ToString());
                // recupera o estado da CPU
                s.InterruptionCode = 1;
                s.PC = s.PC++;
                cpu.UpdateCPUStatus(cpu, s);
                // chama o retorno de interrupção da CPU e retorna
            }
            else // pede ao SO para terminar a execução do programa (como exit)
            if (instruction == "PARA")
            {
                Console.WriteLine("\nIlegal ended\n");
                //Console.WriteLine("CPU parou na instrução " + cpu.programMemory[cpu.status.PC] + " (deve ser PARA)");
                //Console.WriteLine("O valor de m[0] é " + cpu.dataMemory[0] + " (deve ser 42)");
                Environment.Exit(0);
            }
            else
            {
                ViolationHandler(cpu); // violacao de memoria
            }
        }

        public static void ViolationHandler(CPU cpu)
        {
            Console.WriteLine("\nViolation ended");
            Environment.Exit(0);
        }

    }
}
