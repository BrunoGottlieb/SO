using System;
using System.Collections.Generic;
using System.IO;

namespace SO_T1
{
    class Program
    {
        static void Main(string[] args)
        {
            CPU cpu = new CPU(); // instancia da CPU

            Job job1 = new Job();
            job1.programName = "Programa 1"; // opcional, qualquer valor
            job1.input_path = "C://teste/SS.txt";
            job1.output_path = "C://teste/ES/";
            job1.read_delay = 5;
            job1.write_delay = 30;
            job1.memory = 100; // quantidade de memoria necessaria;
            job1.quantum = 20;

            Job job2 = new Job();
            job2.programName = "Programa 2";
            job2.input_path = "C://teste/SO.txt";
            job2.output_path = "C://teste/ES/";
            job2.read_delay = 15;
            job2.write_delay = 25;
            job2.memory = 100;
            job2.quantum = 20;

            Job job3 = new Job();
            job3.programName = "Programa 3"; // opcional, qualquer valor
            job3.input_path = "C://teste/SS.txt";
            job3.output_path = "C://teste/ES/";
            job3.read_delay = 10;
            job3.write_delay = 10;
            job3.memory = 100; // quantidade de memoria necessaria;
            job3.quantum = 20;

            List<Job> jobs = new List<Job>();
            jobs.Add(job1);
            jobs.Add(job2);
            jobs.Add(job3);

            JobManager.SetJobList(jobs); // carrega os jobs para o gerenciador de jobs

            SO.Initialize();
        }
    }
}
