using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Shared.ResponsesSec;

public class ContactViewDTO
{
    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Mensaje { get; set; } = null!;
}