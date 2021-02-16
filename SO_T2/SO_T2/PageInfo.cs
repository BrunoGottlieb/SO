using System;
using System.Collections.Generic;
using System.Text;

namespace SO_T2
{
    class PageInfo
    {
        public int frameNum { get; set; } // o número do quadro correspondente à página
        public bool isValid { get; set; } // uma indicação de validade (o descritor é válido ou não)
        public bool isChangeable { get; set; } // indicador se a página é alterável ou não
        public bool wasAccessed { get; set; } // indicador se a página foi acessada ou não
        public bool wasChanged { get; set; } // indicador se a página foi alterada ou não

        public PageInfo()
        {
            frameNum = -1;
            isValid = false;
            isChangeable = true;
            wasAccessed = false;
            wasChanged = false;
        }
    }
}
