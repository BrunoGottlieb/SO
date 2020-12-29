using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class Job
    {
        public string program; // programa que será executado pelo job
        public int memory; // quantidade de memória necessária;
        public string input_path; // dispositivos de entrada(onde estão os dados e tempo de acesso a cada dado);
        public string output_path; // arquivos de saída(onde serão colocados os dados e tempo de gravação de cada dado);
        public int launchDate; // data de lançamento (um inteiro, de acordo com o timer);
        public int priority; // prioridade (a ser usado de acordo com o escalonador);
    }
}
