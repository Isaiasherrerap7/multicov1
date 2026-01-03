using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Helpers;

// Clave aleatoria para usuarios nuevos
public class UtilityTools : IUtilityTools
{
    public string GeneratePass(int longitud, string caracteres)
    {
        StringBuilder res = new();
        Random rnd = new();
        while (0 < longitud--)
        {
            res.Append(caracteres[rnd.Next(caracteres.Length)]);
        }
        return res.ToString();
    }
}