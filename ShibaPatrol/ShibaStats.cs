public class ShibaStats
{
            //Stat Limiters
            private int _energy;
            private int _energyMax;
            private int _bravery;
            private int _braveryMax;
            private int _security;
            private int _time;

            //Stat Control
            public string ShibaName  { get; set; }
             public int EnergyMax 
            {
                get {return _energyMax;} 
                set {_energyMax = value;}
            }  
            public int Energy           //Poppy spends energy to perform certain tasks.
            {
                get {return _energy; } 
                set
                {
                    if (value < 0)
                        _energy = 0;
                    else if (value > _energyMax)
                        _energy = _energyMax;
                    else
                        _energy = value;
                }
            }       
            public int BraveryMax 
            {
                get {return _braveryMax;} 
                set {_braveryMax = value;}
            } 
            public int Bravery          //Acts as health. Poppy must use bravery to confront threats.
            {             
                get {return _bravery; } 
                set
                {
                    if (value < 0)
                        _bravery = 0;
                    else if (value > _braveryMax)
                        _bravery = _braveryMax;
                    else
                        _bravery = value;
                }
            }      
            public int Security         //Poppy needs to feel the house is secure enough.
            {
                get {return _security; } 
                set { _security = Math.Max(0, value); }
            }     
            public int Time             //Poppy's remaining patrol time.
            {
                get {return _time; } 
                set{ _time = Math.Max(0, value); }
            }    
            public int Mischief         //The trouble that Poppy can get into throughout the house. Affects final score.
            { 
                get; 
                set; 
            }
            public int Power            //Increases damage.
            {
                get;
                set;
            }
            public int XP               //Used to gain levels and points.
            { 
                get; 
                set; 
            } 
            public int XPMax            //XP Needed to level.
            { 
                get; 
                set; 
            } 
            public int Level            //Poppy's current level; increases when XP reaches XPMax.
            { 
                get; 
                set; 
            } 
            public int Points           //Used for increasing stats.
            { 
                get; 
                set; 
            }
        public static void PoppyStats(ShibaStats stats)                             //Displays all of Poppy's stats.
        {
            Console.WriteLine($"=====POPPY=====\t\tLevel {stats.Level}");
            Console.Write($"❤️  Bravery: {stats.Bravery,2} / {stats.BraveryMax,-3}   |");
            FilledGauge(stats.Bravery, ConsoleColor.Green);
            EmptyGauge(stats.Bravery, stats.BraveryMax, ConsoleColor.Green);
            Console.Write("|".PadRight(30-stats.BraveryMax));

            Console.Write($"🛡️  Security: {stats.Security,2} / 10 |");
            FilledGauge(stats.Security, ConsoleColor.Magenta);
            EmptyGauge(stats.Security, 10, ConsoleColor.Magenta);
            Console.WriteLine("|");
            
            Console.Write($"⚡ Energy:  {stats.Energy,2} / {stats.EnergyMax,-3}   |");
            FilledGauge(stats.Energy, ConsoleColor.Cyan);
            EmptyGauge(stats.Energy, stats.EnergyMax, ConsoleColor.Cyan);
            Console.Write("|".PadRight(30-stats.EnergyMax));

            Console.Write($"⏳ Time:    {stats.Time,3} Min. ");
            Console.Write("|");
            FilledGauge(stats.Time/12, ConsoleColor.White);
            EmptyGauge(stats.Time/12, 10, ConsoleColor.White);
            Console.WriteLine("|");

            Console.WriteLine("-------------------------------------------------------------------------");     //Dividing line from the stats.
        }
        public static void FilledGauge(int x, ConsoleColor color)                                                  //For filled gauges.
        {
            for (int eGauge = 0; eGauge < x; eGauge++)
            {
                Console.ForegroundColor = color;
                Console.Write("■");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public static void EmptyGauge(int x, int y, ConsoleColor color)                                            //For empty gauges.
        {
                int emptyGauge = y - x;
            for (int eGauge = 0; eGauge < emptyGauge; eGauge++)
            {
                Console.ForegroundColor = color;    
                Console.Write("-");
                Console.ForegroundColor = ConsoleColor.White;
                
            }
        }
}