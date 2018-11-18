using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCombatBot
{
    public class Weapon
    {
        private int itemID;
        private String itemName;
        private String itemDesc;
        private String itemType;
        private double itemDmg;
        private int price;

        

        public Weapon()
        {
            
        }

        public Weapon(int itemID, string itemName, string itemDesc, string itemType, double itemDmg, int price) { 
            ItemName = itemName;
            ItemDesc = itemDesc;
            ItemType = ItemType;
            ItemDmg = itemDmg;
            Price = price;
        }

        public int ItemID { get => itemID; set => itemID = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public string ItemType { get => itemType; set => itemType = value; }
        public double ItemDmg { get => itemDmg; set => itemDmg = value; }
        public string ItemDesc { get => itemDesc; set => itemDesc = value; }
        public int Price { get => price; set => price = value; }
    }
}
