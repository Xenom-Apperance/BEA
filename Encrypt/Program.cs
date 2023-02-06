namespace Encrypt;
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
                if (IntPrimeCheck(i) == true)
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
                int a;
                if (int.TryParse(Convert.ToString(data), out a) == false)
                {
                    key = PrimeGen(rand.Next(3, Int16.MaxValue));
                }
                else
                {
                    key = PrimeGen(rand.Next(3, a));
                }
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
        static ulong Crunch(string inp)
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
        static ulong Decrypt(string inp, int key)
        {
            string[] ine = inp.Split('-');
            ulong output = Convert.ToUInt64(ine[0]) / Convert.ToUInt64(key);
            output -= ulong.Parse(ine[1]);
            return output;
        }
        static void Main(string[] args)
        {
            Console.Write("Zadejte data: ");
            string data = Console.ReadLine();
            Console.WriteLine("Zadejte (d)ešifrování nebo (z)ašifrování: ");
            char mode = Console.ReadLine().ToLower()[0];
            switch (mode)
            {
                case 'd':
                    Console.WriteLine("Zadejte klíč nebo 0 pro vyhledání klíče metodou bruteforce: ");
                    int key1 = Convert.ToInt32(Console.ReadLine());
                    ulong dec;
                    string s;
                    if (key1 == 0)
                    {
                        dec = Crunch(data);
                    }
                    else
                    {
                        dec = Decrypt(data, key1);
                    }
                    if (dec % 3 != 0)
                    {
                        s = "0" + dec;
                    }
                    else
                    {
                        s = Convert.ToString(dec);
                    }
                    string temp = "";
                    string final = "";
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (temp.Length == 3)
                        {
                            final = final + Convert.ToChar(int.Parse(temp));
                            temp = "";
                        }
                        temp = temp + s[i];
                    }
                    Console.WriteLine("Dešifrovaná zpráva: " + final);
                    break;
                case 'z':
                    Console.WriteLine("Zadejte klíč nebo 0 pro autovygenerování klíče: ");
                    int key2 = int.Parse(Console.ReadLine());
                    string st = "";
                    ulong utemp;
                    foreach (char i in data)
                    {
                        utemp = Convert.ToUInt64(i);
                        if (100 > utemp)
                        {
                            st = st + 0;
                            st = st + utemp;
                        }
                        else
                        {
                            st = st + utemp;
                        }
                    }
                    Console.WriteLine("Zašifrovaná zpráva: " + Encrypt(Convert.ToUInt64(st), key2));
                    break;
                default:
                    Console.WriteLine("Nesprávný vstup, zkuste proram znovu spustit");
                    break;
            }
            Console.ReadKey();
        }
    }