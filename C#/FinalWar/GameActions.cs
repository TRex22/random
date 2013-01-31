using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinalWar
{
    class GameActions
    {
        public int Bank_Withdraw(string mes, string mid)
        {
            DBUtil.UData ud = Program.ActiveUsers[Program.dbutil.FindInList(mid)];
            string[] _split = mes.Split(" "[0]);
            if (_split.Length == 2)
            {
                if (_split[0] == "with")
                {
                    int i = -1;
                    if (int.TryParse(_split[1],out i))
                    {
                        if (i < 0) { return -2; }
                        if (ud.bank > i)
                        {
                            ud.wallet += i;
                            ud.bank -= i * (decimal)1.03; //3% fee on transactions
                            Program.ActiveUsers[Program.dbutil.FindInList(ud.mxitid)] = ud;
                            Console.WriteLine(ud.bank.ToString() + " " + Program.ActiveUsers[Program.dbutil.FindInList(ud.mxitid)].bank.ToString());
                            return i;
                        }
                    }
                    return -3;
                }
            }
            return -1; //transfer failed
        }

        public int Bank_Deposit(string mes, string mid)
        {
            DBUtil.UData ud = Program.ActiveUsers[Program.dbutil.FindInList(mid)];
            string[] _split = mes.Split(" "[0]);
            if (_split.Length == 2)
            {
                if (_split[0] == "dep")
                {
                    int i = -1;
                    if (int.TryParse(_split[1], out i))
                    {
                        if (i < 0) { return -2; }
                        if (ud.wallet > i)
                        {
                            ud.wallet -= i;
                            ud.bank -= i * (decimal)1.03; //3% fee on transactions
                            ud.bank += i;
                            Program.ActiveUsers[Program.dbutil.FindInList(ud.mxitid)] = ud;
                            return i;
                        }
                    }
                    return -3;
                }
            }
            return -1; //transfer failed
        }

        public int Core_Nick(string mes, string mid)
        {
            DBUtil.UData ud = Program.ActiveUsers[Program.dbutil.FindInList(mid)];
            string[] _split = mes.Split(" "[0]);
            if (_split.Length == 2)
            {
                if (_split[0] == "nick")
                {
                    //TODO: check for no duplicate nicknames
                    ud.uname = _split[1];
                    Program.ActiveUsers[Program.dbutil.FindInList(ud.mxitid)] = ud;
                    return 0;
                }
            }
            return -1; //transfer failed
        }

        public void Core_News(string mes, string mid)
        {
            foreach (string s in Program.config.Admins)
            {
                if (s == mid)
                {
                    Program.config.Notice = mes.Remove(0, 6);
                    return;
                }
            }
        }
    }
}
