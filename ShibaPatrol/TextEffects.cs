public class TextEffects()
{
            /// <summary>
            /// Asks for an input before continuing; deletes the Continue prompt and any inputs.
            /// </summary>
     public static void Continue()                                                                              //Asks for an input before continuing; deletes the Continue prompt and any inputs.
        {
            System.Threading.Thread.Sleep(100);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Press Any Key to Continue →");
            Console.ReadKey(true);

            int length = "Press Any Key to Continue →".Length;

            for (int i = 0; i < length; i++)
                Console.Write("\b \b");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }  
            /// <summary>
            /// An effect for displaying text gradually in a typewriter-effect rather than all at once.
            /// </summary>
        public static void Typewriter(String message)                                                              //An effect for displaying text gradually rather than all at once.
        {
            for (int text = 0; text < message.Length; text++)
            {
                Console.Write(message[text]);
                System.Threading.Thread.Sleep(1);
            }
        }
}