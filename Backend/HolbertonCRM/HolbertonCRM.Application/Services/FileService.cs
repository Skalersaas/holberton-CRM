using HolbertonCRM.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Services
{
    public class FileService : IFileService
    {
        public string ReadFile(string path, string body)
        {
            using (StreamReader stream = new StreamReader(path))
            {
                body = stream.ReadToEnd();
            }
            return body;
        }
    }
}
