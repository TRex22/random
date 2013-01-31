using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.IO;

namespace FinalWar
{
    class DBUtil
    {
        static SqlCeCommand cmd;
        static SqlCeConnection con;

        public struct UData
        {
            public int id;
            public string uname;
            public string mxitid;
            public int state;
            public int level;
            public int exp;

            public decimal bank;
            public decimal wallet;
            public int atk;
            public int def;
            public int armour;
            public int gloves;
            public int helmet;
            public int weapon;
            public int fighting;
        }

        public DBUtil()
        {

            con = new SqlCeConnection(Program.config.ConnString.Replace("[ap]", Directory.GetCurrentDirectory()));
            
            con.Open();
            
        }

        public UData GetUdata(string mid)
        {
            UData ud;

            int pos = FindInList(mid);
            if (pos == -1)
            {

                ud = new UData();

                cmd = new SqlCeCommand("SELECT * FROM users WHERE mxitid='" + mid + "'", con);
                SqlCeDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ud.id = reader.GetInt32(0);
                    ud.uname = reader.GetString(1);
                    ud.mxitid = reader.GetString(2);
                    ud.state = reader.GetInt32(3);
                    ud.level = reader.GetInt32(4);
                    ud.exp = reader.GetInt32(5);
                    ud.bank = reader.GetDecimal(6);
                    ud.wallet = reader.GetDecimal(7);
                    ud.atk = reader.GetInt32(8);
                    ud.def = reader.GetInt32(9);
                    ud.armour = reader.GetInt32(10);
                    ud.gloves = reader.GetInt32(11);
                    ud.helmet = reader.GetInt32(12);
                    ud.weapon = reader.GetInt32(13);
                    ud.fighting = reader.GetInt32(14);
                }
                else
                {
                    ud.id = -1;
                }
                Program.ActiveUsers.Add(ud);
            }
            else
            {
                ud = Program.ActiveUsers[pos];
            }


            return ud;
        }

        public UData createUser(string mid)
        {
            UData ud = new UData();

            cmd = new SqlCeCommand("INSERT INTO users(uname,mxitid,state) VALUES ('" + mid + "','" + mid + "',0)", con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCeCommand("SELECT * FROM users WHERE mxitid='"+mid+"'",con);
            SqlCeDataReader reader = cmd.ExecuteReader();

            reader.Read();
            ud.id = reader.GetInt32(0);

            ud.uname = mid;
            ud.mxitid = mid;
            ud.state = -2;

            return ud;
        }

        public int FindInList(string mid)
        {
            int position = -1;
            for (int i = 0; i < Program.ActiveUsers.Count; i++)
            {
                if (mid == Program.ActiveUsers[i].mxitid)
                {
                    position = i;
                    break;
                }
            }

            return position;
        }

        public void SaveData(string mid)
        {
            UData ud;

            int pos = FindInList(mid);
            if (pos == -1)
            {
                return;
            }
            ud = Program.ActiveUsers[pos];


            cmd = new SqlCeCommand("UPDATE users SET uname='" + ud.uname + "', bank='"+ud.bank.ToString()+"', wallet='"+ud.wallet.ToString()+"', atk='"+ ud.atk.ToString() 
                +"', def='"+ud.def.ToString()+"', armour='"+ud.armour.ToString()+"', gloves='"+ud.gloves.ToString()+"', helmet='"+ud.helmet.ToString()+"', weapon='"+ ud.weapon.ToString() 
                +"', level='"+ud.level.ToString()+"', exp='"+ud.exp.ToString()+"', fighting=-1, state=0 WHERE " +
                    "id=" + ud.id.ToString(), con);
            cmd.ExecuteNonQuery();

            Program.ActiveUsers.RemoveAt(pos);
        }
    }
}
