using System;
using System.Collections.Generic;
using System.IO;

namespace SO_T1
{
    class CPU
    {
        public Status status = new Status(); // estado da CPU

        public string[] programMemory; // memoria de programa
        public int[] dataMemory; // memoria de dados
    }

    class Status
    {
        public int PC; // registrador | o contador de programa
        public int A; // registrador | acumulador
        public int interruptionCode;// codigo de interrupcao // 0 normal | 1 instrucao ilegal | 2 violacao de memoria
    }

    class Program
    {
        public const int normal = 0;
        public const int ilegal = 1;
        public const int violacao = 2;
        public const bool debugMode = true;

        static void Main(string[] args)
        {
            string path = "C://teste/SO.txt";
            int[] dados = new int[4];

            CPU cpu = new CPU();
            Status status = new Status();

            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                string[] array = content.Split("\n");
                cpu_altera_programa(cpu, array);
            }
            else
            {
                Console.WriteLine("Path " + path + " was not found.");
            }

            for (int i = 0; i < cpu.programMemory.Length; i++)
            {
                Console.WriteLine(i + " : " + cpu.programMemory[i]);
            }

            cpu_estado_inicializa(status); // inicializa o estado da cpu

            cpu_altera_estado(cpu, status); // altera o estado da cpu

            cpu_altera_dados(cpu, dados);

            while (cpu.status.interruptionCode == normal)
            {
                cpu_executa(cpu);
            }

            if(debugMode)
            {
                Console.WriteLine("----------");

                Console.WriteLine("CPU parou na instrução " + cpu.programMemory[cpu.status.PC] + " (deve ser PARA)");
                Console.WriteLine("O valor de m[0] é " + cpu.dataMemory[0] + " (deve ser 42)");
            }
        }

        // alterar o conteúdo da memória de programa (recebe um vetor de strings)
        static void cpu_altera_programa(CPU cpu, string[] newData)
        {
            cpu.programMemory = newData;
        }

        // alterar o conteúdo da memória de dados (recebe um vetor de inteiros, que é alterado pela execução das instruções)
        static void cpu_altera_dados(CPU cpu, int[] newData)
        {
            cpu.dataMemory = newData;
        }

        // obter o conteúdo da memória de dados (retorna um vetor de inteiros que é o conteúdo atual da memória – não precisa desta função caso o vetor passado pela função acima seja alterado “in loco”)
        static int[] cpu_salva_dados(CPU cpu)
        {
            return cpu.dataMemory;
        }

        // ler o modo de interrupção da CPU (normal ou um motivo de interrupção)
        static int cpu_interrupcao(CPU c)
        {
            return c.status.interruptionCode;
        }

        // colocar a CPU em modo normal (coresponde ao retorno de interrupção) – muda para modo normal e incrementa o PC; se já estiver em modo normal, não faz nada
        static int cpu_retorna_interrupcao(CPU cpu)
        {
            return cpu.status.interruptionCode;
        }

        // obter a instrução em PC (que pode ser inválida se PC estiver fora da memória de programa)
        static string cpu_instrucao(CPU cpu)
        {
            return cpu.programMemory[cpu.status.PC];
        }

        // obter o estado interno da CPU (retorna o valor de todos os registradores)
        static void cpu_salva_estado(CPU cpu, Status e)
        {
            e = cpu.status;
        }

        // alterar o estado interno da CPU (copia para os registradores da cpu os valores recebidos)
        static void cpu_altera_estado(CPU cpu, Status e)
        {
            cpu.status = e;
        }

        // inicializar o estado interno da CPU (PC=0, A=0, estado=normal)
        static void cpu_estado_inicializa(Status e)
        {
            e.PC = 0;
            e.A = 0;
            e.interruptionCode = normal; // normal
        }

        // executar uma instrução (só executa se estiver em modo normal)
        static void cpu_executa(CPU cpu)
        {
            if(cpu_retorna_interrupcao(cpu) == normal)
            {
                bool updatePC = true;

                int PC = cpu.status.PC; // pega o valor atual de PC
                string origem = cpu.programMemory[PC];

                string[] instrucao = origem.Split(' ');
                string instruction = instrucao[0];

                string valueTmp = origem.Remove(0, origem.IndexOf(' ') + 1);

                int value = 0;
                if (Char.IsDigit(valueTmp[0]))
                    value = Convert.ToInt32(valueTmp);

                if (debugMode)
                {
                    Console.WriteLine("instruction: " + instruction);
                    Console.WriteLine("Value: " + value);
                }

                if (instruction == "CARGI") // coloca o valor n no acumulador (A=n)
                {
                    cpu.status.A = value;
                    Console.WriteLine(cpu.status.A);
                }

                else if (instruction == "CARGM") // coloca no acumulador o valor na posição n da memória de dados (A=M[n])
                {
                    cpu.status.A = cpu.dataMemory[value];
                    Console.WriteLine(cpu.status.A);
                }

                else if (instruction == "CARGX") // coloca no acumulador o valor na posição que está na posição n da memória de dados (A=M[M[n]])
                {
                    cpu.status.A = cpu.dataMemory[cpu.dataMemory[value]];
                    Console.WriteLine(cpu.status.A);
                }

                else if (instruction == "ARMM") // coloca o valor do acumulador na posição n da memória de dados (M[n]=A)
                {
                    cpu.dataMemory[value] = cpu.status.A;
                }

                else if (instruction == "ARMX") // 	coloca o valor do acumulador posição que está na posição n da memória de dados (M[M[n]]=A)
                {
                    cpu.dataMemory[cpu.dataMemory[value]] = cpu.status.A;
                }

                else if (instruction == "SOMA") // 	soma ao acumulador o valor no endereço n da memória de dados (A=A+M[n])
                {
                    cpu.status.A = cpu.status.A + cpu.dataMemory[value];
                }

                else if (instruction == "NEG") // 	inverte o sinal do acumulador (A=-A)
                {
                    cpu.status.A *= -1;
                }

                else if (instruction == "DESVZ") // se A vale 0, coloca o valor n no PC
                {
                    if (cpu.status.A == 0)
                    {
                        cpu.status.PC = value;
                        updatePC = false;
                    }
                }

                else // coloca a CPU em interrupção – instrução ilegal
                {
                    cpu.status.interruptionCode = ilegal;
                    updatePC = false;
                }

                if (updatePC) { cpu.status.PC++; }

            }
        }

    }
}
