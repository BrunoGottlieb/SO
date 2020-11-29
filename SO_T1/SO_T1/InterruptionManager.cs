using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class InterruptionManager
    {
        public const int normal = 0;
        public const int ilegal = 1;
        public const int violacao = 2;
        public static void Execution(CPU cpu)
        {
            while (true) // laco que mantem o programa em execucao
            {
                int cpu_state = API.GetCPUInterruptionCode(cpu);

                if (cpu_state == normal)
                {
                    SO.Execute(cpu);
                }
                else if (cpu_state == ilegal)
                {
                    SO.IlegalHandler(cpu);
                }
                else if (cpu_state == violacao)
                {
                    SO.ViolationHandler();
                }

                Timer.UpdateTime();
            }
        }
    }
}
