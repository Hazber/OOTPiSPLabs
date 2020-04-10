using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    [Serializable]
    public class MenuItem
    {
        public enum TCategorys { Burger, Garneer, Combo, Chip, Drink_Desert, Snack};
        public TCategorys Category;
        public int Calories;
        public string ItemName;
        public double Price;

        public MenuItem( int calories, string name, double price)
        {
            this.Price = price;
            this.Calories = calories;
            this.ItemName = name;
        }
        public void ItemAdd(int i)
        {

        }
    }

    [Serializable]
    public class Burger : MenuItem
    {
        public enum TMeatType { chicken, beef, pork, fish };
        public TMeatType meatType;
        public enum TBurgerSize { Single, Double, Triple, Grand };
        public TBurgerSize Size;
        public Burger(TMeatType Mtype, TBurgerSize size, int calories, string name, double price) : base(calories, name,price)
        {
            this.meatType = Mtype;
            this.Size = size;
            this.Category =TCategorys.Burger;
        }

    }

    [Serializable]
    public class Drink_Desert : MenuItem
    {
        public enum TDesertSize { S, M, L };
        public TDesertSize Size;
        public bool NeedAdding;
        public Drink_Desert(TDesertSize size, bool ifAdding, int calories, string name, double price) : base(calories, name, price)
        {
            this.NeedAdding = ifAdding;
            this.Size = size;
            this.Category = TCategorys.Drink_Desert;
        }
    }

    [Serializable]
    public class Snack : MenuItem
    {
        public enum TSnackType { Garneer, Chips };
        public TSnackType TypeofSnack;
        public bool NeedSause;
        public Snack( TSnackType typeofsnack, bool needsause, int calories, string name, double price) : base(calories, name, price)
        {
            this.TypeofSnack = typeofsnack;
            this.NeedSause = needsause;
            this.Category = TCategorys.Snack;
        }
    }

    [Serializable]
    public class Garneer : Snack
    {
        public enum TPortionSize { S, M, L, XL };
        public TPortionSize Portion;
        public Garneer(TPortionSize portion, TSnackType typeofsnack, bool needsause, int calories, string name, double price) : base(typeofsnack, needsause,calories, name,price)
        {
            this.Portion = portion;
            this.Category = TCategorys.Garneer;
        }
    }

    [Serializable]
    public class Chip : Snack
    {
        public enum TChipsType { Onion, Naggets, Shrimp, Fish, Wings };
        public TChipsType ChipsType;
        public int NumofPieces;
        public Chip(TChipsType type, int num, TSnackType typeofsnack, bool needsause, int calories, string name, double price) : base(typeofsnack, needsause, calories, name, price)
        {
            this.ChipsType = type;
            this.NumofPieces = num;
            this.Category =TCategorys.Chip;
        }
    }

    [Serializable]
    public class Combo : MenuItem
    {
        public Burger ComboBurger;
        public Garneer ComboGarneer;
        public Drink_Desert ComboDrink;
        public Combo( Burger combo_burger, Garneer combo_garneer, Drink_Desert combo_desert, int calories, string name, double price) : base(calories, name, price)
        {
            this.ComboBurger = combo_burger;
            this.ComboGarneer = combo_garneer;
            this.ComboDrink = combo_desert;
            this.Category = TCategorys.Combo;
        }
    }
}
