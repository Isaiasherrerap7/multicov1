using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Helpers;

public interface IUtilityTools
{
    //Sistema para Generacion automatica de Clave
    //Se pasa longitud de la clave y caracteres con la que puede hacer la clave
    string GeneratePass(int longitud, string caracteres);
}