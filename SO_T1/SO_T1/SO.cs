using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SO_T1
{
    class SO
    {
        static string path = "C://teste/ES/"; // diretorio

        public const int normal = 0;
        public const int ilegal = 1;
        public const int violacao = 2;
        public const int sleeping = 3;

        public static void Initialize(CPU cpu, Status status, int[] dados)
        {
            cpu.InitializeCPU(status); // inicializa o estado da cpu

            cpu.UpdateCPUStatus(cpu, status); // altera o estado da cpu

            cpu.SetCPUDataMemory(cpu, dados); // envia os dados para a cpu

            InterruptionManager interruptionManager = new InterruptionManager();

            interruptionManager.Execute(cpu);
        }

        public static void IlegalHandler(CPU cpu)
        {
            Status status = cpu.GetCPUStatus(cpu);

            string origem = cpu.GetPCInstruction(cpu); // retorna a instrucao atual do PC

            string[] instrucao = origem.Split(' ');
            string instruction = instrucao[0];

            //Console.WriteLine("Instruction on Ilegal: " + instruction);

            // pede ao SO para fazer a leitura de um dado (inteiro) do dispositivo de E/S n; o dado será colocado no acumulador
            if (instruction == "LE")
            {
                Read(cpu);
            }
            else // pede ao SO gravar o valor do acumulador no dispositivo de E/S n
            if (instruction == "GRAVA")
            {
                Write(cpu);
            }
            else // pede ao SO para terminar a execução do programa (como exit)
            if (instruction == "PARA")
            {
                Console.WriteLine("\nIlegal ended\n");
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

        private static void Read(CPU cpu)
        {
            Console.WriteLine("LE on SO");
            
            Status s = cpu.GetCPUStatus(cpu); // salva o estado da CPU
            
            s.InterruptionCode = sleeping; // coloca a CPU em estado dormindo
            cpu.UpdateCPUStatus(cpu, s);

            // realiza a operação 
            string fullPath = path + cpu.value.ToString() + ".txt";
            string content = File.ReadAllText(fullPath);
            s.A = Int32.Parse(content);

            Timer.NewInterruption('A', 5, ilegal); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna

            return;
        }

        private static void Write(CPU cpu)
        {
            Console.WriteLine("GRAVA on SO");
            
            Status s = cpu.GetCPUStatus(cpu); // salva o estado da CPU
            
            s.InterruptionCode = sleeping; // coloca a CPU em estado dormindo
            cpu.UpdateCPUStatus(cpu, s);

            string fullPath = path + cpu.value.ToString() + ".txt"; // realiza a operação 
            File.WriteAllText(fullPath, s.A.ToString());

            Timer.NewInterruption('A', 5, 1); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna
        }

        public static void TimerCallBack(CPU cpu)
        {
            Status s = cpu.GetCPUStatus(cpu);
            s.InterruptionCode = normal;
            cpu.UpdatePC(cpu);
            cpu.UpdateCPUStatus(cpu, s);
            return;
        }

    }
}
