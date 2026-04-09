using System.Security.Cryptography;
using static TextEffects;
using static ShibaPatrol.Room;
using static ShibaStats;
using static ShibaPatrol.Helpers;
using static ShibaPatrol.MusicPlayer;
using System.Transactions;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ShibaPatrol
{
class Patrol
{
        static Room bedroom;
        static bool bathtime = false;           //Special flag for the Alley mischief event
        public static Room currentRoom;         //Handles room control
        public static bool GameOver = false;    //Used to stop the game loop whenever OutOfTime method is triggered.

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //Introduction
            Console.Clear();
            Play("Music\\Explore.mp3");
            Typewriter("It's nighttime. \nPoppy is on patrol in and around the house for intruders. \nOr... at least things she perceives as intruders. \n");
            Continue();
            Typewriter("Poppy has 2 hours to make her patrol around the house."); 
            Console.WriteLine("");
            Continue();
            
            Console.Clear();
    
            //Stats
            ShibaStats stats = new ShibaStats();

            stats.ShibaName = "Poppy";
            stats.BraveryMax = 10;       //Acts as health. Poppy must use bravery to confront threats.
            stats.Bravery = 10;
            stats.EnergyMax = 10;        //Poppy spends energy to perform certain tasks.
            stats.Energy = 10;
            stats.Security = 0;          //Poppy needs to feel the house is secure enough.
            stats.Time = 120;            //Poppy's remaining patrol time. 
            stats.Power = 0;

            stats.XP = 0;
            stats.XPMax = 10;
            stats.Level = 1;
            stats.Points = 0;

            #region Rooms
            bedroom = new Room 
                {
                    Name = "Bedroom",
                    SelectionA = "A) Move to the Entryway.",
                    SelectionB = "B) Cuddle with Vivian.",
                    SelectionC = "C) End the patrol.",
                };
 
            var entryway = new Room 
                {
                    Name = "Entryway",
                    RoomEncounter = new[] {"Crunchy Leaf", "Letter", "Dirty Shoe", "Suspicious Umbrella", "Grandfather Clock"},
                    EncounterThreat = new[] {5, 6, 7, 8, 9},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Living Room.",
                    SelectionC = "C) Return to the Bedroom.",
                    BedroomDistance = 0,
                };
            
            var livingRoom = new Room 
                {
                    Name = "Living Room",
                    RoomEncounter = new[] {"Chair", "Green Spiky Bone", "Mousetrap", "Vase", "Shadow"},
                    EncounterThreat = new[] {10, 11, 12, 13, 14},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Dining Room.",
                    SelectionC = "C) Move to the Entryway.",
                    SelectionD = "D) Return to the Bedroom.",
                    BedroomDistance = 0,
                };

            var diningRoom = new Room 
                {
                    Name = "Dining Room",
                    RoomEncounter = new[] {"Bar Stool", "Cleaning Rack", "Bookshelf", "Mouse", "Dark Lord Vakoom"},
                    EncounterThreat = new[] {15, 16, 17, 18, 19},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Kitchen.",
                    SelectionC = "C) Move to the Living Room.",
                    SelectionD = "D) Return to the Bedroom.",
                    BedroomDistance = 1,
                };

            var kitchen = new Room 
                {
                    Name = "Kitchen",
                    RoomEncounter = new[] {"Spoon", "Mop Bucket", "Blender", "Trash Can", "Dishwasher"},
                    EncounterThreat = new[] {20, 21, 22, 23, 24},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Back Yard.",
                    SelectionC = "C) Move to the Dining Room.",
                    SelectionD = "D) Return to the Bedroom.",
                    BedroomDistance = 1,
                };

            var backyard = new Room 
                {
                    Name = "Back Yard",
                    RoomEncounter = new[] {"Branch", "Squirrel", "Grill", "Cat", "Possum"},
                    EncounterThreat = new[] {25, 26, 27, 28, 30},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Alley.",
                    SelectionC = "C) Move to the Kitchen.",
                    SelectionD = "D) Return to the Bedroom.",
                    BedroomDistance = 2,
                };

            var alley = new Room 
                {
                    Name = "Alley",
                    RoomEncounter = new[] {"Dumpster", "Raccoon", "Fence", "Garage", "Backstreet Boy"},
                    EncounterThreat = new[] {31, 32, 33, 34, 35},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Side Street.",
                    SelectionC = "C) Move to the Back Yard.",
                    SelectionD = "D) Return to the Bedroom.",
                    BedroomDistance = 3,
                };

            var sideStreet = new Room 
                {
                    Name = "Side Street",
                    RoomEncounter = new[] {"Pile of Trash", "Fire Hydrant", "Group of Squirrels", "Discarded Bottle", "Stray Dog"},
                    EncounterThreat = new[] {36,  37, 38, 39, 40},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Main Street.",
                    SelectionC = "C) Move to the Alley.",
                    SelectionD = "D) Return to the Bedroom. ",
                    BedroomDistance = 4,
                };

            var mainStreet = new Room 
                {
                    Name = "Main Street",
                    RoomEncounter = new[] {"Garden Gnome", "Flower", "Mailbox", "Parked Car", "Pedestrian"},
                    EncounterThreat = new[] {41, 42, 43, 44, 45},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Front Yard.",
                    SelectionC = "C) Move to the Side Street.",
                    SelectionD = "D) Return to the Bedroom. ",
                    BedroomDistance = 5,
                };

            var frontYard = new Room 
                {
                    Name = "Front Yard",
                    RoomEncounter = new[] {"Bird Bath", "Flock of Birds", "Eastern Redbud", "Mail Box", "Fake Skeleton"},
                    EncounterThreat = new[] {46, 47, 48, 49, 50},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Front Porch.",
                    SelectionC = "C) Move to the Main Street. ",
                    SelectionD = "D) Return to the Bedroom.",
                    BedroomDistance = 6,
                };  
            var frontPorch = new Room 
                {
                    Name = "Porch",
                    RoomEncounter = new[] {"Mothman"},
                    EncounterThreat = new[] {60},
                    SelectionA = "A) Investigate.",
                    SelectionB = "B) Move to the Front Yard.",
                    SelectionC = "C) Return to the Bedroom. ",
                    BedroomDistance = 6,
                };           

            var gameOverRoom = new Room
            {
                    Name = "Game Over",
            };

            #endregion

            #region Map Creation
            //Assign the map to all rooms so it auto-generates the map upon entering
                var allRooms = new List<Room>
                {
                    bedroom, entryway, livingRoom, diningRoom, kitchen, backyard, alley, sideStreet, mainStreet, frontYard, frontPorch
                };
                foreach (var room in allRooms)
                    room.AllRooms = allRooms;
            #endregion

            #region Bedroom Actions
            //Retroactively assign all room actions to each room following creation
            //Note: Claude AI used to auto-complete the Kitchen through the Front Yard        
            bedroom.ActionA = (stats) => { bedroom.Leave(stats, "Entryway"); stats.Time--; currentRoom = entryway; return; };
            bedroom.ActionB = (stats) => { Cuddles(stats); };
            bedroom.ActionC = (stats) => { currentRoom = gameOverRoom; EndPatrolCheck(stats); };

            entryway.ActionA = (stats) => { entryway.Investigate(stats); };
            entryway.ActionB = (stats) => { entryway.Leave(stats, "Living Room"); stats.Time--; currentRoom = livingRoom; return; };
            entryway.ActionC = (stats) => { stats.Time--; currentRoom = bedroom; return ;};

            livingRoom.ActionA = (stats) => { livingRoom.Investigate(stats); };
            livingRoom.ActionB = (stats) => { livingRoom.Leave(stats, "Dining Room"); stats.Time--; currentRoom = diningRoom; return ; };
            livingRoom.ActionC = (stats) => { livingRoom.Leave(stats, "Entryway"); stats.Time--; currentRoom = entryway; return ; };
            livingRoom.ActionD = (stats) => { stats.Time -= livingRoom.BedroomDistance; currentRoom = bedroom; return ; };

            diningRoom.ActionA = (stats) => { diningRoom.Investigate(stats); };
            diningRoom.ActionB = (stats) => { diningRoom.Leave(stats, "Kitchen"); stats.Time--; currentRoom = kitchen; return ; };
            diningRoom.ActionC = (stats) => { diningRoom.Leave(stats, "Living Room"); stats.Time--; currentRoom = livingRoom; return ; };
            diningRoom.ActionD = (stats) => { stats.Time -= diningRoom.BedroomDistance; currentRoom = bedroom; return ; };

            kitchen.ActionA = (stats) => { kitchen.Investigate(stats); };
            kitchen.ActionB = (stats) => { kitchen.Leave(stats, "Back Yard"); stats.Time--; currentRoom = backyard; return ; };
            kitchen.ActionC = (stats) => { kitchen.Leave(stats, "Dining Room"); stats.Time--; currentRoom = diningRoom; return ; };
            kitchen.ActionD = (stats) => { stats.Time -= kitchen.BedroomDistance; currentRoom = bedroom; return ; };

            backyard.ActionA = (stats) => { backyard.Investigate(stats); };
            backyard.ActionB = (stats) => { backyard.Leave(stats, "Alley"); stats.Time--; currentRoom = alley; return ; };
            backyard.ActionC = (stats) => { backyard.Leave(stats, "Kitchen"); stats.Time--; currentRoom = kitchen; return ; };
            backyard.ActionD = (stats) => { stats.Time -= backyard.BedroomDistance; currentRoom = bedroom; return ; };

            alley.ActionA = (stats) => { alley.Investigate(stats); };
            alley.ActionB = (stats) => { alley.Leave(stats, "Side Street"); stats.Time--; currentRoom = sideStreet; return ; };
            alley.ActionC = (stats) => { alley.Leave(stats, "Back Yard"); stats.Time--; currentRoom = backyard; return ;};
            alley.ActionD = (stats) => { stats.Time -= alley.BedroomDistance; currentRoom = bedroom; return ; };

            sideStreet.ActionA = (stats) => { sideStreet.Investigate(stats); };
            sideStreet.ActionB = (stats) => { sideStreet.Leave(stats, "Main Street"); stats.Time--; currentRoom = mainStreet; return ; };
            sideStreet.ActionC = (stats) => { sideStreet.Leave(stats, "Alley"); stats.Time--; currentRoom = alley; return ; };
            sideStreet.ActionD = (stats) => { stats.Time -= sideStreet.BedroomDistance; currentRoom = bedroom; return ; };

            mainStreet.ActionA = (stats) => { mainStreet.Investigate(stats); };
            mainStreet.ActionB = (stats) => { mainStreet.Leave(stats, "Front Yard"); stats.Time--; currentRoom = frontYard; return ; };
            mainStreet.ActionC = (stats) => { mainStreet.Leave(stats, "Side Street"); stats.Time--; currentRoom = sideStreet; return ; };
            mainStreet.ActionD = (stats) => { stats.Time -= mainStreet.BedroomDistance; currentRoom = bedroom; return ; };

            frontYard.ActionA = (stats) => { frontYard.Investigate(stats); };
            frontYard.ActionB = (stats) => { frontYard.Leave(stats, "Front Porch"); stats.Time--; currentRoom = frontPorch; return ; };
            frontYard.ActionC = (stats) => { frontYard.Leave(stats, "Main Street"); stats.Time--; currentRoom = mainStreet; return ; };
            frontYard.ActionD = (stats) => { stats.Time -= frontYard.BedroomDistance; currentRoom = bedroom; return ; };

            frontPorch.ActionA = (stats) => { frontPorch.Investigate(stats); };
            frontPorch.ActionB = (stats) => { frontPorch.Leave(stats, "Main Street"); stats.Time--; currentRoom = mainStreet; return ; };
            frontPorch.ActionC = (stats) => { stats.Time -= frontPorch.BedroomDistance; currentRoom = bedroom; return ; };

            #endregion

            #region Mischief Events
            entryway.MischiefEvent = (stats) =>         //Bravery +2                +1 Mischief
            {
                Typewriter("There's a slipper in the entryway that would make a nice chew toy.\n");
                Console.WriteLine("Reward: +2 ❤️\n");
                Typewriter("Chew on the slipper?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy happily chews on the slipper.\nI'm sure the humans will be perfectly forgiving...\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.Bravery += 2;
                        stats.Mischief += 1;
                        Continue();
                        Typewriter("Poppy regains 2 Bravery.\n");
                        Continue();
                        entryway.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy resists the urge to chew the slipper.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };
            
            livingRoom.MischiefEvent = (stats) =>       //Energy +2                 +1 Mischief
            {
                Typewriter("Looks like the humans left a plate of crackers and peanut butter on the coffee table.\n");
                Console.WriteLine("Reward: +2 ⚡\n");
                Typewriter("Eat the crackers?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy munches on some peanut butter and crackers.\nI'm sure nobody will notice.\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.Energy += 2;
                        stats.Mischief += 1;
                        Continue();
                        Typewriter("Poppy regains 2 Energy.\n");
                        Continue();
                        livingRoom.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy ignores the crackers.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            diningRoom.MischiefEvent = (stats) =>       //Max Bravery +2            +2 Mischief
            {
                Typewriter("There's a bookshelf in here. Poppy likes chewing on books, it smells of her favorite human. And ink.\n");
                Console.WriteLine("Reward: Max ❤️  +2\n");
                Typewriter("Chew up a book?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy gnaws on the spine of one of the books and the corner of some pages.\nThey'll definitely notice that.\nWorth it though.\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.BraveryMax += 2;
                        stats.Bravery += 2;
                        stats.Mischief += 2;
                        Continue();
                        Typewriter("Poppy's maximum Bravery increases by 2.\n");
                        Continue();
                        diningRoom.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy resists the temptation of book munching.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            kitchen.MischiefEvent = (stats) =>          //Max Energy +2             +2 Mischief
            {
                Typewriter("Apples on the counter! Those are Poppy's favorite!\n");
                Console.WriteLine("Reward: Max ⚡ +2\n");
                Typewriter("Eat the apples?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy eats some of the apples. They probably would've just given her some had she waited.\nPatience is not a pup's virtue.\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.EnergyMax += 2;
                        stats.Energy += 2;
                        stats.Mischief += 2;
                        Continue();
                        Typewriter("Poppy's maximum Energy increases by 2.\n");
                        Continue();
                        kitchen.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy doesn't steal. She knows the humans will give her apples anyway.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            backyard.MischiefEvent = (stats) =>         //Bravery +2, Energy +2     +1 Mischief
            {
                Typewriter("The humans have a garden bed back here, full of nice-smelling herbs. Pooping in it sure would be luxurious...\n");
                Console.WriteLine("Reward: +2 ❤️  +2 ⚡\n");
                Typewriter("Poop in the garden bed?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy poops in the garden bed. That's a problem she can deal with in the morning.\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.Bravery += 2;
                        stats.Energy += 2;
                        stats.Mischief += 1;
                        Continue();
                        Typewriter("Poppy regains 2 Bravery and 2 Energy.\n");
                        Continue();
                        backyard.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy refuses to poop in the garden.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            alley.MischiefEvent = (stats) =>            //Max Bravery +3, Power +1  +3 Mischief
            {
                Typewriter("There is some stinky garbage back here. Rolling in it would make Poppy feel safe and powerful!\n");
                Console.WriteLine("Reward: Max ❤️  +3, ⚔️  +1\n");
                Typewriter("Roll in the garbage?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy rolls around in the garbage.\nShe's in for a bath when she comes back inside...\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.BraveryMax += 3;
                        stats.Bravery += 3;
                        stats.Power += 1;
                        stats.Mischief += 3;
                        Continue();
                        Typewriter("Poppy's max Bravery increases by 3, and her Power by 1.\n");
                        bathtime = true;
                        Continue();
                        alley.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy ignores the trash.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            sideStreet.MischiefEvent = (stats) =>       //Max Energy +3             +2 Mischief
            {
                Typewriter("There is a parked car with the doors open- someone is carrying in groceries...\nAnd what's that in the front seat? Chicken nuggets?!\n");
                Console.WriteLine("Reward: Max ⚡ +3\n");
                Typewriter("Steal the chicken nuggets?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.\n");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy manages to steal the chicken nuggets from the front seat!\nOh no... the owner saw! Surely they don't know what house she's from... right?\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.EnergyMax += 3;
                        stats.Energy += 3;
                        stats.Mischief += 2;
                        Continue();
                        Typewriter("Poppy's max Energy increases by 3.\n");
                        Continue();
                        sideStreet.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy resists the nuggets.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            mainStreet.MischiefEvent = (stats) =>       //Power +1                  +1 Mischief
            {
                Typewriter("Plenty of neighbors here... with plenty of yards to poop in.\n");
                Console.WriteLine("Reward: ⚔️  +1\n");
                Typewriter("Poop in the neighbor's yard?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy poops in the yard of one of the neighbors.\nAlthough... I don't think there's a dog that lives at that house.\nThey might notice that. Whoops.\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.Power += 1;
                        stats.Mischief += 1;
                        Continue();
                        Typewriter("Poppy's Power increases by 1.\n");
                        Continue();
                        mainStreet.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy refrains from pooping in someone else's yard.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            frontYard.MischiefEvent = (stats) =>        //Energy, Bravery, Power +1 +1 Mischief
            {
                Typewriter("Flowers! Probably planted by the favored human. They'd be nice to roll around in!\n");
                Console.WriteLine("Reward: ❤️  +1, ⚡+1, ⚔️  +1\n");
                Typewriter("Roll around in the flowers?\n");
                Typewriter("A) Yes\nB) No\n");
                string beBad = Console.ReadLine();
                while (beBad.ToUpper() != "A" && beBad.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.");
                    beBad = Console.ReadLine();
                }
                switch (beBad.ToUpper())
                {
                    case "A":
                        Typewriter("Poppy rolls around in the flowers. She loves the smell!\nSome of the flowers are having some trouble standing though... I'm sure it's fine.\n");
                        stats.Time -= new Random().Next(2, 6);
                        stats.Energy += 1;
                        stats.Bravery += 1;
                        stats.Power += 1;
                        stats.Mischief += 1;
                        Continue();
                        Typewriter("Poppy regains 1 Bravery and Energy, and her Power by 1.\n");
                        Continue();
                        frontYard.MischiefDone = true;
                        break;
                    case "B":
                        Typewriter("Poppy ignores the trash.\nGood girl!\n");
                        Continue();
                        break;
                }
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };

            frontPorch.MischiefEvent = (stats) =>        //None
            {
                Typewriter("There's no trouble to be made on the porch.\n");
                Continue();
                frontPorch.MischiefDone = true;
                MusicStop();
                Play("Music\\Explore.mp3");
                return;
            };
            #endregion

            //Begin the Patrol
            PoppyStats(stats);
            bedroom.RoomCleared = true;
            
            currentRoom = bedroom;

            while (!GameOver)
            {
                currentRoom = currentRoom.Enter(stats);
            }
        }
                   //Events        
        public static void Cuddles(ShibaStats stats)                                //Regain Energy and Bravery at the cost of time.
        {
            Console.Clear();
            PoppyStats(stats);
            if (bathtime == true)
            {
                Typewriter($"Your favorite person in the whole world is here.\nUnfortunately, Poppy absolutely smells utterly horrendous and is immediately given a bath.\n");
                Typewriter($"That's gonna take up some time...\n");
                stats.Time -= 20;
                bathtime = false;
                Continue();
                return;
            }
            Typewriter("Your favorite person in the whole wide world is here. Cuddling her will make you feel better. \n \nHow long do you want to cuddle? \n");
            Console.WriteLine("You will regain 1 Energy and 1 Bravery for every 5 minutes. If you wish to leave, type 0.");

            int cuddleTime = 0;
            while (!int.TryParse(Console.ReadLine(), out cuddleTime) || cuddleTime < 0)
            {
                Typewriter("Please enter a valid time, or 0 to not cuddle.");
            }

            if (cuddleTime > stats.Time)
            {
                cuddleTime = stats.Time;
            }

            if (cuddleTime == 0)
            {
                Typewriter("You decided not to cuddle.\n");
                Continue();
                return;
            }
               stats.Time -= cuddleTime;
               stats.Energy += cuddleTime/5;
                if (stats.Energy > 10)
                    {stats.Energy = 10;}    
               stats.Bravery += cuddleTime/5;
                if (stats.Bravery > 10)
                    {stats.Bravery = 10;}
               Typewriter($"You cuddled for {cuddleTime} minutes.\n");
            Continue();

            if (stats.Time <= 0)
            {
                OutOfTime(stats);
            }
            return;
        }
        public static (ShibaStats stats, int threat) Encounter(ShibaStats stats, string enemy, int threat)
        {
            int maxThreat = threat;         //Acts as enemy health.
            int result = 0;
            string action = "";             //Action Selction
            Random roll = new Random();     //Rolls to reduce Threat
            int battle = 1;
            int[] braveDamage = { 0, 1, 1, 1, 1, 1, 2};
            int rage = 0;
            int rageTimer = -1;

            int encounterTimer = 0;

            Console.Clear();
            
            if (enemy == "Mothman")
            {
                Play("Music\\Mothman.mp3");
            }
            else
            {
                Play("Music\\Encounter.mp3");
            }
            
            while (battle == 1)
            {
                PoppyStats(stats);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{enemy}: {threat}/{maxThreat}  \n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("{");
                FilledGauge(threat, ConsoleColor.Red);
                EmptyGauge(threat, maxThreat, ConsoleColor.Red);
                Console.WriteLine("}\n");

            if (stats.Time <= 0)
            {
                OutOfTime(stats);
            }

            int[] growl = {1,4};
            int[] bark = {5,8};
            int[] backup = {8,12};
            int[] morale = {2,4};
            

                if (stats.Bravery > 0 && stats.Energy > 0 && threat > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Choose your action:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"A) Growl \t\t⚔️  {growl[0]+stats.Power+rage} to {growl[1]+stats.Power+rage}.\n");
                    Console.WriteLine($"B) Bark \t\t⚡-1. ⚔️  {bark[0]+stats.Power+rage} to {bark[1]+stats.Power+rage}.\n");
                    Console.WriteLine($"C) Howl for Backup\t⚡-1. ❤️  -1. Call Pup-Tart for a butt-slam. ⚔️  {backup[0]+stats.Power+rage} to {backup[1]+stats.Power+rage}.\n");
                    Console.WriteLine($"D) Howl for Morale\t⚡-2. Poppy howls for a confidence boost. Regains {morale[0]+rage} to {morale[1]+rage} ❤️  .\n");
                    Console.WriteLine($"E) Watch\t\tRegain 1 to 2 ⚡.\n");
                    Console.WriteLine($"F) Shiba Rage\t\t⚡-2. Poppy gets really angry. ⚔️  +2 for 2 turns.\n");
                    Console.WriteLine("G) Retreat\t\tReturn to the bedroom. ❤️  -1. ⏳-1. 🛡️ -1.\n");
                    
                    action = Console.ReadLine();

                        while ((action.ToUpper() != "A") && (action.ToUpper() != "B") && (action.ToUpper() != "C") && (action.ToUpper() != "D") && (action.ToUpper() != "E")&& (action.ToUpper() != "F")&& (action.ToUpper() != "G")&&(action.ToUpper() != "MEGAFLARE"))
                        {
                            Typewriter("That isn't a valid action. Try again.");
                            action = Console.ReadLine();
                        }

                    switch (action.ToUpper())
                    {
                        case "A":       //Growl
                            result = roll.Next(1,5)+stats.Power+rage;
                            Typewriter($"Poppy growls at the {enemy}! Threat reduced by {result}!\n");
                            threat -= result;
                            Continue();
                            break;
                        case "B":       //Bark
                            result = roll.Next(5,9)+stats.Power+rage;
                            Typewriter($"Poppy barks loudly at the {enemy}! Threat reduced by {result}!\n");
                            Continue();
                            stats.Energy --;
                            threat -= result;
                            break;  
                        case "C":       //Call for Backup
                            result = roll.Next(8,13)+stats.Power+rage;
                            Typewriter($"Poppy calls for Pup-Tart's aid! Pup-Tart rushes in and butt-slams the {enemy} before returning to the bedroom! Threat reduced by {result}!\n");
                            Continue();
                            stats.Energy --;
                            stats.Bravery --;
                            threat -= result;
                            break;
                        case "D":       //Howl for Morale
                            result = roll.Next(2,5)+rage;
                            Typewriter($"Poppy lets loose an AROOOOOOOOOO! She feels emboldened and regains {result} Bravery!\n");
                            Continue();
                            stats.Energy -= 2;
                            stats.Bravery += result;
                            break;
                        case "E":       //Watch
                            result = roll.Next(1,3);
                            Typewriter($"Poppy stops and watches the {enemy} intently while regaining her energy!\n");
                            Typewriter($"Poppy regains {result} Energy!\n");
                            Continue();
                            stats.Energy += result;
                            break;
                        case "F":       //Rage
                            if (rageTimer == -1)
                            {
                                Typewriter($"Poppy is overcome by perturbances! Her power increases by 2!\n");
                                Continue();
                                stats.Energy -= 2;
                                rage = 2;
                                rageTimer = 3;
                            }
                            else
                            {
                                Typewriter("Poppy is already mildly perturbed! Nothing happens.");
                            }

                            break;
                        case "G":       //Retreat
                            Typewriter("You run away from the threat back to the safety of the bedroom!\nPoppy loses 1 Bravery!\n");
                            Continue();
                            stats.Bravery --;
                            stats.Time --;
                            stats.Security --;
                            MusicStop();        
                            Play("Music\\Explore.mp3");
                            currentRoom = bedroom;
                            return (stats, threat);
                            break;
                        case "MEGAFLARE":       //Cheat
                            Typewriter($"Poppy unleashes the power of Bahamut and blasts the {enemy} with Megaflare!\n");
                            Continue();
                            threat = 0;
                            break;
                    }

                    encounterTimer ++;
                    if (rageTimer > 0)
                    {
                        rageTimer--;

                        if (rageTimer == 0)
                        {
                            Typewriter("Poppy's Shiba Rage has worn off!\n");
                            rage = 0;
                        }
                    }

                    if (encounterTimer == 3)
                    {
                        stats.Time --;    //Every 3 rounds reduces time by 1 minute.                
                        encounterTimer = 0;
                    }

                    if (threat > 0)
                    {
                        result = roll.Next(7);
                        Typewriter($"The {enemy}'s intimidating presence reduces Poppy's Bravery by {braveDamage[result]}!\n");
                        stats.Bravery -= braveDamage[result];
                        Continue();
                        Console.Clear();
                    }
                        if (threat <= 0)
                    {
                        Console.Clear();
                        MusicStop();
                        Play("Music\\Victory.mp3");
                        PoppyStats(stats);
                        Console.Write($"{enemy}: 0/{maxThreat}  \n");
                        Console.Write("{");
                        FilledGauge(threat, ConsoleColor.Red);
                        EmptyGauge(threat, maxThreat, ConsoleColor.Red);
                        Console.WriteLine("}");

                        //XP Increase
                        stats.XP += maxThreat;
                        //Results
                        Typewriter($"Poppy has intimidated the {enemy} into submission! This room is secure!\n\n");
                        Typewriter($"+{maxThreat} XP\n\nXP: {stats.XP} / {stats.XPMax}\n[");
                        FilledGauge(stats.XP, ConsoleColor.Yellow);
                        EmptyGauge(stats.XP, stats.XPMax, ConsoleColor.Yellow);
                        Console.Write("]\n");

                        //Handles leveling
                        while (stats.XP >= stats.XPMax) 
                        { 
                            stats.Level += 1; 
                            stats.Points += 1; 
                            stats.XP -= stats.XPMax; 
                            stats.XPMax += 10;     
                        }

                        while (stats.Points > 0)
                        {
                            Typewriter($"Poppy is now Level {stats.Level}!\nChoose an attribute to increase:\n");

                            Console.WriteLine($"A) ❤️  {stats.BraveryMax} -> ❤️  {stats.BraveryMax + 1}");
                            Console.WriteLine($"B) ⚡ {stats.EnergyMax} -> ⚡ {stats.EnergyMax + 1}");
                            Console.WriteLine($"C) ⚔️  {stats.Power} -> ⚔️  {stats.Power + 1}");

                            char selection;

                            while (true)
                            {
                                string input = Console.ReadLine().ToUpper();

                                if (input.Length == 1 && (input == "A" || input == "B" || input == "C"))
                                {
                                    selection = input[0];
                                    break;
                                }

                                Typewriter("That isn't a valid choice. Try again.\n");
                            }

                            switch (selection)
                            {
                                case 'A':
                                    stats.BraveryMax += 1;
                                    stats.Bravery += 1;
                                    stats.Points--;
                                    Typewriter($"Bravery increased to {stats.BraveryMax}.\n");
                                    break;

                                case 'B':
                                    stats.EnergyMax += 1;
                                    stats.Energy += 1;
                                    stats.Points--;
                                    Typewriter($"Energy increased to {stats.EnergyMax}.\n");
                                    break;

                                case 'C':
                                    stats.Power += 1;
                                    stats.Points--;
                                    Typewriter($"Power increased to {stats.Power}.\n");
                                    break;
                            }

                            Console.WriteLine($"\nPOPPY\tLevel {stats.Level}");
                            Typewriter($"XP: {stats.XP} / {stats.XPMax}\n[");
                            FilledGauge(stats.XP, ConsoleColor.Yellow);
                            EmptyGauge(stats.XP, stats.XPMax, ConsoleColor.Yellow);
                            Console.Write("]\n");
                            Console.WriteLine("");
                            Console.WriteLine($"❤️  {stats.BraveryMax} ");
                            Console.WriteLine($"⚡ {stats.EnergyMax}");
                            Console.WriteLine($"⚔️  {stats.Power}");
                        }

                        battle = 0;
                        if (rageTimer >= 0)
                        {
                            stats.Power -= rage;
                            rageTimer = -1;
                        }
                        Continue();
                        MusicStop();
                        Play("Music\\Explore.mp3");
                        return (stats, threat);
                    }
                }

                if (stats.Bravery <= 0 || stats.Energy <= 0)
                {
                    Typewriter("Poppy can no longer continue the patrol and returns to the bedroom. \n");
                    battle = 0;
                    Continue();
                    Console.Clear();
                    MusicStop();
                    Play("Music\\Explore.mp3");
                    currentRoom = bedroom;
                    return (stats, threat);
                }
            }
         return (stats, threat);   
        }
        public static async Task OutOfTime(ShibaStats stats)                                                                 //Handles the end of the game once Poppy runs out of time.
        {   
            //Used for final scoring
            float security = stats.Security * 0.2f;
            float mischief = 1500f - (stats.Mischief*100);
            float time = stats.Time * 25;
            float level = (stats.Level-1) * 100;
            float total = (float)Math.Floor((mischief + time + level) * security);

            Console.Clear();
            MusicStop();
            Play("Music\\Patrol Over.mp3");
            Typewriter("Poppy's patrol time is over.\n");
            Continue();
            Typewriter("The house is ");

            switch (stats.Security)
            {
                case 0:
                    Typewriter("not secure whatsoever. A treacherous moose could waltz right into the house and steal all the chicken nuggets. \n");
                    Continue();
                    Typewriter("Pup-Tart will probably never delegate nightly patrol duties to her again.\n");
                    break;
                case 1:
                    Typewriter("barely secure at all. \n");
                    Continue();
                    Typewriter("Pup-Tart will absolutely be retraining his child.\n");
                    break;
                case 2:
                    Typewriter("hardly secure. Very few threats were dealt with.\n");
                    Continue();
                    Typewriter("Pup-Tart is going to treat this as a training exercise and take over for a bit while Poppy gets up to speed.\n");
                    break;
                case 3:
                    Typewriter("lightly secured.\nA breach could still happen.\n");
                    Continue();
                    Typewriter("Pup-Tart will want to supervise her next patrol.\n");
                    break;
                case 4:
                    Typewriter("mildly secured. It is about as secure as it was before, but not quite.\n");
                    Continue();
                    Typewriter("Pup-Tart will sigh about this, but it's not the end of the world. Tonight will be better.\n");
                    break;
                case 5:
                    Typewriter("as secure as it was when she started.\n");
                    Continue();
                    Typewriter("Pup-Tart will see this as acceptable.\n");
                    break;
                case 6:
                    Typewriter("more secure than when she started.\n");
                    Continue();
                    Typewriter("Pup-Tart knows it could be better- but he will take this as a win.\n");
                    break;
                case 7:
                    Typewriter("secured. Many threats are dealt with\n");
                    Continue();
                    Typewriter("Pup-Tart still sees room for improvement- but this was a good night. He is content.\n");
                    break;
                case 8:
                    Typewriter("very secure. The house is safe tonight.\n");
                    Continue();
                    Typewriter("Pup-Tart is content. She did well.\n");
                    break;
                case 9:
                    Typewriter("heavily secured. Everyone can sleep easy.\n");
                    Continue();
                    Typewriter("Pup-Tart is very pleased with her performance tonight. She will make a fine guardian of this household.\n");
                    break;
                case 10:
                    Typewriter("extremely secure. This is basically Fort Knox by Pup-Tart standards. \n");
                    Continue();
                    Typewriter("Pup-Tart couldn't be more proud of his prodigy and will shower her with chicken nuggets in the morning.\n");
                    break;
            }
            Continue();
            switch (stats.Mischief)
            {
                case 0:
                Typewriter("Poppy caused no mischief during her patrol. What a good girl!\n"); break;   
                case <= 3:
                Typewriter("Poppy caused a little mischief during her patrol, but nothing too serious. The humans are mildly annoyed.\n"); break;
                case <= 6:
                Typewriter("Poppy got into trouble during her patrol. She gets scolded for being naughty."); break;
                case <= 9:
                Typewriter("Poppy caused some mild chaos throughout her patrol. She ended up spending 20 minutes in the cage."); break;
                case <= 12:
                Typewriter("Poppy caused significant trouble during her patrol. She spent 2 hours in the cage as punishment."); break;
                case >= 13:
                Typewriter("Poppy caused untold chaos tonight. She spends the night in the cage to think about what she's done."); break;
            }
            //Display the Final Score
            Console.WriteLine("\n═══════════ FINAL SCORE ═══════════");
            Typewriter($"Security: \t\t{security}x Multiplier\n");
            Typewriter($"Good Behavior: \t\t{mischief}\n");
            Typewriter($"Remaining Time: \t{time}\n");
            Typewriter($"Level: \t\t\t{level}\n\n");
            Typewriter($"Total: \t\t\t{total}\n");
            Typewriter("═══════════════════════════════════\n\n");
            Continue();
            
            //Update the leaderboards
            Typewriter("Enter your name for the leaderboard:\n");
            string playerName = Console.ReadLine().Trim();

            while (string.IsNullOrWhiteSpace(playerName))
            {
                Typewriter("Nice try. But the shoobers demand to know who you are. Enter a valid name.\n");
                playerName = Console.ReadLine().Trim();
            }
            await SubmitScoreAsync(playerName, (int)total);
            Typewriter("Score submitted successfully!\n");
            Continue();

            try
            {
                var scores = await GetScoresAsync();

                Console.WriteLine("\n═══════════ LEADERBOARD ═══════════");
                int rank = 1;

                foreach (var score in scores.Take(10))
                {
                    Console.WriteLine($"{rank}: {score.PlayerName,20}  {score.FinalScore}");
                    rank++;
                }

                Console.WriteLine("═══════════════════════════════════\n");
            }
            catch
            {
                Typewriter("The shibas lost the leaderboard.... for shame.\n");
            }
            
            Typewriter("Thank you for patrolling the house!\nPress any key to exit.\n");
            Continue();
            MusicStop();
            Environment.Exit(0);

        }
        public static void EndPatrolCheck(ShibaStats stats)
        {
            Typewriter("You're about to end the patrol for the night. Are you sure?\nA) Yes\nB) No\n");
                string selection = Console.ReadLine();
                while (selection.ToUpper() != "A" && selection.ToUpper() != "B")
                {
                    Typewriter("That isn't a valid action. Try again.\n");
                    selection = Console.ReadLine();
                }
                switch (selection.ToUpper())
                {
                    case "A":
                        OutOfTime(stats);
                        break;
                    case "B":
                        return;
                        break;
                }
            }

            public class Score  //Matches the API
            {
                public string PlayerName { get;  set; }     //This is the name players will enter at the end of the game, like at an arcade
                public int FinalScore { get; set; }         //The total score after all calculations.
            }

            private static readonly HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5145/")
            };

            public static async Task SubmitScoreAsync(string playerName, int finalScore)
            {
                var score = new Score
                {
                    PlayerName = playerName,
                    FinalScore = finalScore
                };

                var response = await httpClient.PostAsJsonAsync("scores", score);
                response.EnsureSuccessStatusCode();
            }

            public static async Task<List<Score>> GetScoresAsync()
            {
                var scores = await httpClient.GetFromJsonAsync<List<Score>>("scores");
                return scores ?? new List<Score>();
            }
        }
    
    }
