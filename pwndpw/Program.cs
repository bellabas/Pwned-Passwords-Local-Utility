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

        static string ReadWithoutEcho()
        {
            Console.Write("Enter password: ");
            string password = "";
            ConsoleKeyInfo ch = Console.ReadKey(true);
            while (ch.Key != ConsoleKey.Enter)
            {
                password += ch.KeyChar;
                Console.Write('*');
                ch = Console.ReadKey(true);
            }
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
