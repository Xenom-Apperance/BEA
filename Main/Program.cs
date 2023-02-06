namespace Main;
internal class Program
    {
        static bool IntPrimeCheck(int num)
        {
            for (int i = 2; i < Math.Sqrt(num); i++)
            {
                if ((num % i) == 0)
                {
                    return false;
                }
            }
            return true;
        }
        static bool UlongPrimeCheck(ulong num)
        {
            for (int i = 2; i < Math.Sqrt(num); i++)
            {
                if ((num % Convert.ToUInt64(i)) == 0)
                {
                    return false;
                }
            }
            return true;
        }
        static int PrimeGen(int g)
        {
            int j = 3;
            int i = 3;
            int n = 0;
            while (j < g)
            {
                if (IntPrimeCheck(i) ==  true)
                {
                    n = i;
                    j++;
                }
                i++;
            }
            return n;
        }
        static string Encrypt(ulong data, int key)
        {
            if (key == 0)
            {
                Random rand = new Random();
                key = PrimeGen(rand.Next(3, Convert.ToInt32(data)));
                Console.WriteLine("Key used to encrypt data: " + key);
            }
            int offset = 0;
            while (UlongPrimeCheck(data) == false)
            {
                offset++;
                data++;
            }
            if (offset == 0)
            {
                data++;
                offset++;
                while (UlongPrimeCheck(data) == false)
                {
                    offset++;
                    data++;
                }
            }
            ulong e = data * Convert.ToUInt64(key);
            return e.ToString() + "-" + offset;
        }
        static int Crunch(string inp)
        {
            string[] ine = inp.Split('-');
            int key = 0;
            for (ulong i = 2; i < Convert.ToUInt64(ine[0]); i++)
            {
                if (UlongPrimeCheck(i) && (Convert.ToUInt64(ine[0]) % i) == 0)
                {
                    if (UlongPrimeCheck(Convert.ToUInt64(ine[0]) / i))
                    {
                        key = Convert.ToInt32(Convert.ToUInt64(ine[0]) / i);
                        break;
                    }
                }
            }
            Console.WriteLine("Found key by crunching: " + key);
            return Decrypt(inp, key);
        }
        static int Decrypt(string inp, int key)
        {
            string[] ine = inp.Split('-');
            int output = Convert.ToInt32(Convert.ToUInt64(ine[0]) / Convert.ToUInt64(key));
            output -= int.Parse(ine[1]);
            return output;
        }
        static void Main(string[] args)
        {
            Console.Write("Enter data: ");
            string data = Console.ReadLine();
            Console.WriteLine("Enter d for decrypt, e for encrypt and c for crunch");
            char mode = Console.ReadLine().ToLower()[0];
            switch (mode)
            {
                case 'd':
                    Console.Write("Enter a key to decrypt the data with: ");
                    int key = int.Parse(Console.ReadLine());
                    Console.WriteLine("Decrypted data: " + Decrypt(Convert.ToString(data), key));
                    break;
                case 'e':
                    Console.Write("Enter key, prime number, or 0 for autogen: ");
                    int inp = int.Parse(Console.ReadLine());
                    Console.WriteLine("Encrypted data: " + Encrypt(Convert.ToUInt64(data), inp));
                    break;
                case 'c':
                    Console.WriteLine("Crunching the key... ");
                    Console.WriteLine("Decrypted data: " + Crunch(Convert.ToString(data)));
                    break;
                default:
                    Console.WriteLine("Wrong input, shutting down...");
                    break;
            }
            Console.ReadKey();
        }
    }