using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1
{
    class Combiantion
    {
        public List<List<int>> result = new List<List<int>>();
        static IList<IList<int>> Permute(int[] nums)
        {
            var list = new List<IList<int>>();
            return DoPermute(nums, 0, nums.Length - 1, list);
        }
        static bool shouldSwap(int[] num,
               int start, int curr)
        {
            for (int i = start; i < curr; i++)
            {
                if (num[i] == num[curr])
                {
                    return false;
                }
            }
            return true;
        }

        static IList<IList<int>> DoPermute(int[] nums, int start, int end, IList<IList<int>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<int>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    bool check = shouldSwap(nums, start, i);
                    if (check)
                    {
                        Swap(ref nums[start], ref nums[i]);
                        DoPermute(nums, start + 1, end, list);
                        Swap(ref nums[start], ref nums[i]);
                    }
                }
            }

            return list;
        }

        static void Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        private void CombinationRepetitionUtil(int[] chosen, int[] arr,
            int index, int r, int start, int end)
        {
            if (index == r)
            {
                int temp_mult = 1;
                int temp_add = 0;
                int[] tempList = new int[r];
                for (int i = 0; i < r; i++)
                {
                    temp_add += arr[chosen[i]];
                    temp_mult *= arr[chosen[i]];
                }
                if (temp_add == temp_mult)
                {
                    for (int i = 0; i < r; i++)
                    {
                        tempList[i] = arr[chosen[i]];
                    }
                    IList<IList<int>> temp = Permute(tempList);
                    foreach (List<int> list in temp)
                    {
                        result.Add(list);
                    }
                }
                return;
            }

            for (int i = start; i <= end; i++)
            {
                chosen[index] = i;
                CombinationRepetitionUtil(chosen, arr, index + 1,
                        r, i, end);
            }
            return;
        }

        public void CombinationRepetition(int[] arr, int n, int r)
        {
            int[] chosen = new int[r + 1];

            CombinationRepetitionUtil(chosen, arr, 0, r, 0, n - 1);
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int n = arr.Length;
            int r;
            Console.WriteLine("Please input path to input.txt and output.txt");
            string path = Console.ReadLine();
            try
            {
                r = Int32.Parse(File.ReadAllText(path + "input.txt"));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File " + path + "input.txt does not exist.");
                return;
            }

            Combiantion comb = new Combiantion();
            comb.CombinationRepetition(arr, n, r);
            List < List<int> > result = comb.result;
            if (r == 1)
            {
                result.Insert(0, new List<int>{ 0 });
            }

            List<string> first_num = new List<string>();
            foreach (int el in result[0])
            {
                first_num.Add(el.ToString());
            }
            string output = comb.result.Count + " " + string.Join("", first_num);
            File.WriteAllText(path + "output.txt", output);
        }
    }
}
