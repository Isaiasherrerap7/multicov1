using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Helpers;

public interface IFileStorage
{
    //Para Guardado en Disco
    Task<string> UploadImage(IFormFile imageFile, string ruta, string guid);

    Task<string> UploadImage(byte[] imageFile, string ruta, string guid);

    bool DeleteImage(string ruta, string guid);
}