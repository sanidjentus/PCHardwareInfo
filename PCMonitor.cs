using System;
using System.Management;
using System.Xml;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace getHardwareInformation
{
    class PCMonitor
    { 
        private static string GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            string result = "";// = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);
            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    result = obj[ClassItemField].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        private static void InputInXML(XmlDocument xmlDoc)
        {
            XmlNode rootNode = xmlDoc.CreateElement("hardware_info");
            xmlDoc.AppendChild(rootNode);

            //процессор
            XmlNode procNode = xmlDoc.CreateElement("processor");

            XmlAttribute manufacturerProc = xmlDoc.CreateAttribute("manufacturer");
            string manufVal = GetHardwareInfo("Win32_Processor", "Manufacturer");
            manufacturerProc.Value = manufVal;// "processor manufacturer"; //???
            procNode.Attributes.Append(manufacturerProc);
            rootNode.AppendChild(procNode);

            XmlAttribute descriptionProc = xmlDoc.CreateAttribute("description");
            string descVal = GetHardwareInfo("Win32_Processor", "Description");
            descriptionProc.Value = descVal;//"processor decription"; //???
            procNode.Attributes.Append(descriptionProc);
            rootNode.AppendChild(procNode);

            XmlAttribute nameProc = xmlDoc.CreateAttribute("name");
            string nameVal = GetHardwareInfo("Win32_Processor", "Name");
            nameProc.Value = nameVal;// "processor name"; //??
            procNode.Attributes.Append(nameProc);
            rootNode.AppendChild(procNode);

            //видеокарта
            XmlNode videoNode = xmlDoc.CreateElement("video_controller");
            
            XmlAttribute videoProc = xmlDoc.CreateAttribute("video_processor");
            string vidVal = GetHardwareInfo("Win32_VideoController", "VideoProcessor");
            videoProc.Value = vidVal;// "video processor";
            videoNode.Attributes.Append(videoProc);
            rootNode.AppendChild(videoNode);

            XmlAttribute videoRam = xmlDoc.CreateAttribute("video_ram");
            string ramVal = GetHardwareInfo("Win32_VideoController", "AdapterRAM");
            videoRam.Value = ramVal + " B";// "video RAM";
            videoNode.Attributes.Append(videoRam);
            rootNode.AppendChild(videoNode);

            XmlAttribute nameVid = xmlDoc.CreateAttribute("name");
            string vidnameVal = GetHardwareInfo("Win32_VideoController", "Name");
            nameVid.Value = vidnameVal;// "video conroller name";
            videoNode.Attributes.Append(nameVid);
            rootNode.AppendChild(videoNode);

            //видеопривод
            XmlNode dvdNode = xmlDoc.CreateElement("cd_dvd");
            
            XmlAttribute dvdLetter = xmlDoc.CreateAttribute("letter");
            string drVal = GetHardwareInfo("Win32_CDROMDrive", "Drive");
            dvdLetter.Value = drVal;// "cd-dvd letter";
            dvdNode.Attributes.Append(dvdLetter);
            rootNode.AppendChild(dvdNode);

            XmlAttribute nameDvd = xmlDoc.CreateAttribute("name");
            string cdVal = GetHardwareInfo("Win32_CDROMDrive", "Name");
            nameDvd.Value = cdVal;// "cd-dvd name";
            dvdNode.Attributes.Append(nameDvd);
            rootNode.AppendChild(dvdNode);

            //жесткий диск
            XmlNode hddNode = xmlDoc.CreateElement("hdd");

            XmlAttribute hddCaption = xmlDoc.CreateAttribute("caption");
            string capVal = GetHardwareInfo("Win32_DiskDrive", "Caption");
            hddCaption.Value = capVal;// "hdd caption";
            hddNode.Attributes.Append(hddCaption);
            rootNode.AppendChild(hddNode);

            XmlAttribute hddSize = xmlDoc.CreateAttribute("size");
            string hddsizeVal = GetHardwareInfo("Win32_DiskDrive", "Size");
            hddSize.Value = hddsizeVal + " B";// "hdd size";
            hddNode.Attributes.Append(hddSize);
            rootNode.AppendChild(hddNode);

            //RAM
            XmlNode ramNode = xmlDoc.CreateElement("ram");

            XmlAttribute ramSize = xmlDoc.CreateAttribute("size");
            string ramsizeVal = GetHardwareInfo("Win32_PhysicalMemory", "Capacity");
            ramSize.Value = ramsizeVal + " B";// "ram size";
            ramNode.Attributes.Append(ramSize);
            rootNode.AppendChild(ramNode);
            
            xmlDoc.Save("D:\\xmlDoc.xml");
        }

        private static void SendMail(string recipient, string message)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.mail.ru"); //???
                client.Credentials = new NetworkCredential("your E-mail", "your passord");
                MailMessage mail = new MailMessage("your e-mail", "sendTo(e-mail)", "test1", "test2");
                string file = "D:/xmlDoc.xml";
                Attachment data = new Attachment(file, MediaTypeNames.Application.Xml);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                mail.Attachments.Add(data);
                client.Send(mail);
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            InputInXML(xmlDoc);
            SendMail("zolotuhin00@list.ru", "XML-file");

        }
    }
}