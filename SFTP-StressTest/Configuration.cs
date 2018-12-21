using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace SFTP_StressTest
{
  public  class Configuration
    {
        public SessionOptions session() {
            // Configuración de opciones de sesión
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = "Write the ip",
                UserName = "",
                Password = "",
                SshHostKeyFingerprint ="",  // key Finger public generated start with "ecdsa-sha2"

            };
            return sessionOptions;
        }
        

    }
}
