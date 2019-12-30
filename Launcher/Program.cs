using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{ 
    class ftpUser
    {
        string login="w_lipetskoblvo_f6d8ab69";
        string password="790b142e456";
        string ftp_folder="http/wp-content/themes/Ic-blank-master";
        string ftp_name="lipetskoblvodokanal.1gb.ru";
        string file_name = "data-2.csv";
        string folder_name="";
        protected static string path = "settings.ini";
        protected IniFile set = new IniFile(path);
        public ftpUser()
        {
            takeDefaultSettings();
        }
        public bool takeDefaultSettings()
        {
            try
            {
                login = set.ReadINI("ftp", "login");
                password = set.ReadINI("ftp", "password");
                ftp_folder = set.ReadINI("ftp", "folder");
                ftp_name = set.ReadINI("ftp", "server");
                file_name = set.ReadINI("data", "file");
                folder_name = set.ReadINI("data", "folder");
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool s_ftp()
        {
            try
            {
                Console.WriteLine("FTP server name:");
                string n = Console.ReadLine();
                Console.WriteLine("Login:");
                string l = Console.ReadLine();
                Console.WriteLine("Password:");
                string p = Console.ReadLine();
                Console.WriteLine("Directory:");
                string f = Console.ReadLine();
                if (n != "") set.Write("ftp", "server", n);
                if (l != "") set.Write("ftp", "login", l);
                if (p != "") set.Write("ftp", "password", p);
                if (f != "") set.Write("ftp", "folder", f);
                takeDefaultSettings();
                Console.WriteLine("Success");
                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool d_ftp()
        {
           
            ReportWriter rw = new ReportWriter("./report/report.rep");
            try
            {
                rw.writeStringReport("Попытка записи файла "  + DateTime.Now);
                FileInfo f = new FileInfo(folder_name + file_name);
                string uri = "ftp://" +"www."+ftp_name + "/" + ftp_folder + "/" + f.Name;
                FtpWebRequest reqFtp;
                reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFtp.Credentials = new NetworkCredential(login,password);
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
                reqFtp.Timeout =40000;
                reqFtp.UseBinary = true; 
                reqFtp.ContentLength = f.Length;
                rw.writeStringReport("Получен Файл " + f.Length + "Кбайт" + DateTime.Now);
                int buffer = 2048;
            byte[] buff = new byte[buffer];
            int contentLength;
            FileStream fs = f.OpenRead();
            
                 Console.WriteLine("Trying to upload file:"+f.Length+"Kb");

                Stream strm = reqFtp.GetRequestStream();
                  contentLength = fs.Read(buff,0,buffer);
                  while(contentLength!=0)
                  {
                      strm.Write(buff,0,contentLength);
                      contentLength = fs.Read(buff,0,buffer);
                  }
                  strm.Close();
                  fs.Close();
                  Console.WriteLine("Success");
                rw.writeStringReport("Файл успешно передан  " + f.Length + "Кбайт"+DateTime.Now);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                rw.writeStringReport("Ошибка " + e.Message + DateTime.Now+" Обратитесь в поддержку. Нажмите любую клавишу ");
                Console.ReadLine();
                return false;
            }
            
        }
        public bool s_folder()
        {
            try
            {
                Console.WriteLine("Directory:");
                string f = Console.ReadLine();
                if (f != "") set.Write("data", "folder", f);
                takeDefaultSettings();
                Console.WriteLine("Success");
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool s_file()
        {
            try
            {
                Console.WriteLine("File name:");
                string n = Console.ReadLine();
                if (n != "") set.Write("data", "file", n);
                takeDefaultSettings();
                Console.WriteLine("Success");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public void  help()
        {
            Console.WriteLine("Сервер загрузки файлов на сайт ОГУП Липецкоблводоканал. Помощь:");
            Console.WriteLine("Команды:");
            Console.WriteLine("s_ftp        Настройка сервера");
            Console.WriteLine("d_ftp        Загрузить файл");
            Console.WriteLine("s_folder     Настроить папку");
            Console.WriteLine("s_file       Настроить имя файла");
            Console.WriteLine("-h           Помощь");
            Console.WriteLine("-clean       Очистить консоль");
            Console.WriteLine("ex           Выход");
        }
        
    }
    class Program
    {
        static void Main(string[] args)
        {
           
            ftpUser ftp = new ftpUser();
            if (args.Contains("auto")) { ftp.d_ftp(); }
            else
            {
                Console.WriteLine("Launcher of dispetcher's soft. ");
                string command = "";
                while (command != "ex")
                {
                    Console.WriteLine("Your command: ");
                    command = Console.ReadLine();
                    switch (command)
                    {
                        case "d_ftp": Console.WriteLine("FTP download:"); ftp.d_ftp(); break;//загрузить на сайт
                        case "s_ftp": Console.WriteLine("FTP settings:"); ftp.s_ftp(); break;//логин, пароль, сервер
                        case "s_folder": Console.WriteLine("Folder name settings:"); ftp.s_folder(); break;//папка на компе
                        case "s_file": Console.WriteLine("File name settings:"); ftp.s_file(); break;//имя файла
                        case "dd_ftp": Console.WriteLine("Вы нажали dd_ftp"); break;//
                        case "-h": Console.WriteLine("Help:"); ftp.help(); break;//
                        case "-clean": Console.Clear(); ftp.help(); break;//
                    }
                }
            }
        }
    }
    public class ReportWriter
    {
        private string path;
        public ReportWriter(string _path)
        {
            path = _path;
        }
        public bool writeStringReport(string str)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter w = new StreamWriter(fs, Encoding.UTF8);
                w.WriteLine(str);
                w.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи в отчет.  " + ex.Message);
                return false;
            }
        }
    }

}
