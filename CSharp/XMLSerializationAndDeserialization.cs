using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace FinalWar
{
    [Serializable()]

    public class Config : ISerializable
    {
        public int VersionMajor;
        public int VersionMinor;

        public List<string> Admins = new List<string>();

        public string ConnString;

        public string Notice;

        public Config()
        {
            //default config
            VersionMajor = 0;
            VersionMinor = 1;

            //Admins = new IList();
            Admins.Add("");

            ConnString = "";
            Notice = "No notices.";
        }

        public void Save()
        {
            FileStream fs = new FileStream("Config.xml",FileMode.OpenOrCreate, FileAccess.Write);
            XmlSerializer xs = new XmlSerializer(this.GetType());
            try
            {
                xs.Serialize(fs,this);
            }
            finally
            {
                fs.Close();
            }
            Console.WriteLine("Config saved");
        }

        public void Load()
        {
            FileStream fs = new FileStream("Config.xml", FileMode.Open, FileAccess.Read);
            XmlSerializer xs = new XmlSerializer(this.GetType());
            try
            {
                Config ld = new Config();
                ld = (Config)xs.Deserialize(fs);

                VersionMajor = ld.VersionMajor;
                VersionMinor = ld.VersionMinor;
                Admins = ld.Admins;
                ConnString = ld.ConnString;
                Notice = ld.Notice;

            }
            finally
            {
                fs.Close();
            }
            Console.WriteLine("Config loaded");

        }


        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
