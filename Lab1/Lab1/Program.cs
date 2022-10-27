using System;
using System.Linq;
using System.IO;

namespace KP_Lab
{
    class Unit
    {
        public string lowercasedName;
        public double value;

        public Unit(string name, double value)
        {
            this.lowercasedName = name.ToLower();
            this.value = value;
        }

        public string descr()
        {
            return lowercasedName + " " + value;
        }
    }

    class ProcessableUnit: IComparable<ProcessableUnit>
    {
        public string name;
        public double count;
        public double oldPrice;
        public double newPrice;

        public ProcessableUnit(string name, double count, double oldPrice, double newPrice)
        {
            this.name = name;
            this.count = count;
            this.oldPrice = oldPrice;
            this.newPrice = newPrice;
        }

        public string descr()
        {
            return name + " " + count + " " + oldPrice + " " + newPrice;
        }

        public double economy()
        {
            return oldPrice - newPrice;
        }

        public int CompareTo(ProcessableUnit other)
        {
            return economy().CompareTo(other.economy());
        }
    }

    class Program
    {

        static int n = 0, k1 = 0, k2 = 0;
        static double d = 0;
        static Unit[] neededProducts;
        static Unit[] base1;
        static Unit[] base2;

        static ProcessableUnit[] processableUnits = new ProcessableUnit[0];

        static void Main(string[] args)
        {
            readInput();
            defineUnitsToProcess();
            processUnits();
        }

        static void processUnits()
        {
            int nonNullElementsCount = 0;
            for (int i = 0; i < processableUnits.Length; i++)
            {
                if (processableUnits[i] != null)
                {
                    nonNullElementsCount++;
                }
            }
            int settedUnits = 0;
            ProcessableUnit[] units = new ProcessableUnit[nonNullElementsCount];
            for (int i = 0; i < processableUnits.Length; i++)
            {
                if (processableUnits[i] != null)
                {
                    units[settedUnits] = processableUnits[i];
                    settedUnits++;
                }
            }

            double priceLeft = d;
            Array.Sort(units);
            Array.Reverse(units);

            Unit[] counts = new Unit[nonNullElementsCount];
            int currentIndex = 0;
            for (int i = 0; i < units.Length; i++)
            {
                double allowedCapacity = priceLeft / units[i].oldPrice;
                if (allowedCapacity >= units[i].count)
                {
                    counts[currentIndex] = new Unit(units[i].name, units[i].count);
                    priceLeft -= units[i].count * units[i].oldPrice;
                } else
                {
                    counts[currentIndex] = new Unit(units[i].name, allowedCapacity);
                    priceLeft -= allowedCapacity * units[i].oldPrice;
                }
                currentIndex++;
            }

            string countsToWrite = "";
            for (int i = 0; i < neededProducts.Length; i++)
            {
                Unit neededProduct = neededProducts[i];
                double count = 0;
                
                for (int j = 0; j < counts.Length; j++)
                {
                    if (counts[j].lowercasedName == neededProduct.lowercasedName)
                    {
                        count = counts[j].value;
                    }
                }

                countsToWrite += count;
                countsToWrite += "\n";
            }

            File.WriteAllText("/Users/den444ik/Desktop/DB/Study/University/4.1/KP_Labs/Lab1/Lab1/output.txt", countsToWrite);
        }

        static void defineUnitsToProcess()
        {
            processableUnits = new ProcessableUnit[n];
            for (int i = 0; i < n; i++)
            {
                Unit neededProduct = neededProducts[i];
                double oldPrice = 0;
                double newPrice = 0;

                for (int j = 0; j < k1; j++)
                {
                    Unit unitInOldBase = base1[j];
                    if (unitInOldBase.lowercasedName == neededProduct.lowercasedName)
                    {
                        oldPrice = unitInOldBase.value;
                    }
                }

                for (int j = 0; j < k2; j++)
                {
                    Unit unitInNewBase = base2[j];
                    if (unitInNewBase.lowercasedName == neededProduct.lowercasedName)
                    {
                        newPrice = unitInNewBase.value;
                    }
                }

                if (oldPrice == 0)
                {
                    Console.WriteLine("Unit is not present in old base");
                    return;
                }

                if (newPrice < oldPrice && newPrice != 0)
                {
                    processableUnits[i] = new ProcessableUnit(neededProduct.lowercasedName, neededProduct.value, oldPrice, newPrice);
                }
            }
        }

        static void readInput()
        {
            string data = File.ReadAllText("/Users/den444ik/Desktop/DB/Study/University/4.1/KP_Labs/Lab1/Lab1/input.txt");
            if (data == "")
            {
                Console.WriteLine("Input is empty");
            }

            string[] inputs = data.Split('\n');
            string[] inputNumbers = inputs[0].Split(' ');

            if (inputNumbers.Length != 4 || inputNumbers.Contains(""))
            {
                Console.WriteLine("Expected 4 inputs");
                return;
            }

            bool isParsable = Int32.TryParse(inputNumbers[0], out n) &&
                              Double.TryParse(inputNumbers[1], out d) &&
                              Int32.TryParse(inputNumbers[2], out k1) &&
                              Int32.TryParse(inputNumbers[3], out k2);

            if (!isParsable)
            {
                Console.WriteLine("Some inputs are not numbers");
                return;
            }

            if (n < 1 || n > 1000 || d <= 0 || k1 < 1 || k2 > 1000)
            {
                Console.WriteLine("Input is not following rules");
                Console.WriteLine("1 < n < 1000\nd >= 0\nk1 >= 1\nk2<=1000");
                return;
            }

            neededProducts = new Unit[n];
            base1 = new Unit[k1];
            base2 = new Unit[k2];

            for (int i = 1; i < inputs.Length; i++)
            {
                string[] currentUnitData = inputs[i].Split(' ');
                double value = 0;

                if (currentUnitData.Length != 2)
                {
                    Console.WriteLine("Data malformed");
                    return;
                }

                bool isUnitParsable = Double.TryParse(currentUnitData[1], out value);
                if (!isUnitParsable)
                {
                    Console.WriteLine(currentUnitData[1]);
                    Console.WriteLine("Data malformed");
                    return;
                }

                int j = i - 1;
                if (j < n)
                {
                    neededProducts[j] = new Unit(currentUnitData[0], value);
                }
                else if (j - n < k1)
                {
                    base1[j - n] = new Unit(currentUnitData[0], value);
                }
                else
                {
                    base2[j - n - k1] = new Unit(currentUnitData[0], value);
                }
            }
        }
    }
}
