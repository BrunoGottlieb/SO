using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class API
    {
        public const int normal = 0;
        public const int ilegal = 1;
        public const int violacao = 2;

        // altera o valor do acumulador
        void SetCPU_A(Status e, int newValue)
        {
            e.A = newValue;
        }
        // retorna o valor do acumulador
        public static int GetCPU_A(Status e)
        {
            return e.A;
        }

        // alterar o conteúdo da memória de programa (recebe um vetor de strings)
        public static void SetCPUProgramMemory(CPU cpu, string[] newData)
        {
            cpu.programMemory = newData;
        }

        // alterar o conteúdo da memória de dados (recebe um vetor de inteiros, que é alterado pela execução das instruções)
        public static void SetCPUDataMemory(CPU cpu, int[] newData)
        {
            cpu.dataMemory = newData;
        }

        // obter o conteúdo da memória de dados (retorna um vetor de inteiros que é o conteúdo atual da memória – não precisa desta função caso o vetor passado pela função acima seja alterado “in loco”)
        public static int[] GetCPUDataMemory(CPU cpu)
        {
            return cpu.dataMemory;
        }

        // ler o modo de interrupção da CPU (normal ou um motivo de interrupção)
        public static int GetCPUInterruptionCode(CPU c)
        {
            return c.status.InterruptionCode;
        }

        // colocar a CPU em modo normal (coresponde ao retorno de interrupção) – muda para modo normal e incrementa o PC; se já estiver em modo normal, não faz nada
        public static int GetCPUInterruptionMode(CPU cpu)
        {
            return cpu.status.InterruptionCode;
        }

        // obter a instrução em PC (que pode ser inválida se PC estiver fora da memória de programa)
        public static string GetPCInstruction(CPU cpu)
        {
            return cpu.programMemory[cpu.status.PC];
        }

        // obter o estado interno da CPU (retorna o valor de todos os registradores)
        public static Status GetCPUStatus(CPU cpu)
        {
            return cpu.status;
        }

        // alterar o estado interno da CPU (copia para os registradores da cpu os valores recebidos)
        public static void UpdateCPUStatus(CPU cpu, Status e)
        {
            cpu.status = e;
        }

        // inicializar o estado interno da CPU (PC=0, A=0, estado=normal)
        public static void InitializeCPU(Status e)
        {
            e.PC = 0;
            e.A = 0;
            e.InterruptionCode = normal; // normal
        }

        public static int GetMemoryDataSize(CPU cpu)
        {
            return cpu.dataMemory.Length;
        }

        // executar uma instrução (só executa se estiver em modo normal)
        public static void ExecuteCPU(CPU cpu)
        {
            if (GetCPUInterruptionMode(cpu) == normal)
            {
                bool updatePC = true; // controla se o PC devera ser atualizado ao terminar a execucao da instrucao

                Status status = GetCPUStatus(cpu);

                string origem = GetPCInstruction(cpu); // retorna a instrucao atual do PC

                string[] instrucao = origem.Split(' ');
                string instruction = instrucao[0];

                string valueTmp = origem.Remove(0, origem.IndexOf(' ') + 1);

                int value = 0;
                if (Char.IsDigit(valueTmp[0]))
                    value = Convert.ToInt32(valueTmp);

                /*if (debugMode)
                {
                    Console.WriteLine("instruction: " + instruction);
                    Console.WriteLine("Value: " + value);
                }*/

                if (instruction == "CARGI") // coloca o valor n no acumulador (A=n)
                {
                    status.A = value;
                }

                else if (instruction == "CARGM") // coloca no acumulador o valor na posição n da memória de dados (A=M[n])
                {
                    int[] data = GetCPUDataMemory(cpu);
                    if(value < GetMemoryDataSize(cpu))
                    {
                        status.A = data[value];
                    } else
                    {
                        status.InterruptionCode = violacao;
                    }
                }

                else if (instruction == "CARGX") // coloca no acumulador o valor na posição que está na posição n da memória de dados (A=M[M[n]])
                {
                    int[] data = GetCPUDataMemory(cpu);
                    int pos = 0;
                    if (value < GetMemoryDataSize(cpu))
                    {
                        pos = data[value];
                        status.A = pos;
                    }
                    else
                    {
                        status.InterruptionCode = violacao;
                    }
                }

                else if (instruction == "ARMM") // coloca o valor do acumulador na posição n da memória de dados (M[n]=A)
                {
                    int[] data = GetCPUDataMemory(cpu);
                    if (value < GetMemoryDataSize(cpu))
                    {
                        data[value] = GetCPU_A(status);
                        SetCPUDataMemory(cpu, data);
                    }
                    else
                    {
                        status.InterruptionCode = violacao;
                    }
                    //cpu.dataMemory[value] = cpu.status.A;
                }

                else if (instruction == "ARMX") // 	coloca o valor do acumulador posição que está na posição n da memória de dados (M[M[n]]=A)
                {
                    int[] data = GetCPUDataMemory(cpu);
                    if (value < GetMemoryDataSize(cpu))
                    {
                        int pos = data[value];
                        data[pos] = GetCPU_A(status);
                        SetCPUDataMemory(cpu, data);
                    }
                    else
                    {
                        status.InterruptionCode = violacao;
                    }                    
                    //cpu.dataMemory[cpu.dataMemory[value]] = cpu.status.A;
                }

                else if (instruction == "SOMA") // 	soma ao acumulador o valor no endereço n da memória de dados (A=A+M[n])
                {
                    if (value < GetMemoryDataSize(cpu))
                    {
                        status.A = status.A + cpu.dataMemory[value];
                    }
                    else
                    {
                        status.InterruptionCode = violacao;
                    }
                }

                else if (instruction == "NEG") // 	inverte o sinal do acumulador (A=-A)
                {
                    status.A *= -1;
                }

                else if (instruction == "DESVZ") // se A vale 0, coloca o valor n no PC
                {
                    if (status.A == 0)
                    {
                        status.PC = value;
                        updatePC = false;
                    }
                }

                else // coloca a CPU em interrupção – instrução ilegal
                {
                    status.InterruptionCode = ilegal;
                    updatePC = false;
                }

                UpdateCPUStatus(cpu, status); // atualiza o estado da CPU com os novos dados

                if (updatePC) { cpu.status.PC++; }

            }
        }
    }
}
