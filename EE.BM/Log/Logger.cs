using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EE.BM
{
    public class Logger:IDisposable
    {
        private StreamWriter logWriter;

        private string logFile = string.Empty;
        private string tempFile = Path.GetTempFileName();

        private const string ERROR = "Error: {0}-{1}";
        private const string WARNING = "Warning: {0}-{1}";
        private const string EVENT = "Event: {0}-{1}";

        private Logger() 
        {
            logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.log", "event"));

            logWriter = new StreamWriter(tempFile);
        }

        public void WriterWarning(string message)
        {
            logWriter.WriteLine(string.Format(WARNING,message,getTime()));
        }

        public void WriteMessage(string message)
        {
            logWriter.WriteLine(string.Format(EVENT,message,getTime()));
        }

        public void WriteError(string message)
        {
            logWriter.WriteLine(string.Format(ERROR, message, getTime()));
        }

        private string getTime()
        {
            return DateTime.Now.ToString("yy-MM-dd hh:mm:ss");
        }
        

        private static Logger logger = null;

        public static Logger CreateLogger()
        {
            if (logger == null) logger = new Logger();

            return logger;
        }

        public void Dispose()
        {
            if (logWriter != null)
            {
                logWriter.Flush();
                logWriter.Close();
                logWriter = null;
            }

            File.Copy(tempFile, logFile, true);
        }
    }
}
