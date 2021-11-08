using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catemon
{
    public class Cat
    {
        public int maxHP { get; }
        public int HP { get; set; }
        public int AD { get; }

        public int dodgeChance { get; set; }

        private readonly string[] asset;

        public Position position { get; set; }
        public List<int> steps { get; }
        public int currentStep { get; set; }
        public bool defendState { get; set; }
        public string[] GetAsset()
        {
            return asset;
        }

        public Cat(string[] asset)
        {
            this.asset = asset;
            Random random = new Random();
            this.HP = random.Next(40, 100);
            this.maxHP = this.HP;
            this.AD = random.Next(10, 30);
            this.dodgeChance = random.Next(1, 60);
            int i = 0;

            i = random.Next(1, 96);


            int j = 0;
            while (true)
            {
                j = random.Next(0, 24);
                if ((j > 2 && j < 25) && (j<4 || j>12))
                    break;
            }
            this.position = new(i, j);
            steps = new();
            for (int z = 0; z < 4; z++)
            {
                steps.Add(random.Next(0, 4));
            }
            for (int z = 3; z >= 0; z--)
            {
                steps.Add(steps[z]);
            }
            this.currentStep = 0;
        }
        public Cat(Cat cat)
        {
            this.asset = cat.asset;
            this.HP = cat.maxHP;
            this.maxHP = cat.maxHP;
            this.AD = cat.AD;
            this.dodgeChance = cat.dodgeChance;
        }



        public bool Attack(Cat cat)
        {
            Random random = new Random();
            if (random.Next(100) > cat.dodgeChance)
            {
                cat.HP -= AD;
                return true;
            }
            return false;
        }

        public void Defend(bool disable=false)
        {
            if(defendState == true)
            {
                defendState = false;
                dodgeChance -= 30;
                return;
            }
            if (disable)
                return;
            defendState = true;
            dodgeChance +=30;

        }
        public bool Catch()
        {
            Random random = new Random();
            if (random.Next(maxHP) > HP)
                return true;

            return false;
        }

        public void NextStep()
        {
           this.currentStep=(++this.currentStep)%8;
        }
    }
}
