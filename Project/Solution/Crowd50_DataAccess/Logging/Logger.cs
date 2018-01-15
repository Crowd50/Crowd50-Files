namespace Crowd50_DataAccess.Logging
{
    using System;
    using System.IO;

    public static class Logger
    {
        public static string FilePath { get; set; }

        public static void LogError(string iError)
        {
            using (StreamWriter lWriter = new StreamWriter(FilePath))
            {
                lWriter.WriteLine(String.Format("{0:yyyy-MM-dd hh:mm:ss} - {1} - {2}", DateTime.Now, "", iError));
                lWriter.Flush();
                lWriter.Close();
            }
        }
    }
}
