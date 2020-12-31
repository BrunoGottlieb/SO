using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class JobManager // Escalonador
    {
        public static List<Job> jobs = new List<Job>();

        private static int cpuIdleTime = 0;

        public static void SetJobList(List<Job> j) // enche o gerenciador com a lista de jobs
        {
            jobs = j;
        }

        public static bool InitNextJobOnCPU() // inicilizar CPU com os dados de um job
        {
            if (jobs.Count == 0) // nao ha mais nenhum job na lista
            {
                SO.FinishExecution();
            }

            foreach (Job j in jobs)
            {
                if (j.IsReady()) 
                {
                    if (!j.isInitialized) { j.Init(); } // inicializa o job
                    j.PutJobOnCPU(); // colocar os dados do job na CPU
                    return true; // sucesso
                }
            }

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

        public static void UpdateCPUIdleTime(int time)
        {
            cpuIdleTime += time;
        }

    }
}
