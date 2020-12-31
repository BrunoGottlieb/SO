using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class JobManager
    {
        public static List<Job> jobs = new List<Job>();

        public static void SetJobList(List<Job> j) // enche o gerenciador com a lista de jobs
        {
            jobs = j;
        }

        public static bool InitNextJobOnCPU() // inicilizar CPU com os dados de um job
        {
            Console.WriteLine("InitNextJobOnCPU");

            if (jobs.Count == 0) // nao ha mais nenhum job na lista
            {
                SO.FinishExecution();
            }

            foreach (Job j in jobs)
            {
                if (j.IsReady()) 
                {
                    Console.WriteLine(j.programName + " is ready");
                    if (!j.isInitialized) { j.Init(); } // inicializa o job
                    j.PutJobOnCPU(); // colocar os dados do job na CPU
                    return true; // sucesso
                }
            }
            Console.WriteLine("Nobody is ready");
            return false;
        }

        public static Job GetCurrentJob()
        {
            if (jobs.Count == 0) // nao ha mais nenhum job na lista
            {
                SO.FinishExecution();
                return null;
            }

            foreach(Job j in jobs)
            {
                if(j.IsReady()) { return j; }
            }

            return null;

            //Job targetJob = jobs[0];
            //jobs.RemoveAt(0);

            //Initialize(targetJob); // inicializa o proximo job da lista
        }

        public static void RemoveJob(Job j)
        {
            jobs.Remove(j);
        }

        public static void SetCurrentJobStatus(int state) // atualiza o estado do job atual
        {
            Job j = GetCurrentJob();
            j.UpdateJobStatus(state);
        }

    }
}
