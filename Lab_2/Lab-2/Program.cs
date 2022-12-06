using System;
using System.Collections.Generic;
using System.IO;

namespace Lab_2
{
    class Program
    {

        static void Main(string[] args)
        {

            string file = File.ReadAllText("Input.txt").Replace("\r", "");
            string[] lines = file.Split('\n');

            CheckFileStructure(lines);


            int varieties = Convert.ToInt32(lines[0]);
            List<int[]> treeShadows = GetShadowsList(lines);
            int gardens = Convert.ToInt32(lines[varieties + 1]);
            int[] gardenBeds = new List<int>(Array.ConvertAll(lines[varieties + 2].Split(" "), int.Parse)).ToArray();

            var alley = new Dictionary<int, int>(); // 0 - garden bed index, 1 - varieties index

            PlantFirstTree(varieties, treeShadows, gardenBeds, alley);
            PlantAnotherTrees(varieties, treeShadows, gardens, gardenBeds, alley);

            int answer = alley.Count;


            ShowShadows(treeShadows);
            ShowGardenBeds(gardenBeds);
            ShowAlley(alley);
            ShowAnswer(answer);

            File.WriteAllText("Output.TXT", answer.ToString());
        }

        private static List<int[]> GetShadowsList(string[] lines)
        {
            var shadows = new List<int[]>();
            for (int i = 1; i < Convert.ToInt32(lines[0]) + 1; i++)
            {
                shadows.Add(new List<int>(Array.ConvertAll(lines[i].Split(" "), int.Parse)).ToArray());
            }
            return shadows;
        }

        private static void CheckFileStructure(string[] lines)
        {

        }

        private static void ShowAnswer(int answer)
        {
            Console.WriteLine($"\nThere are maximum available places for planting trees: {answer}");
        }

        private static void ShowGardenBeds(int[] gardenBeds)
        {
            Console.WriteLine("\nAvailable gardens beds: ");
            foreach (int bed in gardenBeds)
            {
                Console.Write($"{bed} ");
            }
            Console.WriteLine();
        }

        private static void ShowShadows(List<int[]> treeShadows)
        {
            Console.WriteLine("\nVarities shadows(W, E):");
            foreach (int[] shadow in treeShadows)
            {
                Console.WriteLine($"{shadow[0]} {shadow[1]}");
            }
        }

        private static void ShowAlley(Dictionary<int, int> alley)
        {
            Console.WriteLine("\nCompatible garden beds:");
            foreach (int key in alley.Keys)
            {
                Console.WriteLine($"{key} {alley[key] + 1}");
            }
        }

        static void PlantFirstTree(int varieties, List<int[]> treeShadows, int[] gardenBeds, Dictionary<int, int> alley)
        {
            int[] minRightShadow = { 0, treeShadows[0][1] };
            for (int i = 1; i < varieties; i++)
            {
                if (treeShadows[i][1] < minRightShadow[1])
                {
                    minRightShadow[0] = i;
                    minRightShadow[1] = treeShadows[i][1];
                }
            }

            alley.Add(gardenBeds[0], minRightShadow[0]);
        }

        static void PlantAnotherTrees(int varietiesNum, List<int[]> treeShadows, int gardensNum, int[] gardenBeds, Dictionary<int, int> alley)
        {
            int previousPlantedBedIndex = 0;
            for (int i = 1; i < gardensNum; i++)
            {
                int[] minRightShadow = { 0, GetMaxLength(treeShadows) };
                bool treeForGardenPositionFound = false;
                for (int j = 0; j < varietiesNum; j++)
                {
                    if (IsTreeCompatible(treeShadows, minRightShadow, gardenBeds, previousPlantedBedIndex, alley, i, j))
                    {
                        minRightShadow[0] = j;
                        minRightShadow[1] = treeShadows[j][1];
                        treeForGardenPositionFound = true;
                    }
                }
                if (treeForGardenPositionFound)
                {
                    previousPlantedBedIndex = i;
                    alley.Add(gardenBeds[i], minRightShadow[0]);
                }
            }
        }

        static bool IsTreeCompatible(List<int[]> treeShadows, int[] minRightShadow, int[] gardenBeds, int previousPlantedBed, Dictionary<int, int> alley, int i, int j)
        {
            bool isRightMinimal = treeShadows[j][1] <= minRightShadow[1];
            bool isLeftCompatible = treeShadows[j][0] - (gardenBeds[i] - gardenBeds[previousPlantedBed]) <= 0;
            bool isRightCompatible = treeShadows[alley[gardenBeds[previousPlantedBed]]][1] - (gardenBeds[i] - gardenBeds[previousPlantedBed]) <= 0;
            return isRightMinimal && isLeftCompatible && isRightCompatible;
        }

        static int GetMaxLength(List<int[]> treeShadows)
        {
            int maxVal = treeShadows[0][0];
            foreach (int[] tree in treeShadows)
            {
                if (tree[1] > maxVal)
                {
                    maxVal = tree[1];
                }
            }
            return maxVal;
        }

    }
}
