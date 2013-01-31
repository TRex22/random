using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MXit;
using MXit.Messaging;
using MXit.Messaging.MessageElements;
using MXit.Messaging.MessageElements.Actions;
using MXit.Messaging.MessageElements.Replies;
using MXit.User;

namespace FinalWar
{
    class GameMenus
    {
        public GameMenus()
        {

        }

        public int CheckStates(DBUtil.UData ud, MessageReceived received, MessageToSend mes)
        {
            string b = "";
            if (received.Body.Contains("type=reply|nm="))
            {
                //TODO: Regex
                b = received.Body.Split("|"[0])[2].Split("="[0])[1].ToLower();
            }
            else
            {
                b = received.Body.ToLower();
            }
            //Console.WriteLine(b);
            switch (ud.state)
            {
                case -2: //first time user
                    {
                        if (b.Contains("nick"))
                        {
                            if (Program.gameActions.Core_Nick(b, ud.mxitid) == 0)
                            {
                                mes.AppendLine("Nick changed successfully!", System.Drawing.Color.Green, TextMarkup.Bold);
                            }
                            else
                            {
                                mes.AppendLine("Nick change unsuccessful! Usage: nick <newnick>", System.Drawing.Color.Red, TextMarkup.Bold);
                            }
                            return -2; //Stay on this screen
                        }
                        if (b == "menu")
                        {
                            return 0;
                        }
                        if (b == "help")
                        {
                            return 1;
                        }
                        return -2;
                    }
                    //break;
                case 0: //Main Menu
                    {
                        if (b.Contains(".news"))
                        {
                            Program.gameActions.Core_News(b, ud.mxitid);
                        }
                        if (b == "help")
                        {
                            return 1;
                        }
                        if (b == "1")
                        {
                            return 2;
                        }
                        if (b == "2")
                        {
                            return 3;
                        }
                        if (b == "3")
                        {
                            return 4;
                        }
                        if (b == "4")
                        {
                            return 5;
                        }
                        if (b == "5")
                        {
                            return 6;
                        }
                    }
                    break;
                case 1: //Help Menu
                    {
                        return 0;
                    }
                    //break;
                case 2:
                    {
                        if (b == "menu")
                        {
                            return 0;
                        }
                        return 2;
                    }
                case 3: //bank
                    {
                        if (b == "menu")
                        {
                            return 0;
                        }
                        if (b.Contains("with"))
                        {
                            int o = Program.gameActions.Bank_Withdraw(b, ud.mxitid);
                            if (o >= 0)
                            {
                                mes.AppendLine("NOTICE:", System.Drawing.Color.Green ,TextMarkup.Bold);
                                mes.AppendLine("You withdraw $" + o.ToString() + " from the bank", System.Drawing.Color.Green);
                            }
                            else if (o == -1)
                            {
                                mes.AppendLine("USAGE:", System.Drawing.Color.Blue, TextMarkup.Bold);
                                mes.AppendLine("Command: with <amount>", System.Drawing.Color.Blue);
                            }
                            else if (o == -2)
                            {
                                mes.AppendLine("ERROR:", System.Drawing.Color.Red, TextMarkup.Bold);
                                mes.AppendLine("You cannot withdraw negative amounts", System.Drawing.Color.Red);
                            }
                            else if (o == -3)
                            {
                                mes.AppendLine("NOTICE:", System.Drawing.Color.Red, TextMarkup.Bold);
                                mes.AppendLine("You have insufficient funds to withdraw that amount", System.Drawing.Color.Red);
                            }
                            mes.AppendLine();
                        }
                        if (b.Contains("dep"))
                        {
                            int o = Program.gameActions.Bank_Deposit(b, ud.mxitid);
                            if (o >= 0)
                            {
                                mes.AppendLine("NOTICE:", System.Drawing.Color.Green, TextMarkup.Bold);
                                mes.AppendLine("You deposit $" + o.ToString() + " into the bank", System.Drawing.Color.Green);
                            }
                            else if (o == -1)
                            {
                                mes.AppendLine("USAGE:", System.Drawing.Color.Blue, TextMarkup.Bold);
                                mes.AppendLine("Command: dep <amount>", System.Drawing.Color.Blue);
                            }
                            else if (o == -2)
                            {
                                mes.AppendLine("ERROR:", System.Drawing.Color.Red, TextMarkup.Bold);
                                mes.AppendLine("You cannot deposit negative amounts", System.Drawing.Color.Red);
                            }
                            else if (o == -3)
                            {
                                mes.AppendLine("NOTICE:", System.Drawing.Color.Red, TextMarkup.Bold);
                                mes.AppendLine("You have insufficient funds to deposit that amount", System.Drawing.Color.Red);
                            }
                            mes.AppendLine();
                        }
                        return 3;
                    }
                case 4: //tech
                    {
                        if (b == "menu")
                        {
                            return 0;
                        }
                        return 4;
                    }
                case 5: //gang
                    {
                        if (b == "menu")
                        {
                            return 0;
                        }
                        return 5;
                    }
                case 6: //streets
                    {
                        if (b == "menu")
                        {
                            return 0;
                        }
                        return 6;
                    }
            }

            return 0; //all else fails go to the menu
        }

