using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SO_T1
{
    class Job
    {
        public const int ready = 0;
        public const int blocked = 1;
        public const int finished = 2;

        // Editavel pelo usuario
        public string programName; // programa que será executado pelo job
        public int memory; // quantidade de memória necessária;
        public string input_path; // dispositivos de entrada(onde estão os dados);
        public int read_delay; // tempo de acesso a cada dado de leitura
        public string output_path; // arquivos de saída(onde serão colocados os dados e tempo de gravação de cada dado);
        public int write_delay; // tempo de acesso a cada dado de escrita

        // Sistema
        public bool isInitialized = false;
        public int jobStatus = ready; // status do programa
        public int launchDate; // data de lançamento (um inteiro, de acordo com o timer);
        public int priority; // prioridade (a ser usado de acordo com o escalonador);
        public int timeExpent; // tempo que a CPU gastou executando esse processo
        // CPU
        public Status cpu_status; // status da cpu
        public string[] programMemory; // memoria de programa
        public int[] dataMemory; // memoria de dados

        public bool IsReady()
        {
            return jobStatus == 0 ? true : false;
        }

        public void Init()
        {
            if (File.Exists(input_path)) // confere se o diretorio existe e le o seu conteudo
            {
                string content = File.ReadAllText(input_path);
                string[] array = content.Split("\n");
                programMemory = array; // manda as instrucoes para a cpu
            }
            else
            {
                Console.WriteLine("Path " + input_path + " was not found.");
            }

            jobStatus = ready;
            timeExpent = 0;
            launchDate = Timer.GetCurrentTime();
            cpu_status = new Status();
            dataMemory = new int[memory];
            isInitialized = true;

            Console.WriteLine("\n\n-------- Initializing job: " + programName + " at time: " + launchDate + "--------\n\n");
            //priority = x
        }

        public void UpdateTimeExpent(int time)
        {
            timeExpent += time;
        }

        public void UpdateJobCPUStatus(Status e) // salvar os dados do Job

        {
            cpu_status = e;
            programMemory = CPU.programMemory;
            dataMemory = CPU.dataMemory;
        }

        public void PutJobOnCPU() // colocar os dados do job na CPU
        {
            cpu_status.InterruptionCode = 0; // set to normal again
            CPU.UpdateCPUStatus(cpu_status); // altera o estado da cpu
            CPU.SetCPUDataMemory(dataMemory); // envia os dados para a cpu
            CPU.SetCPUProgramMemory(programMemory); // envia os dados para a cpu

            Console.WriteLine("\n\n-------- Continuing job: " + programName + "--------\n\n");
        }

        public void UpdateJobStatus(int state)
        {
            jobStatus = state;
            if(jobStatus == finished) { JobManager.RemoveJob(this); } // remove o jog finalizado da lista
        }
    }
}
