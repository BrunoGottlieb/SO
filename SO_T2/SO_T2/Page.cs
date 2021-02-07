using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T2
{
    class Page
    {
        int frameNum; // o número do quadro correspondente à página
        bool isValid; // uma indicação de validade (o descritor é válido ou não)
        bool isChangeable; // indicador se a página é alterável ou não
        bool wasAccessed; // indicador se a página foi acessada ou não
        bool wasChanged; // indicador se a página foi alterada ou não
    }
}
