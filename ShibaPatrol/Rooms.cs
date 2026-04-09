using ShibaPatrol;
using static ShibaStats;
using static TextEffects;
using static ShibaPatrol.Patrol;
using static ShibaPatrol.MusicPlayer;

namespace ShibaPatrol
{
    public class Room
    {
        //Core Room Properties
        public string ?Name { get; set;} //Name of the room.
        public bool RoomCleared { get; set; } //Determines if the room is clear or not.
        public bool MischiefDone { get; set; } //Checks if the player has performed a mischief event.
        public Action<ShibaStats> ?MischiefEvent { get; set; } //The mischief event that plays.

        // Encounter Data
        public string[] ?RoomEncounter { get; set; } //A random list of threats that is selected during room creation.
        public int[] ?EncounterThreat { get; set; }  //The 'Enemy Health'; when an encounter is randomly selected, the corresponding threat index is selected as well.
        
        // Navigation
        public int BedroomDistance { get; set; } //How far away the bedroom is from the player. When they select "Return to the Bedroom", this is the number of minutes it takes to return.
        public List<Room> AllRooms { get; set; }  //Used for the map.

        // Actions
        public Action<ShibaStats> ActionA { get; set; }
        public Action<ShibaStats> ActionB { get; set; }
        public Action<ShibaStats> ActionC { get; set; }
        public Action<ShibaStats> ActionD { get; set; }

        public string SelectionA { get; set; }
        public string SelectionB { get; set; }
        public string SelectionC { get; set;}
        public string SelectionD { get; set; }
        
        public Room Enter(ShibaStats stats)
        {
            while (Name != "Game Over")
            {
                Console.Clear();
                PoppyStats(stats);
                if (stats.Time <= 0)
                {
                    OutOfTime(stats);
                }

                MapDisplay(AllRooms, this);
                Typewriter($"{stats.ShibaName} is currently in the {Name}.\n");
                
                if (Name != "Bedroom")
                {
                    if (!RoomCleared)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("===Room Not Secured===");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("===Room Secured===");
                        Console.ForegroundColor = ConsoleColor.White;

                        if (!MischiefDone)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Poppy has not caused trouble in this room.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Poppy has done something bad! There will be consequences...");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }

                if (SelectionA != null) Console.WriteLine(SelectionA);
                if (SelectionB != null) Console.WriteLine(SelectionB);
                if (SelectionC != null) Console.WriteLine(SelectionC);
                if (SelectionD != null) Console.WriteLine(SelectionD);

                string input = Console.ReadLine();
                while (!IsValidInput(input))
                {
                    TextEffects.Typewriter("That is not a valid action.");
                    input = Console.ReadLine();
                }

                var previousRoom = this;
            
                ActionControl(input, stats);

                if (Patrol.currentRoom != previousRoom)
                {
                    return Patrol.currentRoom;
                }
            }
            return this;
        }

        public static void MapDisplay(List<Room> allRooms, Room currentRoom)
        {
            foreach (var room in allRooms)
            {
                if (room == currentRoom)
                    Console.ForegroundColor = ConsoleColor.Blue;
                
                Console.Write($"[{room.Name}]");
                Console.ForegroundColor = ConsoleColor.White;
                
                if (room != allRooms.Last())
                    Console.Write("--");
            }
            Console.WriteLine("\n");
        }//Map Method
        public void Investigate(ShibaStats stats)
        {
            if (!RoomCleared)
            {
                //Generate the threat in the room
                Random roll = new Random();
                int index = roll.Next(RoomEncounter.Length);
                string enemy = RoomEncounter[index];
                int threat = EncounterThreat[index];

                //Announce the threat and begin the encounter
                if (enemy == "Mothman")
                {
                    Typewriter("The Mothman is on the front porch! He's certainly a long way from home...\n");
                    Continue();
                    Typewriter("No matter- this cannot stand. Mothman- prepare for a butt-slamming!\n");
                }
                else
                {
                    Typewriter($"There is a {enemy} here! This is unacceptable!\n");
                }
                        Continue();
                        var result = Encounter(stats, enemy, threat);
                        stats.Energy = result.stats.Energy;
                        stats.Bravery = result.stats.Bravery;  
                        stats.Security = result.stats.Security;
                        stats.Time = result.stats.Time;
                        threat = result.threat;
                        
                        if (threat <= 0)
                        {
                            stats.Security ++;
                            RoomCleared = true;
                        }
                        return;
            }
            else if (!MischiefDone)
            {
                MusicStop();
                Play("Music\\Mischief.mp3");
                MischiefEvent?.Invoke(stats);
            }
            else
            {
                TextEffects.Typewriter("Poppy has already cleared this room. There's nothing more to do here.\n");
                TextEffects.Continue();
                return;
            }
        }//Investiate Method
        public void ActionControl(string input, ShibaStats stats)
        {
            switch (input.ToUpper())
            {
                case "A":
                    ActionA?.Invoke(stats);
                    break;
                case "B":
                    ActionB?.Invoke(stats);
                    break;
                case "C":
                    ActionC?.Invoke(stats);
                    break;
                case "D":
                    ActionD?.Invoke(stats);
                    break;
                default:
                    Typewriter("That is  not a valid action.\n");
                    break;
            }
        }
        private bool IsValidInput(string input)
        {
            return (input.ToUpper() == "A" && ActionA != null) ||
                   (input.ToUpper() == "B" && ActionB != null) ||
                   (input.ToUpper() == "C" && ActionC != null) ||
                   (input.ToUpper() == "D" && ActionD != null);
        }
        public void Leave (ShibaStats stats, string room)
        {
                Typewriter($"Poppy trots into the {room}.\n");
                Continue();
        }
        

    }//Room Class

}//Namespace ShibaPatrol