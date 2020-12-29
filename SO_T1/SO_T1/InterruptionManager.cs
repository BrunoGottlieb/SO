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
        public const int sleeping = 3;

        public void Execute(CPU cpu)
        {
            //Timer timer = new Timer();
            //timer.Initialize();

            Timer.Initialize();

            while (true) // laco que mantem o programa em execucao
            {
                Console.WriteLine("Managing");

                Console.WriteLine("PC value: " + cpu.status.PC);

                int interruptionCode = cpu.GetCPUInterruptionCode(cpu);

                if (interruptionCode == normal)
                {
                    cpu.ExecuteCPU(cpu);
                }
                else if (interruptionCode == ilegal)
                {
                    SO.IlegalHandler(cpu);
                }
                else if (interruptionCode == violacao)
                {
                    SO.ViolationHandler(cpu);
                }
                else if (interruptionCode == sleeping)
                {
                    // nothing
                }

                // ficar chamando esse metodo enquanto houver interrupcao
                do
                {
                    //interruptionCode = timer.UpdateTime();
                    interruptionCode = Timer.UpdateTime();
                }
                while (interruptionCode != normal);
            }
        }
    }
}