        public void BuildMenu(DBUtil.UData ud, MessageToSend mes)
        {
            IMessageElement link;
            switch (ud.state)
            {
                case -2: //first time user
                    {
                        mes.AppendLine("Final War", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Welcome to Final War, the war to end all wars! Join the struggle to control the cyber world through brute force.");
                        mes.AppendLine();
                        mes.Append("It is your first time here, to get started use the ");
                        mes.Append("nick", TextMarkup.Underline);
                        mes.Append(" command to change your nickname (currently: " + ud.uname + ", this is the only free name change available), then why dont you read the ");

                        link = MessageBuilder.Elements.CreateLink("tuthelp",          // Optional
                                                          "help",             // Compulsory
                                                          "help",  // Optional
                                                          "help");        // Optional
                        mes.Append(link);

                        mes.AppendLine(" otherwise, head to the main menu to jump in.");

                        link = MessageBuilder.Elements.CreateLink("tutmenu",          // Optional
                                                          "Back to the menu.",             // Compulsory
                                                          "menu",  // Optional
                                                          "menu");        // Optional
                        mes.Append(link);
                        return;
                    }
                case 0: //main menu
                    {
                        mes.AppendLine("Final War", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Welcome back " + ud.uname + " what would you like to do?");
                        mes.AppendLine();

                        link = MessageBuilder.Elements.CreateLink("menu1",          // Optional
                                  "1) My Hideout",             // Compulsory
                                  "1",  // Optional
                                  "1");        // Optional
                        mes.AppendLine(link);

                        link = MessageBuilder.Elements.CreateLink("menu2",          // Optional
                                  "2) Bank",             // Compulsory
                                  "2",  // Optional
                                  "2");        // Optional
                        mes.AppendLine(link);

                        link = MessageBuilder.Elements.CreateLink("menu3",          // Optional
                                  "3) Tech",             // Compulsory
                                  "3",  // Optional
                                  "3");        // Optional
                        mes.AppendLine(link);

                        link = MessageBuilder.Elements.CreateLink("menu4",          // Optional
                                  "4) Gang",             // Compulsory
                                  "4",  // Optional
                                  "4");        // Optional
                        mes.AppendLine(link);

                        link = MessageBuilder.Elements.CreateLink("menu5",          // Optional
                                  "5) The streets",             // Compulsory
                                  "5",  // Optional
                                  "5");        // Optional
                        mes.AppendLine(link);




                        link = MessageBuilder.Elements.CreateLink("menu9",          // Optional
                                  "9) Leaderboards",             // Compulsory
                                  "9",  // Optional
                                  "9");        // Optional
                        mes.AppendLine(link);

                        link = MessageBuilder.Elements.CreateLink("menuhelp",          // Optional
                                  "help) Help",             // Compulsory
                                  "help",  // Optional
                                  "help");        // Optional
                        mes.AppendLine(link);

                        mes.AppendLine();
                        mes.AppendLine("News: " + Program.config.Notice);

                        return;
                    }
                case 1: //help menu
                    {
                        mes.AppendLine("Help", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Do stuff, kill people, level up");
                        mes.AppendLine();

                        link = MessageBuilder.Elements.CreateLink("helpmenu",          // Optional
                                  "Back",             // Compulsory
                                  "menu",  // Optional
                                  "menu");        // Optional
                        mes.Append(link);
                        
                        return;
                    }
                case 2: //hideout
                    {
                        mes.AppendLine("Your Hideout", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Your stats:");
                        mes.AppendLine("Level: " + ud.level.ToString() + "  Exp: " + ud.exp.ToString());
                        mes.AppendLine();

                        link = MessageBuilder.Elements.CreateLink("hidemenu",          // Optional
                                  "Back",             // Compulsory
                                  "menu",  // Optional
                                  "menu");        // Optional
                        mes.Append(link);
                        return;
                    }
                case 3: //bank
                    {
                        mes.AppendLine("Your Bank Account", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Your bank details:");
                        mes.AppendLine("Bank Balance: $" + ud.bank.ToString());
                        mes.AppendLine("Wallet Balance: $" + ud.wallet.ToString());
                        mes.AppendLine();
                        mes.Append("Use the ");
                        mes.Append("with", TextMarkup.Underline);
                        mes.AppendLine(" command to withdraw money");
                        mes.Append("Use the ");
                        mes.Append("dep", TextMarkup.Underline);
                        mes.AppendLine(" command to deposit it");

                        link = MessageBuilder.Elements.CreateLink("bankback",          // Optional
                                  "Back",             // Compulsory
                                  "menu",  // Optional
                                  "menu");        // Optional
                        mes.Append(link);

                        return;
                    }
                case 4: //tech
                    {
                        mes.AppendLine("Your Tech", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Equips and Stats:");
                        mes.AppendLine("Atk: " + ud.atk.ToString() + "  Def: " + ud.def.ToString());
                        mes.Append("Armour: ");
                        if (ud.armour == -1)
                        {
                            mes.AppendLine("None");
                        }
                        else
                        {
                            mes.AppendLine(ud.armour.ToString() + "(armour ID)");
                        }
                        mes.Append("Gloves: ");
                        if (ud.gloves == -1)
                        {
                            mes.AppendLine("None");
                        }
                        else
                        {
                            mes.AppendLine(ud.gloves.ToString() + "(gloves ID)");
                        }
                        mes.Append("Helmet: ");
                        if (ud.helmet == -1)
                        {
                            mes.AppendLine("None");
                        }
                        else
                        {
                            mes.AppendLine(ud.helmet.ToString() + "(helmet ID)");
                        }
                        mes.Append("Weapon: ");
                        if (ud.weapon == -1)
                        {
                            mes.AppendLine("None");
                        }
                        else
                        {
                            mes.AppendLine(ud.weapon.ToString() + "(weapon ID)");
                        }
                        mes.AppendLine();
                        link = MessageBuilder.Elements.CreateLink("techback",          // Optional
                                  "Back",             // Compulsory
                                  "menu",  // Optional
                                  "menu");        // Optional
                        mes.Append(link);

                        return;
                    }
                case 5: //gang
                    {
                        mes.AppendLine("Your Gang", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Members, Message, factories, tax, etc");
                        mes.AppendLine();
                        link = MessageBuilder.Elements.CreateLink("gangback",          // Optional
                                  "Back",             // Compulsory
                                  "menu",  // Optional
                                  "menu");        // Optional
                        mes.Append(link);

                        return;
                    }
                case 6: //streets
                    {
                        mes.AppendLine("The Streets", TextMarkup.Bold);
                        mes.AppendLine();
                        mes.AppendLine("Fight, Level, etc");
                        mes.AppendLine();
                        link = MessageBuilder.Elements.CreateLink("streetback",          // Optional
                                  "Back",             // Compulsory
                                  "menu",  // Optional
                                  "menu");        // Optional
                        mes.Append(link);

                        return;
                    }

            }
        }
    }
}
