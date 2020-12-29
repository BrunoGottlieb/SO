using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T1
{
    class Job
    {
        //xxxxx // programa que será executado pelo job
        int memory; // quantidade de memória necessária;
        string e_path; // dispositivos de entrada(onde estão os dados e tempo de acesso a cada dado);
        string s_path; // arquivos de saída(onde serão colocados os dados e tempo de gravação de cada dado);
        int launchDate; // data de lançamento (um inteiro, de acordo com o timer);
        int priority; // prioridade (a ser usado de acordo com o escalonador);
    }
}
