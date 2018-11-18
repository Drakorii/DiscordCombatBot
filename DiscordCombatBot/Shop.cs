using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCombatBot
{
    class Shop
    {
        List<Weapon> weaponStock;

        public Shop(List<Weapon> weaponStock)
        {
            WeaponStock = weaponStock;
        }

        public Boolean buyItem(User user,Weapon w)
        {
            int price = w.Price;
            int money = user.Money;

            if(money < price)
            {
                return false;
            }
            else
            {
                user.Money = (user.Money - price);
                user.AddWeapon(w);
                removeItem(w);
                return true;
            }

        }

        public void AddItem(Weapon w)
        {
            weaponStock.Add(w);
        }

        public void RemoveItem(Weapon w)
        {
            weaponStock.Remove(w);
        }

        public void removeItem(Weapon w)
        {
            weaponStock.Remove(w);
        }

        public List<Weapon> WeaponStock { get => weaponStock; set => weaponStock = value; }
    }
}
