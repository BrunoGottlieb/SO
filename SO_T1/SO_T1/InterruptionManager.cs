using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class InterruptionManager // Controlador
    {
        public const int normal = 0;
        public const int ilegal = 1;
        public const int violacao = 2;
        public const int sleeping = 3;

        public void Execute()
        {
            Timer.Initialize();

            while (true) // laco que mantem o programa em execucao
            {
                int interruptionCode = CPU.GetCPUInterruptionCode();

                if (interruptionCode == normal)
                {
                    CPU.ExecuteCPU();
                    //cpu.UpdatePC(cpu); /// retirar depois
                }
                else if (interruptionCode == ilegal)
                {
                    SO.IlegalHandler();
                }
                else if (interruptionCode == violacao)
                {
                    SO.ViolationHandler();
                }
                else if (interruptionCode == sleeping)
                {
                    // nothing
                }

                // ficar chamando esse metodo enquanto houver interrupcao
                do
                {
                    interruptionCode = Timer.UpdateTime();
                }
                while (interruptionCode != normal);

            }
        }
    }
}
