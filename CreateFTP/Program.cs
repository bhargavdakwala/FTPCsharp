// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.IO.Enumeration;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

string ftpUrl = "FTP Path";
string userName = "UserName";
string password = "Password";
string filePath = "FilePath";

#region working code for simple file upload on ftp
try
{


    //Create FTP request
   
    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpUrl + "/" + Path.GetFileName(filePath));
    request.Proxy = null;
    request.Method = WebRequestMethods.Ftp.UploadFile;
    request.Credentials = new NetworkCredential(userName, password);
    request.UsePassive = true;
    request.UseBinary = true;
    request.KeepAlive = false;
    request.EnableSsl = true;
    request.Timeout = 10000000;
    request.ReadWriteTimeout = 10000000; ;

    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

    Console.WriteLine(String.Format("Uploading {0}...", request));
    string responseString = string.Empty;
    using (WebResponse weresponse = request.GetResponse())
    {
        using (Stream stream1 = weresponse.GetResponseStream())
        {
            using (StreamReader reader =
            new StreamReader(stream1, Encoding.UTF8))
            {
                responseString = reader.ReadToEnd();
            }
        }
    }

    var response = (FtpWebResponse)request.GetResponse();

    //Load the file
    FileStream stream = File.OpenRead(filePath);
    byte[] buffer = new byte[stream.Length];

    stream.Read(buffer, 0, buffer.Length);
    stream.Close();

    //Upload file
    Stream reqStream = request.GetRequestStream();
    reqStream.Write(buffer, 0, buffer.Length);
    reqStream.Close();

}
catch (WebException wEx)
{
    Console.WriteLine(wEx.Message, "Upload Error");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message, "Upload Error");
}

#endregion

