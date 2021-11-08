using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catemon
{
    public class Position
    {
        public int width { get; set; }
        public int height { get; set; }
        public Position(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public Position(Position position)
        {
            this.width = position.width;
            this.height = position.height;
        }
    }
    public class Player
    {
        public string gender { get; }
        public string name { get; }
        public List<Cat> cats { get; }
        public Position position { get; set; }
        public readonly char model = '@';
        public int usedCat { get; set; }

        public Player(string gender, string name, Cat cat)
        {
            this.gender = gender;
            this.name = name;
            cats = new();
            addCat(cat);
            this.position = new(34, 12);

        }

        public void addCat(Cat cat)
        {
            Cat newCat = new(cat);
            this.cats.Add(newCat);
        }
    }
}
