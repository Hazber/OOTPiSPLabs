using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{

    public class MenuItem
    {
        public enum TCategorys { Burgers, Garneers, Combo, Chips, Drinks_Deserts, Snack};
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
    public class Burgers : MenuItem
    {
        public enum TMeatType { chicken, beef, pork, fish };
        public TMeatType meatType;
        public enum TBurgerSize { Single, Double, Triple, Grand };
        public TBurgerSize Size;
        public Burgers(TMeatType Mtype, TBurgerSize size, int calories, string name, double price) : base(calories, name,price)
        {
            this.meatType = Mtype;
            this.Size = size;
            this.Category =TCategorys.Burgers;
        }

    }

    public class Drinks_Deserts : MenuItem
    {
        public enum TDesertSize { S, M, L };
        public TDesertSize Size;
        public bool NeedAdding;
        public Drinks_Deserts(TDesertSize size, bool ifAdding, int calories, string name, double price) : base(calories, name, price)
        {
            this.NeedAdding = ifAdding;
            this.Size = size;
            this.Category = TCategorys.Drinks_Deserts;
        }
    }
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
    public class Garneers : Snack
    {
        public enum TPortionSize { S, M, L, XL };
        public TPortionSize Portion;
        public Garneers(TPortionSize portion, TSnackType typeofsnack, bool needsause, int calories, string name, double price) : base(typeofsnack, needsause,calories, name,price)
        {
            this.Portion = portion;
            this.Category = TCategorys.Garneers;
        }
    }

    public class Chips : Snack
    {
        public enum TChipsType { Onion, Naggets, Shrimp, Fish, Wings };
        public TChipsType ChipsType;
        public int NumofPieces;
        public Chips(TChipsType type, int num, TSnackType typeofsnack, bool needsause, int calories, string name, double price) : base(typeofsnack, needsause, calories, name, price)
        {
            this.ChipsType = type;
            this.NumofPieces = num;
            this.Category =TCategorys.Chips;
        }
    }

    public class Combo : MenuItem
    {
        public Burgers ComboBurger;
        public Garneers ComboGarneer;
        public Drinks_Deserts ComboDrink;
        public Combo( Burgers cb, Garneers cg, Drinks_Deserts cd, int calories, string name, double price) : base(calories, name, price)
        {
            this.ComboBurger = cb;
            this.ComboGarneer = cg;
            this.ComboDrink = cd;
            this.Category = TCategorys.Combo;
        }
    }
}
