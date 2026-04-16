using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontek_Full_Web_E_Commerce.Application.Interfaces
{
    public interface ICryptoService
    {
        string Encrypt(string Texto);
        string Decrypt(string TextoCifrado);
    }
}
