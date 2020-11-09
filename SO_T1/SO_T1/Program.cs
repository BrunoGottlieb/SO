using System;
using System.IO;

namespace SO_T1
{
    class CPU
    {
        public int counter; // registrador | o contador de programa
        public int accumulator; // registrador | acumulador
        public bool status;// codigo de interrupcao
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = "C://teste/SO.txt";

            CPU cpu = new CPU();

            string[] programMemory = new string[10]; // memoria de programa
            int[] dataMemory = new int[10]; // memoria de dados

            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                programMemory = content.Split("\n");
            }
            else
            {
                Console.WriteLine("Path " + path + " was not found.");
            }

            for (int i = 0; i < programMemory.Length; i++)
            {
                Console.WriteLine(i + " : " + programMemory[i]);
            }
        }
    }
}
