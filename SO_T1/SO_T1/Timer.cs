using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class Schedule
    {
        public char type { get; set; } // tipo de agendamento de interrupcao
        public int date { get; set; } // quando
        public int interruptionCode { get; set; } // que interrupcao sera gerada
    }

    class Timer
    {
        private List<Schedule> queue = new List<Schedule>();
        //private Queue<Schedule> queue = new Queue<Schedule>();

        public int currentTime { get; set; }

        public void Initialize()
        {
            currentTime = 0;
            queue.Clear();
        }

        // informar a passagem do tempo (ele simplesmente incrementa um contador interno)
        public int UpdateTime()
        {
            currentTime++;
            return GetInterruption(); // checa se ele esta causando alguma interrupcao
        }

        // ler o tempo atual (retorna o valor atual do contador)
        public int GetCurrentTime()
        {
            return currentTime;
        }

        // verificar se tem uma interrupção pendente - ele retorna o código da interrupção ou um código para dizer que não tem nenhuma interrupção.
        // Essa função pode ser chamada diversas vezes, para se saber se tem várias interrupções no mesmo tempo – o timer “esquece” cada interrupção que ele retorna
        public int GetInterruption()
        {
            if(queue.Count == 0) { return 0; }
            if(currentTime < queue[0].date) { return 0; } // caso ainda nao haja interrupcoes
            else
            {
                Schedule schedule = new Schedule();
                schedule = queue[0]; // pega a interrupcao
                queue.RemoveAt(0); // remove da fila
                if(schedule.type == 'P')
                {
                    NewInterruption('P', schedule.date + currentTime, schedule.interruptionCode); // reinsere as periodicas
                }
                return schedule.interruptionCode; // retorna o codigo da interrupcao
            }
        }

        // gerar novas interrupções – deve informar o tipo (periódica ou não), o período (tempo entre interrupções) ou tempo até a interrupção, e o código da interrupção que será gerada
        public void NewInterruption(char type, int date, int interruptionCode)
        {
            Schedule newSchedule = new Schedule();
            newSchedule.type = type;
            newSchedule.date = date;
            newSchedule.interruptionCode = interruptionCode;

            int index = 0;
            while (queue[index].date < date)
            {
                index++;
            }
            queue.Insert(index, newSchedule); // insere ordenado
        }

    }

}
