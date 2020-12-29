using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class Schedule
    {
        public char type { get; set; } // tipo de agendamento de interrupcao
        public int date { get; set; } // quando
        public int period { get; set; } // tempo original
        public int interruptionCode { get; set; } // que interrupcao sera gerada
    }

    class Timer
    {
        private static List<Schedule> queue = new List<Schedule>();
        //private Queue<Schedule> queue = new Queue<Schedule>();

        public static int currentTime { get; set; }

        public static void Initialize()
        {
            currentTime = 0;
            queue.Clear();
        }

        // informar a passagem do tempo (ele simplesmente incrementa um contador interno)
        public static int UpdateTime()
        {
            currentTime++;
            Console.WriteLine("Timer current time: " + currentTime);
            return GetInterruption(); // checa se ele esta causando alguma interrupcao
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
            if(queue.Count == 0) { return 0; } // nao ha interrupcoes
            if(currentTime < queue[0].date) { return 0; } // ainda nao chegou a interrupcao
            else
            {
                Schedule schedule = new Schedule();
                schedule = queue[0]; // pega a interrupcao
                queue.RemoveAt(0); // remove da fila
                if(schedule.type == 'P')
                {
                    NewInterruption('P', schedule.period + currentTime, schedule.interruptionCode); // reinsere as periodicas
                }
                SO.TimerCallBack();
                return schedule.interruptionCode; // retorna o codigo da interrupcao
            }
        }

        // gerar novas interrupções – deve informar o tipo (periódica ou não), o período (tempo entre interrupções) ou tempo até a interrupção, e o código da interrupção que será gerada
        public static void NewInterruption(char type, int date, int interruptionCode)
        {
            Schedule newSchedule = new Schedule();
            newSchedule.type = type;
            newSchedule.period = date;
            newSchedule.date = currentTime + date;
            newSchedule.interruptionCode = interruptionCode;

            Console.WriteLine("\nCreating new interruption for time: " + newSchedule.date);

            int index = 0;
            if (queue.Count > 0)
            {
                while (queue[index].date < date)
                {
                    index++;
                }
            }
            
            queue.Insert(index, newSchedule); // insere ordenado
        }

    }

}
