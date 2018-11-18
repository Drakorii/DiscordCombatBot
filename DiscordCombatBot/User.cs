using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCombatBot
{
    public class User
    {
        private String userID;
        private String nickname;
        private String profession;
        private int money = 0;
        private List<Weapon> weapons = new List<Weapon>();

        //Attributes:
        public double health;
        public double damage;

        public User(string userID, string nickname, string profession)
        {
            UserID = userID ?? throw new ArgumentNullException(nameof(userID));
            Nickname = nickname ?? throw new ArgumentNullException(nameof(nickname));
            Profession = profession ?? throw new ArgumentNullException(nameof(profession));
         
        }

        public User()
        {
            
        }

        public bool AddWeapon(Weapon w)
        {
            if (weapons.Contains(w))
            {
                return false;
            }
            else
            {
                weapons.Add(w);
                return true;
            }
        }

        public string UserID { get => userID; set => userID = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string Profession { get => profession; set => profession = value; }
        internal List<Weapon> Weapons { get => weapons; set => weapons = value; }
        public int Money { get => money; set => money = value; }
    }
}
