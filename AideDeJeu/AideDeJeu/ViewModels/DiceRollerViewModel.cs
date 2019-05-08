using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AideDeJeu.ViewModels
{
    public class DiceRollerViewModel : BaseViewModel
    {
        private Random _Random = null;

        public DiceRollerViewModel()
        {
            _Random = new Random(Environment.TickCount);
        }


        private int _Quantity = 3;
        public int Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetProperty(ref _Quantity, value);
            }
        }
        public int _Type = 6;
        public int Type { get { return _Type; } set { SetProperty(ref _Type, value); } }
        public int _Mod = 0;
        public int Mod { get { return _Mod; } set { SetProperty(ref _Mod, value); } }



        public List<int> RollMRick()
        {
            var dices = new List<int>();
            var roll = RollDices(6, 2);
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            roll = RollDices(6, 2);
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            roll = RollDices(6, 2);
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            return dices;
        }
        public int RollDices(int diceMaxFace, int diceCount = 1, int diceBonusMalus = 0)
        {
            int dicesResult = diceBonusMalus;
            for (int i = 0; i < diceCount; i++)
            {
                dicesResult += _Random.Next(diceMaxFace) + 1;
            }
            return dicesResult;
        }

        public Dictionary<int, int> DiceValues(int diceMaxFace)
        {
            var dices = new Dictionary<int, int>();
            for(int i = 1; i < diceMaxFace + 1; i++)
            {
                dices[i] = 1;
            }
            return dices;
        }

        public Dictionary<int, int> DicesValues(int diceMaxFace, int diceCount)
        {
            if(diceCount == 1)
            {
                return DiceValues(diceMaxFace);
            }
            else
            {
                return AddDicesValues(DiceValues(diceMaxFace), DicesValues(diceMaxFace, diceCount - 1));
            }
        }

        public Dictionary<int, int> AddDicesValues(Dictionary<int, int> dice1, Dictionary<int, int> dice2)
        {
            var dices = new Dictionary<int, int>();
            foreach (var kv1 in dice1)
            {
                foreach (var kv2 in dice2)
                {
                    var key = kv1.Key + kv2.Key;
                    var value = kv1.Value + kv2.Value;
                    if (!dices.ContainsKey(key))
                    {
                        dices[key] = value;
                    }
                    else
                    {
                        dices[key] += value;
                    }
                }
            }
            return dices;
        }

        //public List<List<int>> AllDices(int diceMaxFace, int diceMaxCount)
        //{
        //    if(diceMaxCount == 1)
        //    {
        //        return new List<List<int>>() { DiceValues(diceMaxFace).Values.ToList() };
        //    }
        //    else
        //    {
        //        return new List<List<int>>()
        //        {
        //            DiceValues(diceMaxFace).Values.ToList(),
        //            AllDices(diceMaxFace, diceMaxCount - 1)
        //        };
        //    }
        //}

    }
}
