using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class Timer
    {
        private static int currentTime = 0;

        public static void Initialize()
        {
            currentTime = 0;
        }

        // informar a passagem do tempo (ele simplesmente incrementa um contador interno)
        public static void UpdateTime()
        {
            currentTime++;
        }

        // ler o tempo atual (retorna o valor atual do contador)
        public static int GetCurrentTime()
        {
            return currentTime;
        }

        // verificar se tem uma interrupção pendente - ele retorna o código da interrupção ou um código para dizer que não tem nenhuma interrupção.
        // Essa função pode ser chamada diversas vezes, para se saber se tem várias interrupções no mesmo tempo – o timer “esquece” cada interrupção que ele retorna
        public static int GetInterruption()
        {
            return 0;
        }

        // pedir novas interrupções – deve informar o tipo (periódica ou não), o período (tempo entre interrupções) ou tempo até a interrupção, e o código da interrupção que será gerada
        public static int NewInterruption()
        {
            return 0;
        }

    }
}
