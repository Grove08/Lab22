using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Lab22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер массива: ");
            int n = Convert.ToInt32(Console.ReadLine());

            Func<object, int[]> func1 = new Func<object, int[]>(GetArray);
            Task<int[]> task1 = new Task<int[]>(func1, n);

            Func<Task<int[]>, int[]> func2 = new Func<Task<int[]>, int[]>(SortArray);

            Task<int[]> task2 = task1.ContinueWith<int[]>(func2);

            Action<Task<int[]>> action1 = new Action<Task<int[]>>(PrintSortedArray);
            Task task3 = task2.ContinueWith(action1);

            Func<Task<int[]>, int[]> func3 = new Func<Task<int[]>, int[]>(CalculateSumAndMax);

            Task<int[]> task4 = task2.ContinueWith<int[]>(func3);

            Action<Task<int[]>> action2 = new Action<Task<int[]>>(PrintResults);
            Task task5 = task4.ContinueWith(action2);

            task1.Start();
            Console.ReadKey();
        }
        static int[] GetArray(object a)
        {
            int n = (int)a;
            int[] array = new int[n];
            Random random= new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 100);
            }
            return array;
        }
        static int[] SortArray(Task<int[]> task)
        {
            int[] array = task.Result;
            for (int i = 0; i <= array.Count()-1; i++) 
            {
                for (int j = i+1; j< array.Count(); j++) 
                {
                    if (array[i] > array[j])
                    {
                        int t = array[i];
                        array[i] = array[j];
                        array[j] = t;
                    }
                }
            }return array;
        }
        static int[] CalculateSumAndMax(Task<int[]> task)
        {
            int[] array = task.Result;
            int sum = array.Sum();
            int max = array.Max();
            return new int[] { sum, max };
        }
        static void PrintSortedArray(Task<int[]> task)
        {
            int[] result = task.Result;
            Console.WriteLine("Отсортированный массив:");
            foreach (int num in result)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine();
        }
        static void PrintResults(Task<int[]> task)
        {
            int[] result = task.Result;
            Console.WriteLine($"Сумма чисел массива: {result[0]}");
            Console.WriteLine($"Максимальное число в массиве: {result[1]}");
        }

    }
}
