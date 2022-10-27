using System;
using System.Linq;
using System.IO;

namespace KP_Lab
{
    class Program
    {
        static int n;
        static int[] prices;
        
        static void Main(string[] args)
        {
            readInput();
            processData();
        }

        static void processData()
        {
            int[] sortedPrices = new int[prices.Length];
            for (int i = 0; i < n; i++) sortedPrices[i] = prices[i];
            Array.Sort(sortedPrices);
            Array.Reverse(sortedPrices);
            int couponsCount = 0;
            int totalPrice = 0;

            for (int i = 0; i < n; i++)
            {
                bool willUseCoupon = false;
                for (int j = 0; j < couponsCount; j++)
                    if (prices[i] == sortedPrices[j])
                    {
                        willUseCoupon = true;
                        couponsCount--;
                    }
                        
                if (!willUseCoupon)
                {
                    if (prices[i] > 100) couponsCount++;
                    totalPrice += prices[i];
                }


                for (int j = 0; j < n; j++)
                {
                    if (prices[i] == sortedPrices[j])
                    {
                        sortedPrices[j] = 0;
                        Array.Sort(sortedPrices);
                        Array.Reverse(sortedPrices);
                    }
                }
            }
            File.WriteAllText("/Users/den444ik/Desktop/DB/Study/University/4.1/KP_Labs/Lab2/Lab2/output.txt", totalPrice.ToString());
        }
   
        static void readInput()
        {
            string data = File.ReadAllText("/Users/den444ik/Desktop/DB/Study/University/4.1/KP_Labs/Lab2/Lab2/input.txt");
            if (data == "")
            {
                Console.WriteLine("No data");
                return;
            }
            string[] inputs = data.Split('\n');
            if (!Int32.TryParse(inputs[0], out n)) {
                Console.WriteLine("Data malformed");
                return;
            }
            prices = new int[n];
            for (int i = 0; i < n; i++)
            {
                if (!Int32.TryParse(inputs[i+1], out prices[i]))
                {
                    Console.WriteLine("Data malformed");
                    return;
                }
            }
        }
    }
}
