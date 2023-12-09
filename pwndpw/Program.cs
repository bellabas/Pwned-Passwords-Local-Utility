using System;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Diagnostics;

namespace pwndpw
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                string password;
                if (args.Length != 0 && args[0].ToLower() == "-echo")
                {
                    password = ReadWithoutEcho(true);
                }
                else
                {
                    password = ReadWithoutEcho();
                }
                if (password != "")
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes = sha1.ComputeHash(sourceBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                    string hash5 = hash.Substring(0, 5);
                    string search = hash.Substring(5);
                    Console.WriteLine($"\nComplete SHA1 hash: {hash}");
                    Console.WriteLine($"First 5 characters of hash: {hash5}");
                    Console.WriteLine($"Rest of the hash: {search}");
                    Clipboard.SetText(search);
                    Console.WriteLine("\nCopied to clipboard!");
                    string url = "https://api.pwnedpasswords.com/range/";
                    url += hash5;
                    Process.Start(url);
                }
            }
        }

        static string ReadWithoutEcho()
        {
            Console.Write("Enter password: ");
            string password = "";
            ConsoleKeyInfo ch = new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false); // null

            do
            {
                if (ch.Key != ConsoleKey.NoName && ch.Key != ConsoleKey.Backspace && 
                    ( Char.IsLetterOrDigit(ch.KeyChar) || Char.IsPunctuation(ch.KeyChar) || Char.IsSymbol(ch.KeyChar) ) )
                {
                    password += ch.KeyChar;
                    Console.Write('*');
                }
                if (password.Length > 0 && ch.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b \b");
                    password = password.Substring(0, password.Length - 1);
                }
                ch = Console.ReadKey(true);
            }
            while (ch.Key != ConsoleKey.Enter);

            return password;
        }

        static string ReadWithoutEcho(bool withEcho)
        {
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            return password;
        }
    }
}
