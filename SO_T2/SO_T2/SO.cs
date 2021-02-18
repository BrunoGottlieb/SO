﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SO_T2
{
    class SO
    {
        static string path = "C://teste/ES/"; // diretorio

        public static string[] programMemory; // memoria de programa

        public static List<Page> secondaryMemory;

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
        private static int SOcalledCount = 0;
        private static int cpuIdleTime = 0;

        private static int ilegalCount = 0;
        private static int violationCount = 0;
        private static int timerCallBackCount = 0;
        private static int readCount = 0;
        private static int writeCount = 0;
        public static int jobChangeCount = 0;
        private static int changesByQuantumCount = 0;

        public static Queue<PageInfo> pagesFIFO = new Queue<PageInfo>();

        public static void Initialize()
        {
            CPU.InitializeCPU();

            JobManager.InitNextJobOnCPU(); // inicializa o Job na CPU

            InterruptionManager interruptionManager = new InterruptionManager();

            Memory.Init();

            secondaryMemory = new List<Page>();

            /// Timer.NewInterruption(JobManager.GetCurrentJob(), 'P', 50, ilegal); // interrupcao periodica do SO

            interruptionManager.Execute();
        }

        public static void IlegalHandler()
        {
            Console.WriteLine("\nIlegal handler\n");

            SOcalledCount++; // incrementa o numero de vezes que o SO foi chamado
            ilegalCount++; // atualiza o numero de vezes que foi chamado por instrucao ilegal

            Job b = JobManager.GetCurrentJob();
            b.UpdateTimeSpent(Timer.GetCurrentTime() - lastExecutionTime); // atualiza o tempo de execucao do job
            lastExecutionTime = Timer.GetCurrentTime(); // atualiza o tempo que o SO foi chamado pela ultima vez

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
                MMU.CleanJobFromMemory(JobManager.GetCurrentJob());
                JobManager.SetCurrentJobStatus(finished); // esse programa terminou de ser executado
            }
            else
            {
                Console.WriteLine("Memory Violation");
                ViolationHandler(); // violacao de memoria
            }

            if (instruction != "PARA") { CPU.UpdatePC(); }

            JobManager.InitNextJobOnCPU(); // inicilizar CPU com os dados de um job

        }

        public static void ViolationHandler()
        {
            violationCount++; // atualiza o numero de vezes que foi chamado por violacao
            Console.WriteLine("\nViolation ended");
            Environment.Exit(0);
        }

        private static void Read()
        {
            readCount++; // atualiza o numero de vezes que foi chamado por conta de leitura

            Status s = CPU.GetCPUStatus(); // salva o estado da CPU

            s.InterruptionCode = sleeping; // coloca a CPU em estado dormindo

            // realiza a operação 
            string fullPath = path + CPU.value.ToString() + ".txt";
            if (File.Exists(fullPath) == false)
            {
                Console.WriteLine("Error: " + fullPath + " does not exist");
                Environment.Exit(0);
            }
            string content = File.ReadAllText(fullPath);
            CPU.SetCPU_A(Int32.Parse(content)); // atualiza o acumulador com o valor do input

            Job j = JobManager.GetCurrentJob();
            j.UpdateJobCPUStatus(s); // salva o estado da CPU no job
            j.UpdateJobStatus(blocked); // bloqueia o job

            Timer.NewInterruption(j, 'A', j.read_delay, ilegal); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna

            return;
        }

        private static void Write()
        {
            writeCount++; // atualiza o numero de vezes que foi chamado por conta de escrita

            Status s = CPU.GetCPUStatus(); // salva o estado da CPU

            s.InterruptionCode = sleeping; // coloca a CPU em estado dormindo

            string fullPath = path + CPU.value.ToString() + ".txt";
            if (File.Exists(fullPath) == false)
            {
                Console.WriteLine("Error: " + fullPath + " does not exist");
                Environment.Exit(0);
            }
            File.WriteAllText(fullPath, s.A.ToString()); // realiza a operação 

            Job j = JobManager.GetCurrentJob();
            j.UpdateJobCPUStatus(s); // salva o estado da CPU no job
            j.UpdateJobStatus(blocked); // bloqueia o job

            Timer.NewInterruption(j, 'A', j.write_delay, ilegal); // programa o timer para gerar uma interrupção devido a esse dispositivo depois de um certo tempo e retorna

            return;
        }

        public static void TimerCallBack(Job j, int newStatus) // chamado apos uma interrupcao terminar
        {
            Console.WriteLine("\nTIMER CALLBACK\n");

            timerCallBackCount++; // incrementa o numero de vezes que foi chamado pelo timer

            SOcalledCount++; // incrementa o numero de vezes que o SO foi chamado

            if (CPU.GetCPUInterruptionCode() == sleeping) // CPU estava ociosa
            {
                cpuIdleTime += (Timer.GetCurrentTime() - lastExecutionTime); // atualiza o tempo com que a CPU ficou ociosa
            }
            else // havia um processo em execucao
            {
                Job b = JobManager.GetCurrentJob(); // atualiza o tempo desse job antes de trocar para outro
                b.UpdateTimeSpent(Timer.GetCurrentTime() - lastExecutionTime); // atualiza o tempo de execucao do job
                b.exceedTimeCount++; // atualiza o número de vezes que um processo perdeu a CPU por preempção
                changesByQuantumCount++; // atualiza o numero de vezes que o SO foi chamado por preempção
            }

            lastExecutionTime = Timer.GetCurrentTime(); // atualiza o tempo que o SO foi chamado pela ultima vez

            j.UpdateJobStatus(newStatus);

            //if(CPU.GetCPUInterruptionCode() != normal) { CPU.UpdatePC(); Console.WriteLine("Updating PC for " + JobManager.GetCurrentJob().programName); } // CPU normalizou, atualiza o valor de PC

            JobManager.InitNextJobOnCPU(); // chama outro processo para executar

            //CPU.UpdatePC();

            return;
        }

        private static void InitPage(int index) // inicializa uma nova pagina apos page default
        {
            int frame = index / Memory.pageSize; // numero do quadro para onde vai o dado
            Console.WriteLine("\nIndex no SO: " + index + "\n");

            Console.WriteLine("frame no SO: " + frame);

            Job currentJob = JobManager.GetCurrentJob(); // processo atual

            int physicalFrame = MMU.GetNextFreeFrame(); // descobre qual eh o proximo quadro livre da memoria fisica
            Console.WriteLine("physicalFrame: " + physicalFrame);

            Console.WriteLine("\nValue: " + CPU.value + "\n");

            if (physicalFrame >= 0) // ha quadro disponivel
            {
                Console.WriteLine("\nHa quadro disponivel na memoria fisica. Quadro: " + physicalFrame + "\n");
                MMU.SetMemoryFrameValidity(physicalFrame, false); // mapeia a pagina na memoria fisica

                if(currentJob.pagesTable[frame].isAtSecondary) // pagina ja existe e esta na memoria secundaria
                {
                    Console.WriteLine("\nQuadro " + frame + " ja estava na memoria\n");
                    int pos = currentJob.pagesTable[frame].posAtSecondary; // posicao da pagina na memoria secundaria

                    Console.WriteLine("\nConteudo desse quadro:");
                    foreach(int i in secondaryMemory[pos].content)
                    {
                        Console.WriteLine(i);
                    }

                    Memory.dataMemory[physicalFrame] = secondaryMemory[pos]; // retira da memoria secundaria e passa para a primaria
                    secondaryMemory.RemoveAt(pos); // remove da memoria secundaria
                }

                currentJob.pagesTable[frame].frameNum = physicalFrame; // posicao dessa pagina na memoria fisica
                Console.WriteLine("frameNum: " + currentJob.pagesTable[frame].frameNum);

                currentJob.pagesTable[frame].isValid = true; // marca a pagina como valida

                pagesFIFO.Enqueue(currentJob.pagesTable[frame]); // adiciona a lista de filas na memoria principal

                SetMMUPagesTable(); // envia a tabela de paginas do processo para a MMU
            }
            else
            {
                // FIFO

                Console.WriteLine("FIFO ########################################################################");

                foreach (PageInfo pageInfo in pagesFIFO)
                {
                    Console.WriteLine(pageInfo.frameNum);
                }

                PageInfo pageTarget = pagesFIFO.Dequeue(); // retira a pagina da fila
                Console.WriteLine("Target: " + pageTarget.frameNum); // informa o numero do quadro

                AddToSecondaryMemory(pageTarget); // passa a pagina para a memoria secundaria

                Console.WriteLine("\nPage now with: " + JobManager.GetCurrentJob().programName);
            }
        }

        // passa os quadros para a memoria secundaria e adiciona outra pagina no lugar
        private static void AddToSecondaryMemory(PageInfo pageInfo)
        {
            Page targetPage = Memory.dataMemory[pageInfo.frameNum]; // pagina que sera retirada da memoria principal
            secondaryMemory.Add(targetPage); // adiciona essa pagina a memoria secundaria

            Console.WriteLine("Secondary now:");

            foreach (Page p in secondaryMemory)
            {
                Console.WriteLine("\n");
                foreach (int i in p.content)
                {
                    Console.WriteLine(i);
                }
            }

            Job job = pageInfo.ownJob; // processo dono dessa pagina
            int frame = pageInfo.ownFrame; // posicao dessa pagina no vetor do processo

            job.pagesTable[frame].isAtSecondary = true;
            job.pagesTable[frame].isValid = false;
            job.pagesTable[frame].posAtSecondary = secondaryMemory.IndexOf(targetPage);

            pageInfo.ownJob.CleanPageTable(); // marca as paginas como invalidas

            Memory.dataMemory[pageInfo.frameNum] = new Page(); // libera a pagina na memoria principal

            Console.WriteLine("\nOwn: " + job.pagesTable[frame].ownJob.programName);
            Console.WriteLine("\nPos at secondary: " + job.pagesTable[frame].posAtSecondary);

            return;
        }

        // envia a tabela de paginas do processo para a MMU
        private static void SetMMUPagesTable()
        {
            Job currentJob = JobManager.GetCurrentJob(); // processo atual
            MMU.pagesTable = currentJob.pagesTable;
        }

        public static void PageFaultHandler()
        {
            Console.WriteLine("\nSO Page Fault Handler");
            Console.WriteLine("CPU Value: " + CPU.value);
            InitPage(CPU.value); // inicializa uma nova pagina
            Status s = CPU.GetCPUStatus(); // salva o estado da CPU
            s.InterruptionCode = normal; // coloca a CPU em estado dormindo
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
            Console.WriteLine("\n********************************************");
            Console.WriteLine("\n-- Program Stats --\n");
            foreach (Job j in JobManager.finishedJobs)
            {
                Console.WriteLine(" ");
                Console.WriteLine(j.programName); // nome do processo
                Console.WriteLine(" ");
                Console.WriteLine("Start time: " + j.launchDate); // hora de início
                Console.WriteLine("Finish time: " + j.finishDate); // hora de término
                Console.WriteLine("Life time: " + (j.finishDate - j.launchDate)); // Tempo de retorno
                Console.WriteLine("Time using CPU: " + j.timeSpent); // tempo de CPU
                Console.WriteLine("CPU usage: " + (((float)j.timeSpent / (float)Timer.currentTime) * 100).ToString("00.00") + "%"); // percentual de CPU utilizada durante sua vida
                Console.WriteLine("Time spent blocked: " + j.timeSpentBlocked); // Tempo que passou bloqueado
                Console.WriteLine("Block count: " + j.blockCount); // número de vezes que bloqueou
                Console.WriteLine("Called count: " + j.calledCount); // número de vezes que foi escalonado
                Console.WriteLine("Exceed quantum count: " + j.exceedTimeCount); // número de vezes que perdeu a CPU por preempção
            }

            Console.WriteLine("\n-- SO Stats --\n");

            Console.WriteLine("Total activity time: " + Timer.currentTime); // tempo em que o sistema esteve ativo
            Console.WriteLine("CPU idle time: " + cpuIdleTime); // tempo ocioso total da CPU
            Console.WriteLine("CPU time spent in idle: " + (((float)cpuIdleTime / (float)Timer.currentTime) * 100).ToString("00.00") + "%"); // tempo ocioso total da CPU
            Console.WriteLine("SO execution count: " + SOcalledCount); // quantas vezes o SO executou
            Console.WriteLine("Ilegal: " + ilegalCount); // quantas por cada tipo de interrupção
            Console.WriteLine("Violation: " + violationCount); //
            Console.WriteLine("Timer: " + timerCallBackCount); //
            Console.WriteLine("Read: " + readCount); //
            Console.WriteLine("Write: " + writeCount); //
            Console.WriteLine("Change execution count: " + jobChangeCount); // quantas trocas de processos houve
            Console.Write("Changes by quantum count: " + changesByQuantumCount); // quantas foram preempção

            Console.WriteLine("\n\n");
        }

    }
}
