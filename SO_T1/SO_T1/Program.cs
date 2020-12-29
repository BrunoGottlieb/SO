using System;
using System.Collections.Generic;
using System.IO;

namespace SO_T1
{
    class Program
    {
        public const bool debugMode = true;

        static void Main(string[] args)
        {
            CPU cpu = new CPU(); // instancia da CPU

            Job job1 = new Job();
            job1.program = "1";
            job1.input_path = "C://teste/SS.txt";
            job1.output_path = "C://teste/ES/";
            job1.launchDate = 0;
            job1.memory = 100;
            job1.priority = 1;

            Job job2 = new Job();
            job2.program = "2";
            job2.input_path = "C://teste/SO.txt";
            job2.output_path = "C://teste/ES/";
            job2.launchDate = 0;
            job2.memory = 100;
            job2.priority = 1;

            List<Job> jobs = new List<Job>();
            jobs.Add(job1);
            jobs.Add(job2);

            SO.SetJobList(jobs);
            SO.JobManager();

            //SO.Initialize(cpu, status, dados);
        }
    }
}
