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

        public static List<Job> jobs = new List<Job>();

        public static void SetJobList(List<Job> j) // enche o SO com a lista de jobs
        {
            jobs = j;
        }

        public static void JobManager() // gerencia os jobs a serem executados
        {
            if(jobs.Count == 0) // nao ha mais nenhum job na lista
            {
                Console.WriteLine("No more jobs to be executed");
                Console.WriteLine("Finishing with success");
                Environment.Exit(0);
            }

            Job targetJob = jobs[0];
            jobs.RemoveAt(0);

            Initialize(targetJob); // inicializa o proximo job da lista
        }

        public static void Initialize(Job j)
        {
            Console.WriteLine("-------- Initializing job: " + j.program + "--------");

            Status status = new Status(); // instancia dos status do CPU

            CPU.InitializeCPU(status); // inicializa o estado da cpu

            CPU.UpdateCPUStatus(status); // altera o estado da cpu

            CPU.SetCPUDataMemory(new int[j.memory]) ; // envia os dados para a cpu

            if (File.Exists(j.input_path)) // confere se o diretorio existe e le o seu conteudo
            {
                string content = File.ReadAllText(j.input_path);
                string[] array = content.Split("\n");
                CPU.SetCPUProgramMemory(array); // manda as instrucoes para a cpu
            }
            else
            {
                Console.WriteLine("Path " + j.input_path + " was not found.");
            }

            InterruptionManager interruptionManager = new InterruptionManager();

            interruptionManager.Execute();
        }

        public static void IlegalHandler()
        {
            Status status = CPU.GetCPUStatus();

            string origem = CPU.GetPCInstruction(); // retorna a instrucao atual do PC

            string[] instrucao = origem.Split(' ');
            string instruction = instrucao[0];

            //Console.WriteLine("Instruction on Ilegal: " + instruction);

            // pede ao SO para fazer a leitura de um dado (inteiro) do dispositivo de E/S n; o dado será colocado no acumulador
            if (instruction == "LE")
            {
                Read();
            }
            else // pede ao SO gravar o valor do acumulador no dispositivo de E/S n
            if (instruction == "GRAVA")
            {
                Write();
            }
            else // pede ao SO para terminar a execução do programa (como exit)
            if (instruction == "PARA")
            {
                Console.WriteLine("\nIlegal ended\n");
                JobManager(); // executa o proximo job da lista, se nao houver nenhum, finaliza
                //Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Memory Violation");
                ViolationHandler(); // violacao de memoria
            }
        }

        public static void ViolationHandler()
        {
            Console.WriteLine("\nViolation ended");
            Environment.Exit(0);
        }

        private static void Read()
        {
            Console.WriteLine("LE on SO");
            
            Status s = CPU.GetCPUStatus(); // salva o estado da CPU
            
            s.InterruptionCode = sleeping; // coloca a CPU em estado dormindo
            CPU.UpdateCPUStatus(s);

            // realiza a operação 
            string fullPath = path + CPU.value.ToString() + ".txt";
            string content = File.ReadAllText(fullPath);
            s.A = Int32.Parse(content);

            Timer.NewInterruption('A', 5, ilegal); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna

            return;
        }

        private static void Write()
        {
            Console.WriteLine("GRAVA on SO");
            
            Status s = CPU.GetCPUStatus(); // salva o estado da CPU
            
            s.InterruptionCode = sleeping; // coloca a CPU em estado dormindo
            CPU.UpdateCPUStatus(s);

            string fullPath = path + CPU.value.ToString() + ".txt"; // realiza a operação 
            File.WriteAllText(fullPath, s.A.ToString());

            Timer.NewInterruption('A', 5, 1); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna
        }

        public static void TimerCallBack()
        {
            Status s = CPU.GetCPUStatus();
            s.InterruptionCode = normal;
            CPU.UpdatePC();
            CPU.UpdateCPUStatus(s);
            return;
        }

    }
}
