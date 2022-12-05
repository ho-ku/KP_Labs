using System;
using System.Linq;
using System.IO;

namespace KP_Lab
{
    class Program
    {
        static int n = 0, m = 0;
        static int[,] matrix;
        
        static void Main(string[] args)
        {
            if (readInput()) processData();
        }

        static void processData()
        {
            int[,] oneIndices = new int[n * m, 2];
            int currentIndex = 0;

            bool hasOne = false;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        oneIndices[currentIndex, 0] = i;
                        oneIndices[currentIndex, 1] = j;
                        hasOne = true;
                    }
                    else
                    {
                        oneIndices[currentIndex, 0] = -1;
                        oneIndices[currentIndex, 1] = -1;
                    }
                    currentIndex++;
                }

            if (!hasOne)
            {
                Console.WriteLine("We have no ones");
                return;
            }

            int[,] resultMatrix = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (matrix[i, j] == 0)
                    {
                        int minimalDistance = 1000000000;
                        for (int f = 0; f < n * m; f++)
                        {
                            if (oneIndices[f, 0] == -1) continue;
                            int currentDistance = Math.Abs(i - oneIndices[f, 0]) + Math.Abs(j - oneIndices[f, 1]);
                            if (currentDistance < minimalDistance) minimalDistance = currentDistance;
                        }
                        resultMatrix[i, j] = minimalDistance;
                    } else
                    {
                        resultMatrix[i, j] = 0;
                    }

            string resultData = "";
            for (int i = 0; i < n; i++)
            {
                string tempData = "";
                for (int j = 0; j < m; j++) tempData = tempData + resultMatrix[i, j] + " ";
                tempData += "\n";
                resultData += tempData;
            }
                
            File.WriteAllText("/Users/den444ik/Desktop/DB/Study/University/4.1/KP_Labs/Lab3/Lab3/output.txt", resultData);
        }

        static bool readInput()
        {
            string data = File.ReadAllText("/Users/den444ik/Desktop/DB/Study/University/4.1/KP_Labs/Lab3/Lab3/input.txt");
            if (data == "")
            {
                Console.WriteLine("No data");
                return false;
            }
            string[] inputs = data.Split('\n');
            string[] numbers = inputs[0].Split(' ');

            if (numbers.Length != 2 || numbers.Contains(""))
            {
                Console.WriteLine("Data malformed");
                return false;
            }

            if (!Int32.TryParse(numbers[0], out n) ||
                !Int32.TryParse(numbers[1], out m)) {
                Console.WriteLine("Data malformed");
                return false;
            }

            if (n <= 0 || m <= 0 || n > 100 || m > 100)
            {
                Console.WriteLine("n and m must be in range between 1 and 100");
                return false;
            }

            if (n != inputs.Length - 1)
            {
                Console.WriteLine("Mismatch between strings count and exact info");
                return false;
            }

            matrix = new int[n, m];

            
            for (int i = 0; i < n; i++)
            {
                string[] currentData = inputs[i + 1].Split(' ');
                if (currentData.Length != m)
                {
                    Console.WriteLine("Inputs count is not m");
                    return false;
                }

                for (int j = 0; j < m; j++)
                {
                    if (!Int32.TryParse(currentData[j], out matrix[i,j]) || !(matrix[i, j] == 0 || matrix[i, j] == 1))
                    {
                        Console.WriteLine("Data malformed");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
