using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SO_T1
{
    class SO
    {
        static string path = "C://teste/ES/"; // diretorio

        // Constantes da CPU
        private const int normal = 0;
        private const int ilegal = 1;
        private const int violacao = 2;
        private const int sleeping = 3;

        // Constantes de Job
        private const int ready = 0;
        private const int blocked = 1;
        private const int finished = 2;

        private static int lastExecutionTime = 0;

        public static void Initialize()
        {
            CPU.InitializeCPU();

            JobManager.InitNextJobOnCPU(); // inicializa o Job na CPU

            InterruptionManager interruptionManager = new InterruptionManager();

            /// Timer.NewInterruption(JobManager.GetCurrentJob(), 'P', 50, ilegal); // interrupcao periodica do SO

            interruptionManager.Execute();
        }

        public static void IlegalHandler()
        {
            Console.WriteLine("\nIlegal handler\n");

            Job b = JobManager.GetCurrentJob();
            b.UpdateTimeSpent(Timer.GetCurrentTime() - lastExecutionTime); // atualiza o tempo de execucao do job
            lastExecutionTime = Timer.GetCurrentTime(); // atualiza o tempo que o SO foi chamado pela ultima vez

            Status status = CPU.GetCPUStatus();

            string origem = CPU.GetPCInstruction(); // retorna a instrucao atual do PC

            string[] instrucao = origem.Split(' ');
            string instruction = instrucao[0];

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
                JobManager.SetCurrentJobStatus(finished); // esse programa terminou de ser executado
            }
            else
            {
                Console.WriteLine("Memory Violation");
                ViolationHandler(); // violacao de memoria
            }

            JobManager.InitNextJobOnCPU(); // inicilizar CPU com os dados de um job

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

            // realiza a operação 
            string fullPath = path + CPU.value.ToString() + ".txt";
            string content = File.ReadAllText(fullPath);
            CPU.SetCPU_A(Int32.Parse(content)); // atualiza o acumulador com o valor do input
            //s.A = Int32.Parse(content);

            Job j = JobManager.GetCurrentJob();
            j.UpdateJobCPUStatus(s); // salva o estado da CPU no job
            j.UpdateJobStatus(blocked); // bloqueia o job

            Timer.NewInterruption(j, 'A', j.read_delay, ilegal); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna

            return;
        }

        private static void Write()
        {
            Console.WriteLine("GRAVA on SO");
            
            Status s = CPU.GetCPUStatus(); // salva o estado da CPU
            
            s.InterruptionCode = sleeping; // coloca a CPU em estado dormindo

            string fullPath = path + CPU.value.ToString() + ".txt"; // realiza a operação 
            File.WriteAllText(fullPath, s.A.ToString());

            Job j = JobManager.GetCurrentJob();
            j.UpdateJobCPUStatus(s); // salva o estado da CPU no job
            j.UpdateJobStatus(blocked); // bloqueia o job

            Timer.NewInterruption(j, 'A', j.write_delay, ilegal); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna

            return;
        }

        public static void TimerCallBack(Job j, int newStatus) // chamado apos uma interrupcao terminar
        {
            Console.WriteLine("\nTIMER CALLBACK\n");

            if (CPU.GetCPUInterruptionCode() == sleeping) // CPU estava ociosa
            { 
                JobManager.UpdateCPUIdleTime(Timer.GetCurrentTime() - lastExecutionTime); // atualiza o tempo com que a CPU ficou ociosa
            }
            else // havia um processo em execucao
            {
                Job b = JobManager.GetCurrentJob(); // atualiza o tempo desse job antes de trocar para outro
                b.UpdateTimeSpent(Timer.GetCurrentTime() - lastExecutionTime); // atualiza o tempo de execucao do job
                b.exceedTimeCount++; // atualiza o número de vezes que perdeu a CPU por preempção
            }

            lastExecutionTime = Timer.GetCurrentTime(); // atualiza o tempo que o SO foi chamado pela ultima vez

            j.UpdateJobStatus(newStatus);

            if(CPU.GetCPUInterruptionCode() != 0) { CPU.UpdatePC(); } // CPU normalizou, atualiza o valor de PC

            JobManager.InitNextJobOnCPU(); // chama outro processo para executar
            return;
        }

        public static void FinishExecution()
        {
            Console.WriteLine("\n\nNo more jobs to be executed");
            Console.WriteLine("Finishing with success");
            ShowStats(); // Exibe as estatisticas de execucao
            Environment.Exit(0);
        }

        private static void ShowStats() // Exibe as estatisticas de execucao
        {
            Console.WriteLine("\n********************************************\n");
            Console.WriteLine("\n-- Program Stats --\n");
            foreach (Job j in JobManager.finishedJobs)
            {
                Console.WriteLine(" ");
                Console.WriteLine(j.programName); // nome do processo
                Console.WriteLine(" ");
                Console.WriteLine("Start time: " + j.launchDate); // hora de início
                Console.WriteLine("Finish time: " + j.finishDate); // hora de término
                // Tempo de retorno
                Console.WriteLine("Time using CPU: " + j.timeSpent); // tempo de CPU
                Console.WriteLine("CPU usage: " + ((float)j.timeSpent / (float)Timer.currentTime).ToString("0.00") + "%"); // percentual de CPU utilizada durante sua vida
                Console.WriteLine("Time spent blocked: " + j.timeSpentBlocked); // Tempo que passou bloqueado
                Console.WriteLine("Block count: " + j.blockCount); // número de vezes que bloqueou
                Console.WriteLine("Called count: " + j.calledCount); // número de vezes que foi escalonado
                Console.WriteLine("Exceed quantum count: " + j.exceedTimeCount); // número de vezes que perdeu a CPU por preempção
            }

            Console.WriteLine("\n-- SO Stats --\n");

            Console.WriteLine("Total activity time: " + Timer.currentTime); // tempo em que o sistema esteve ativo
            // tempo ocioso total da CPU
            // quantas vezes o SO executou
            // quantas por cada tipo de interrupção
            // quantas trocas de processos houve
            // quantas foram preempção
        }

    }
}
