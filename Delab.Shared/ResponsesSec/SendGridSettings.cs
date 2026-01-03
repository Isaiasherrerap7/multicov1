using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Shared.ResponsesSec;

public class SendGridSettings
{
    public string? SendGridApiKey { get; set; }
    public string? SendGridFrom { get; set; }
    public string? SendGridNombre { get; set; }
}