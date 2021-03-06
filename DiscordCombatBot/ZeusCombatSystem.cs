﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DiscordCombatBot
{
    class ZeusCombatSystem
    {
        List<Weapon> weapons;
        Shop shop;
        public static List<User> userList = new List<User>();
        public ZeusCombatSystem()
        {
            weapons = new List<Weapon>();
            shop = new Shop(weapons);
            initItems();
        }

        public void registerUser(String uID, String n, String p)
        {
            User user = new User(uID, n, p);
            userList.Add(user);
            Console.WriteLine("Registered User with Hash: " + user.GetHashCode());
            Console.WriteLine("User credentials: " + user.UserID + " " + user.Nickname + " " + user.Profession);
            saveUserToFile();
        }

        public String checkUserString(String registerMessage, ulong avatarID)
        {
            List<string> args = registerMessage.Split(' ').ToList();

            if (args.Count() < 3 || args.Count() > 3)
            {
                return "Please type the command in the following scheme: !zeusRegister [nickname] [profession]";
            }
            else
            {


                args.Insert(1, avatarID.ToString());
                String nick = args[2];
                String prof = args[3];




                if (nick.Length == 0 || prof.Length == 0 || nick == null || prof == null)
                {
                    return "Please type the command in the following scheme: !zeusRegister [nickname] [profession]";
                }
                else
                {
                    if (prof.ToUpper() == "ARCHER" || prof.ToUpper() == "WARRIOR" || prof.ToUpper() == "MAGE")
                    {
                        if (alreadyRegistered(args.ToArray()))
                        {
                            return "The Nickname is already taken or you are already registered";
                        }
                        else
                        {
                            registerUser(args[1], args[2], args[3]);
                            return "Welcome to the battle System " + args[2] + "!";
                        }
                    }
                    else
                    {
                        return "The Profession you chose is not valid. Please choose either Archer, Warrrior or Mage";
                    }
                }
            }
                      
        }

        public Boolean alreadyRegistered(String[] args)
        {
            Boolean containsID = false;
            Boolean containsNickname = false;

            foreach(User u in userList)
            {
                if(u.UserID == args[1])
                {
                    containsID = true;
                }else if(u.Nickname == args[2])
                {
                    containsNickname = true;
                }
            }

            if(containsID == true || containsNickname == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean alreadyRegistered(ulong id)
        {
            User user = new User();

            foreach (User u in userList)
            {
                if (u.UserID == id.ToString())
                {
                    user = u;
                }
            }

            if (user == new User())
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public User findUser(ulong id)
        {
            User user = null;

            foreach (User u in userList)
            {
                if (u.UserID == id.ToString())
                {
                    user = u;
                }
            }

            return user;
        }

        public void readUserFromFile()
        {
            userList = (from e in XDocument.Load(@"../../../data/user.xml").Root.Elements("User")
                        select new User
                        {
                            UserID = (string)e.Element("Id"),
                            Nickname = (string)e.Element("Nickname"),
                            Profession = (string)e.Element("Profession"),
                            Money = (int)e.Element("Money"),
                            Weapons = (from w in XDocument.Load(@"../../../data/user.xml").Root.Elements("User").Elements("Weapon")
                                      select new Weapon
                                      {
                                          ItemName = (string)w.Element("Name"),
                                          ItemDesc = (string)w.Element("Description"),
                                          ItemDmg = (double)w.Element("Damage"),
                                          ItemID = (int)w.Element("ID"),
                                          ItemType = (string)w.Element("Type"),
                                          Price = (int)w.Element("Price")
                                      }).ToList()
                        }).ToList();
        }

        public String showMoney(ulong id)
        {
            return "Your Current Account Balance is: " + findUser(id).Money;
        }

        public String[] showHero(ulong id)
        {
            if (alreadyRegistered(id)) {
                User user = findUser(id);
                String[] args = new string[3] { user.Nickname, user.Profession, user.Money.ToString() };
                return args;
            }
            else
            {
                return null;
            }
            
        }

        public void saveUserToFile()
        {
            var xml = new XElement("Users", userList.Select(x => new XElement("User",
                                               new XElement("Id", x.UserID),
                                               new XElement("Nickname", x.Nickname),
                                               new XElement("Profession", x.Profession),
                                               new XElement("Money", x.Money),
                                               new XElement("Weapons"), x.Weapons.Select(y => new XElement("Weapon", 
                                                                                                    new XElement("ID", y.ItemID),
                                                                                                    new XElement("Name", y.ItemName),
                                                                                                    new XElement("Description", y.ItemDesc),
                                                                                                    new XElement("Type", y.ItemType),
                                                                                                    new XElement("Damage", y.ItemDmg),
                                                                                                    new XElement("Price", y.Price)
                                                                                                    )))));

            XDocument doc = new XDocument();
            doc.Add(xml);
            doc.Save(@"../../../data/user.xml");
        }

        public String userBuysItem(ulong id, string itemNr)
        {
            User user = findUser(id);
            int nr = int.Parse(itemNr) -1;
            if(!alreadyRegistered(id))
            {
                return "Please register before you take any actions! Hint: !zeusHelp";
            }
            else
            {
                Weapon targetItem = shop.WeaponStock[nr];
                if (hasUserItem(user, targetItem)) {

                    user.Money += targetItem.Price;
                    shop.RemoveItem(targetItem);
                    initItems();
                    saveUserToFile();
                    return "You already had that item in your Inventory, so we sold it for you:)";
                }
                else
                {
                    if (shop.buyItem(findUser(id), shop.WeaponStock[nr]))
                    {
                        initItems();
                        saveUserToFile();
                        return "You just bought: " + targetItem.ItemName;
                    }
                    else
                    {

                        return "Sorry, you dont have enough Money!";
                    }
                }
            }
        }

        public Boolean hasUserItem(User u, Weapon w)
        {
            foreach(Weapon weapon in u.Weapons)
            {
                if(weapon.ItemName == w.ItemName)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Weapon> showInventory(ulong id)
        {
            User user = findUser(id);

            String inv = "Inventory: ";

            if (!user.Weapons.Any())
            {
                return null;
            }
            else
            {
                return user.Weapons;
            }
        }

        public String showShop()
        {
            String s = "Shop: ";

            for(int i = 0; i < shop.WeaponStock.Count; i++)
            {
                s = s + (" Item " + (i+1).ToString() + " : "+ shop.WeaponStock.ElementAt(i).ItemName + " for " + shop.WeaponStock.ElementAt(i).Price + " Money");
            }

            return s;
        }

        public void initItems()
        {
            

            Weapon woodenSword = new Weapon(0, "Wooden Sword", "My wood! Made out of.. hmm", "warrior", 1, 5);

            shop.AddItem(woodenSword);
        }

        private List<User> UserList { get => userList; set => userList = value; }
    }
}
