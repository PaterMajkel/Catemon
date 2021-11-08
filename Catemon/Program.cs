using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Catemon
{
    public class Mobs
    {
        public Player player { get; set; }
        public List<Cat> cats { get; set; }
        public Mobs(Player player)
        {
            this.player = player;
            cats = new();
        }
        public void AddCat(Cat cat)
        {
            cats.Add(cat);
        }

    }
    public class Program
    {
        static char direction;
        public static int change = 0;
        public static string gender = "boy";
        public static string name = "";
        public static string[] map = Assets.Map();
        public static int running = 0;
        static void Main(string[] args)
        {
            Timer _timerPlayer = null;
            Timer _timerEnemies = null;
            Console.SetWindowSize(100, 30);
            Console.SetBufferSize(100, 30);
            Console.OutputEncoding = Encoding.Unicode;
            TitleScreen();
            profOak();
            string [][] catsToChoose = { Assets.Cat1(), Assets.Cat2(), Assets.Cat3() };
            int chosenCat = catScreen(catsToChoose);
            string[][] cats = { Assets.Cat1(), Assets.Cat2(), Assets.Cat3(), Assets.Cat4(), Assets.Cat5() };
            profOak2();
            Mobs mobs = new(new Player(gender, name, new Cat(cats[chosenCat])));
            for (int i = 0; i < 5; i++)
            {
                if (i == chosenCat)
                    continue;
                mobs.AddCat(new Cat(cats[i]));
            }
            Draw(map);
            instructions();
            Console.CursorVisible = false;
            _timerPlayer = new Timer(TimerCallback, null, 0, 23);
            _timerEnemies = new Timer(TimerCallbackEnemies, null, 0, 750);

            while (true)
            {
                if(mobs.cats.Count==0)
                { endScreen();
                    return;
                }
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    break;
                switch(key.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            direction = 'w';
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            direction = 's';
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            direction = 'a';
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            direction = 'd';
                            break;
                        }
                    case ConsoleKey.I:
                        {
                            string[][] catsInventory= new string[mobs.player.cats.Count][];
                            for (int i = 0; i < mobs.player.cats.Count; i++)
                            {
                                catsInventory[i] = mobs.player.cats[i].GetAsset();
                            }
                            _timerPlayer.Dispose(); _timerEnemies.Dispose();
                            _timerEnemies.Dispose(); _timerEnemies.Dispose();
                            mobs.player.usedCat=catScreen(catsInventory,mobs.player.cats.ToArray());
                            Console.Clear();
                            Draw(map);
                            instructions();
                            _timerPlayer = new Timer(TimerCallback, null, 0, 23);
                            _timerEnemies = new Timer(TimerCallbackEnemies, null, 0, 750);
                            break;
                        }
                    default:
                        {
                            direction = key.KeyChar;
                            break;
                        }
                }
            }


            void TimerCallback(Object o)
            {
                if (running == 1)
                    return;
                char[] directions = new char[] { 'w', 'a', 's', 'd' };
                char[] forbidenChars = new char[] { '_', '|', '/', '\\', '.', '\'' };
                Position position = mobs.player.position;
                var currentPosition = new Position(position);
                if (Array.Exists(directions, dir => dir == direction))
                {
                    switch (direction)
                    {
                        case 's':
                            {
                                if (Array.Exists(forbidenChars, fc => fc == map[position.height + 1][position.width]))
                                    return;
                                position.height += 1;
                                break;
                            }
                        case 'w':
                            {
                                if (position.height - 1 < 2 || Array.Exists(forbidenChars, fc => fc == map[position.height - 1][position.width]))
                                    return;
                                position.height -= 1;
                                break;
                            }
                        case 'd':
                            {
                                if (Array.Exists(forbidenChars, fc => fc == map[position.height][position.width + 1]))
                                    return;
                                position.width += 1;
                                break;
                            }
                        case 'a':
                            {
                                if (position.width - 1 < 0 || Array.Exists(forbidenChars, fc => fc == map[position.height][position.width - 1]))
                                    return;
                                position.width -= 1;
                                break;
                            }
                        
                    }
                    Console.SetCursorPosition(currentPosition.width, currentPosition.height);
                    Console.Write(map[currentPosition.height][currentPosition.width]);
                    Console.SetCursorPosition(position.width, position.height);
                    if (mobs.player.gender == "BOY")
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write('@');
                    Console.ForegroundColor = ConsoleColor.White;
                    direction = 'x';
                }
            }
            void TimerCallbackEnemies(object o)
            {
                running = 1;
                foreach (var x in mobs.cats)
                {
                    char[] forbidenChars = new char[] { '_', '|', '/', '\\', '.', '\'' };
                    Position position = x.position;
                    var currentPosition = new Position(position);
                    if (position.height == mobs.player.position.height && position.width == mobs.player.position.width)
                    { _timerPlayer.Dispose(); _timerEnemies.Dispose();
                        transition();
                        fightScreen(x);
                        transition();
                        Console.Clear();
                        Draw(map);
                        instructions();
                        _timerPlayer = new Timer(TimerCallback, null, 0, 23);
                        _timerEnemies = new Timer(TimerCallbackEnemies, null, 0, 750);
                        return; }
                    switch (x.steps[x.currentStep])
                    {
                        case 0:
                            {
                                if (Array.Exists(forbidenChars, fc => fc == map[position.height + 1][position.width]))
                                    break;
                                position.height += 1;
                                break;
                            }
                        case 1:
                            {
                                if (position.height - 1 < 2 || Array.Exists(forbidenChars, fc => fc == map[position.height - 1][position.width]))
                                    break;
                                position.height -= 1;
                                break;
                            }
                        case 2:
                            {
                                if (Array.Exists(forbidenChars, fc => fc == map[position.height][position.width + 1]))
                                    break;
                                position.width += 1;
                                break;
                            }
                        case 3:
                            {
                                if (position.width - 1 < 0 || Array.Exists(forbidenChars, fc => fc == map[position.height][position.width - 1]))
                                    break;
                                position.width -= 1;
                                break;
                            }
                    }


                    Console.SetCursorPosition(currentPosition.width, currentPosition.height);
                    Console.Write(map[currentPosition.height][currentPosition.width]);
                    Console.SetCursorPosition(x.position.width, x.position.height);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write('#');
                    Console.ForegroundColor = ConsoleColor.White;
                    x.NextStep();
                }
                running = 0;
            }
            void fightScreen(Cat foughtCat)
            {
                Cat playerCat = mobs.player.cats[mobs.player.usedCat];
                string[][] options = { Assets.Attack(), Assets.Defend(), Assets.Catch() };
                int currentOption = 0;
                Redraw(true, true, false, false);
                var direction = Console.ReadKey(true);
                while (foughtCat.HP > 0)
                {
                    if (playerCat.HP <= 0)
                    {
                        mobs.player.position.width = 34;
                        mobs.player.position.height = 12;
                        playerCat.HP = playerCat.maxHP;
                        Console.Clear();
                        Console.SetCursorPosition(0, 10);
                        Draw(Assets.loseScreen(), 2);
                        Thread.Sleep(4000);
                        return;
                    }
                    direction = Console.ReadKey(true);
                    if (direction.Key == ConsoleKey.RightArrow)
                    {
                        currentOption++;
                        for(int i = 2; i >= 0; i--)
                        {

                            Console.SetCursorPosition(0, 22);

                            if (i == currentOption % 3)
                                Console.ForegroundColor = ConsoleColor.Green;
                            Draw(options[i], 3 + (i * 2));
                            Console.ForegroundColor = ConsoleColor.White;

                        }

                    }
                    if (direction.Key == ConsoleKey.LeftArrow)
                    {
                        currentOption+=2;
                        for (int i = 2; i >= 0; i--)
                            {
                        Console.SetCursorPosition(0, 22);
                            if (i == currentOption % 3)
                                Console.ForegroundColor = ConsoleColor.Green;
                            Draw(options[i], 3 + (i * 2));
                            Console.ForegroundColor = ConsoleColor.White;

                        }

                    }
                    if(direction.Key == ConsoleKey.Enter)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.Write($"Your turn, {mobs.player.name}");
                        switch (currentOption%3)
                        {
                            case 0:
                                {
                                    Redraw(true, true, true, false);
                                    Thread.Sleep(500);

                                    if (playerCat.Attack(foughtCat))
                                    {
                                        for (int i = 0; i < 5; i++)
                                        {
                                            Redraw(true, i%2==0, false, false);
                                            Thread.Sleep(250);
                                            Console.SetCursorPosition(30, 8);
                                            Console.Write("NICE!      ");
                                        }

                                    }
                                    else
                                    {
                                        Redraw(true, false, false, false);
                                        Thread.Sleep(250);
                                        Redraw(true, true, false, false);

                                        Console.SetCursorPosition(30, 8);
                                        Console.Write("MISSED!     ");
                                    }
                                        
                                    break;
                                }
                            case 1:
                                {
                                    Redraw(false, true, false, false);
                                    Thread.Sleep(250);
                                    Redraw(true, true, false, false);

                                    Console.SetCursorPosition(30, 8);
                                    Console.Write("DEFENDING!");
                                    playerCat.Defend();
                                    break;
                                }
                            case 2:
                                {
                                    if (foughtCat.Catch())
                                    {
                                        Random random = new();
                                        int j = random.Next(4, 10);
                                        for (int i = 0; i < j; i++)
                                        {
                                            Redraw(true, i % 2 == 0, false, false);
                                            Thread.Sleep(250);
                                        }
                                        Redraw(true, true, false, false);
                                            Console.SetCursorPosition(30, 8);

                                        Console.Write("COUGHT!   ");
                                        Thread.Sleep(4000);
                                        foughtCat.HP = 0;
                                    }
                                    else
                                    {
                                        Random random = new();
                                        int j = random.Next(4, 10);
                                        for (int i = 0; i < j; i++)
                                        { 
                                            Redraw(true, i % 2 == 0, false, false);
                                            Thread.Sleep(250);
                                        }
                                        Redraw(true, true, false, false);
                                            Console.SetCursorPosition(30, 8);

                                        Console.Write("ESCAPED!    ");
                                        Thread.Sleep(3000);
                                    }
                                    break;
                                }
                        }
                        foughtCat.Defend(true);
                        Random rand = new Random();
                        int enemiesOption = rand.Next(100);
                        Console.SetCursorPosition(0, 0);
                        if (foughtCat.HP <= 0)
                            break;
                        Console.Write("Enemy's turn                 ");
                        Thread.Sleep(3000);
                        switch (enemiesOption)
                        {
                            case <80:
                                {
                                    Redraw(true, true, false, true );
                                    Thread.Sleep(500);

                                    if (foughtCat.Attack(playerCat))
                                    {
                                        for (int i = 0; i < 5; i++)
                                        {
                                            Redraw(i % 2 == 0, true, false, false);
                                            Thread.Sleep(250);
                                            Console.SetCursorPosition(30, 8);
                                            Console.Write("OUCH!      ");
                                        }

                                    }
                                    else
                                    {
                                        Redraw(false, true, false, false);
                                        Thread.Sleep(250);
                                        Redraw(true, true, false, false);

                                        Console.SetCursorPosition(30, 8);
                                        Console.Write("MISSED YOU!   ");
                                    }

                                    break;
                                }
                            case >=80:
                                {
                                    Redraw(true, false, false, false);
                                    Thread.Sleep(250);
                                    Redraw(true, true, false, false);

                                    Console.SetCursorPosition(30, 8);
                                    Console.Write("DEFENDING!");
                                    foughtCat.Defend();
                                    break;
                                }
                        }

                        playerCat.Defend(true);

                    }
                }
                Console.Clear();
                Console.SetCursorPosition(0, 10);
                Draw(Assets.winScreen(), 2);
                Thread.Sleep(4000);
                mobs.player.addCat(new Cat(foughtCat));
                playerCat.HP = playerCat.maxHP;
                mobs.cats.Remove(foughtCat);

                void Redraw(bool first, bool second, bool move1, bool move2)
                {
                    Console.Clear();
                    if(second)
                    {
                        Console.SetCursorPosition(0, 3);
                        int moved = 6;
                        if (move2)
                            moved = 5;
                        Draw(foughtCat.GetAsset(), moved);
                        Console.WriteLine();
                        Draw(String.Format("Enemy Cat's Health Points: " + foughtCat.HP.ToString()), 6);

                    }
                    if(first)
                    {
                    Console.SetCursorPosition(0, 8);
                        int moved = 0;
                        if (move1)
                            moved = 1;
                        Draw(playerCat.GetAsset(),moved);
                    Console.WriteLine();
                    Draw(String.Format("Your Cat's Health Points: " + playerCat.HP.ToString()));

                    }
                    Console.SetCursorPosition(0, 28);
                    Console.WriteLine("Use arrow keys to navigate\nPress enter to choose");
                    for (int i = 2; i >= 0; i--)
                    {
                        Console.SetCursorPosition(0, 22);
                        if (i == currentOption%3)
                            Console.ForegroundColor = ConsoleColor.Green;
                        Draw(options[i], 3 + (i * 2));
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                }

            }
        }
       
        private static void TimerCallback2(Object o)
        {
            string[] calls = { "Press any key to continue", "                         " };
            Console.SetCursorPosition(30, 20);
            Console.WriteLine(calls[(change++)%2]);


        }

        public static void TitleScreen()
        {
            Console.SetCursorPosition(0, 5);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(Assets.Title());
            Console.ForegroundColor = ConsoleColor.White;

            Timer _timer = new Timer(TimerCallback2, null, 0, 1000);
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.RightWindows)
                    break;
            }
            _timer.Dispose();
            Console.Clear();

        }

        public static void profOak()
        {
            Console.SetCursorPosition(0, 3);
            Console.WriteLine(Assets.Doc());
            string[] genders = new string[] { "BOY", "GIRL" };
            Console.SetCursorPosition(0, 26);
            Console.WriteLine("Are you a BOY, or are you a GIRL");
            gender = Console.ReadLine().ToUpper();
            while (true)
            {
                if (Array.Exists(genders, sex => sex == gender))
                    break;
                Console.SetCursorPosition(0, 26);
                Console.WriteLine("Please answer! Are you a BOY, or are you a GIRL");
                Console.WriteLine("                                                ");
                Console.SetCursorPosition(0, 27);
                gender = Console.ReadLine().ToUpper();
            }

            Console.SetCursorPosition(0, 26);
            Console.WriteLine("                                                ");
            Console.SetCursorPosition(0, 26);
            Console.WriteLine("What is your name?");
            Console.WriteLine("                                                ");
            Console.SetCursorPosition(0, 27);
            name = Console.ReadLine();
            while(name=="")
            {
                Console.SetCursorPosition(0, 26);
                Console.WriteLine("You surely must have a name!");
                Console.WriteLine("                                                ");
                Console.SetCursorPosition(0, 27);
                name = Console.ReadLine();
            }

            Console.SetCursorPosition(0, 26);
            Console.WriteLine("                                                ");
            Console.SetCursorPosition(0, 26);
            Console.WriteLine($"Now is the time to choose your first cat, {name}");
            Console.WriteLine("                                                ");
            Console.SetCursorPosition(0, 27);
            Thread.Sleep(1000);
            Console.WriteLine("Enter-->");
            Console.ReadLine();

            Console.Clear();
            
        }
        public static void profOak2()
        {
            Console.SetCursorPosition(0, 3);
            Console.WriteLine(Assets.Doc());
            Console.SetCursorPosition(0, 26);
            Console.WriteLine("Now, you'll begin your adventure!");
            Console.WriteLine("                                                ");
            Console.SetCursorPosition(0, 27);
            Thread.Sleep(500);
            Console.WriteLine("Enter-->");
            Console.ReadLine();

            Console.SetCursorPosition(0, 26);
            Console.WriteLine("Go around the field and catch all of the other cats!");
            Console.WriteLine("                                                ");
            Console.SetCursorPosition(0, 27);
            Thread.Sleep(500);
            Console.WriteLine("Enter-->");
            Console.ReadLine();

            Console.Clear();
        }

        public static int catScreen(string[][] cats, Cat[] Catlist = null)
        {
            int currentCat = 0;
            Console.Clear();
            Console.SetCursorPosition(0, 26);
            Console.WriteLine("\t\tUse arrow keys to navigate\n\t\tPress enter to choose");
            Console.SetCursorPosition(0, 6);
            Draw(cats[0],2);
            if (Catlist != null)
            {
                Console.WriteLine($"\n\tCat's HP: {Catlist[(currentCat) % cats.Length].HP}");
                Console.WriteLine($"\n\tCat's Damage: {Catlist[(currentCat) % cats.Length].AD}");
                Console.WriteLine($"\n\tCat's Dodge Chance: {Catlist[(currentCat) % cats.Length].dodgeChance}");
            }
            var direction = Console.ReadKey(true);
            while (direction.Key!=ConsoleKey.Enter)
            {
                direction = Console.ReadKey(true);
                if (direction.Key == ConsoleKey.RightArrow)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 26);
                    Console.WriteLine("\t\tUse arrow keys to navigate\n\t\tPress enter to choose");
                    Console.SetCursorPosition(0, 6);
                    Draw(cats[(++currentCat)%cats.Length],2);
                    if (Catlist != null)
                    {
                        Console.WriteLine($"\n\tCat's HP: {Catlist[(currentCat) % cats.Length].HP}");
                        Console.WriteLine($"\n\tCat's Damage: {Catlist[(currentCat) % cats.Length].AD}");
                        Console.WriteLine($"\n\tCat's Dodge Chance: {Catlist[(currentCat) % cats.Length].dodgeChance}");
                    }

                }
                if (direction.Key == ConsoleKey.LeftArrow)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 26);
                    Console.WriteLine("\t\tUse arrow keys to navigate\n\t\tPress enter to choose");
                    Console.SetCursorPosition(0, 6);
                    Draw(cats[(currentCat+=cats.Length-1) % cats.Length],2);
                    if (Catlist != null)
                    {
                        Console.WriteLine($"\n\tCat's HP: {Catlist[(currentCat) % cats.Length].HP}");
                        Console.WriteLine($"\n\tCat's Damage: {Catlist[(currentCat) % cats.Length].AD}");
                        Console.WriteLine($"\n\tCat's Dodge Chance: {Catlist[(currentCat) % cats.Length].dodgeChance}");
                    }
                }
            }
            Console.Clear();
            return currentCat% (cats.Length);
        }
        public static void instructions()
        {
            Console.SetCursorPosition(10, 28);
            Console.Write("Move: [<,^,>,v] or [w,s,a,d]          Inventory: [I]");
        }
        public static void Draw(string[] element, int tabCount = 0)
        {
            foreach(var x in element)
                {
                for (int i = 0; i < tabCount; i++)
                {
                    Console.Write("\t");
                }
                Console.WriteLine(x);
            }
        }
        public static void Draw(string element, int tabCount = 0)
        {

            for (int i = 0; i < tabCount; i++)
            {
                Console.Write("\t");
            }
            Console.WriteLine(element);
            
        }

        public static void transition()
        {
            Console.Clear();
            for(int i=0; i<32; i++)
            { Console.WriteLine("####################################################################################################");
                Thread.Sleep(50);
            }
        }
        public static void endScreen()
        {
            transition();
            Console.Clear();
            Console.SetCursorPosition(0, 5);
            Draw(Assets.wonGame(), 2);
            Thread.Sleep(5000);
            transition();
        }
    }
}
