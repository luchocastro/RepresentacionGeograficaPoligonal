using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Security.Cryptography;
namespace Hexagon.Services.Helpers
{
    public class UserHelper
    {
        public  static User GetUser (string name, string pwd)
        {
            string ApiDirectory = Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);


            var FileConfiguration = Path.Combine(new string[] { ApiDirectory, ServicesConstants.FilesDirectory, name });
            var FilePWD = Path.Combine(new string[] { ApiDirectory, ServicesConstants.FilesDirectory, name, pwd  });

            if (File.Exists(FileConfiguration))
            {
                var FilePWDLines = File.ReadAllText(FilePWD);

                var OldPass = StringCipher.Decrypt(FilePWDLines, "Hexagono");
                if (OldPass == pwd)
                {
                    var ProyectosDir = Path.Combine(new string[] { ApiDirectory, ServicesConstants.FilesDirectory, name, "Proyectos" });
                    List<string> Proyectos = new List<string>();
                    if (Directory.Exists(ProyectosDir))
                        foreach (string dir in Directory.GetDirectories(ProyectosDir))
                        {
                            Proyectos.Add(dir);
                        }

                    User UserExiste = new User { Name = name  };
                    return UserExiste;
                }
            }
            return null;
        }
    }
}
