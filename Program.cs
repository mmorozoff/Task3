using System;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;

namespace Task3
{
    class Program
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private static Random rnd = new Random();
        private const int SIZE = 25;
        static void Main(string[] args)
        {
            List<String> values = Console.ReadLine().Split(' ').ToList();
            if (!CheckInput(values))
            {
                Console.WriteLine("Error!");
                return;
            }
            String computer = values[rnd.Next(0, values.Count() - 1)];
            byte[] key = new Byte[SIZE];
            rngCsp.GetBytes(key, 0, SIZE);
            String HMAC_key = "";
            key.ToList().ForEach(x => HMAC_key += string.Format("{0:x2}", x));
            byte[] tmp = Encoding.UTF8.GetBytes(computer);
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                byte[] hashValue = hmac.ComputeHash(tmp);
                var hmac_str = "";
                hashValue.ToList().ForEach(x => hmac_str += string.Format("{0:x2}", x));
                Console.WriteLine("HMAC: {0}", hmac_str);
            }
            var player = SecondMove(values);
            if (player == "-1") return;
            Console.WriteLine("Computer move: {0}", computer);
            Console.WriteLine(Result(values, player, computer));
            Console.WriteLine("HMAC key: {0}", HMAC_key);
        }

        private static String SecondMove(List<String> values)
        {
            int move;
            while (true)
            {
                Console.WriteLine("Available moves:");
                for (int i = 0; i < values.Count(); i++)
                    Console.WriteLine("{0} - {1}", i + 1, values[i]);
                Console.WriteLine("0 - exit");
                Console.Write("Enter your move: ");
                bool flag = int.TryParse(Console.ReadLine(), out move);
                if (!flag || move > values.Count() || move < 0)
                    Console.WriteLine("Error!");
                else
                    break;
            }
            if (move == 0)
            {
                Console.WriteLine("Good Bye");
                return "-1";
            }
            else
                Console.WriteLine("Your move: {0}", values[move - 1]);
            return values[move - 1];
        }
        private static String Result(List<String> values, String player, String computer)
        {
            int x = values.IndexOf(player), y = values.IndexOf(computer);
            if (x == y)
                return "Draw!";
            int count = (values.Count() - 1) >> 1;
            if (x - count >= 0)
                if (y < x && y >= x - count)
                    return "You win!";
                else
                    return "Computer win!";
            else
                if (y < x || y >= values.Count() - 1 + x - count)
                return "You win!";
            else
                return "Computer win!";
        }

        private static bool CheckInput(List<String> values)
        {
            SortedSet<String> tmp = new SortedSet<String>();
            values.ForEach(x => tmp.Add(x));
            if (values.Count() != tmp.Count() || values.Count() % 2 != 1 || values.Count() <= 1) return false;
            return true;
        }
    }
}
