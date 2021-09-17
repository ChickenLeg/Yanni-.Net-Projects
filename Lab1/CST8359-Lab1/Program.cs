using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CST8359_Lab1
{
    class Program
    {
        protected static IList<string> words = new List<string>();

        static void Main(string[] args)
        {
            char choice = 'c';
            do
            {
                DisplayMenu();
                string userInput = Console.ReadLine();
                try
                {
                    choice = Convert.ToChar(userInput);
                }
                catch (FormatException)
                {
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("|'{0}' is not in the correct format for conversion to a Char. |",
                                      userInput);
                    Console.WriteLine("-------------------------------------------------------------");
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("| A null string cannot be converted to a Char. |");
                    Console.WriteLine("-------------------------------------------------------------");
                }
                switch (choice)
                {
                    case '1':
                        Console.WriteLine(ReadFile());
                        break;
                    case '2':
                        words.Clear();
                        // Everytime when case 2 runs, it calls CreateFileList method, thus it cannot be time count 0
                        TimeCount(() => BubbleSort(CreateFileIList(words)));
                        break;
                    case '3':
                        TimeCount(() => LambdaSort());
                        break;
                    case '4':
                        CountDistinct();
                        break;
                    case '5':
                        FirstTen();
                        break;
                    case '6':
                        WordStartWithJ();
                        break;
                    case '7':
                        WordEndWithD();
                        break;
                    case '8':
                        WordLongerThanFour();
                        break;
                    case '9':
                        WordLessThanThreeAndStartWithA();
                        break;
                    case 'x':
                        Console.WriteLine("Exiting... ");
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            } while (choice != 'x');
        }
        private static void DisplayMenu()
        {

            Console.WriteLine();
            Console.WriteLine("Hello World!!! My first C# APP!");
            Console.WriteLine("Options: ");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("1 - Import Words From File");
            Console.WriteLine("2 - Bubble Sort words");
            Console.WriteLine("3 - LINQ/Lambda sort words");
            Console.WriteLine("4 - Count the Distinct Words");
            Console.WriteLine("5 - Take the first 10 words");
            Console.WriteLine("6 - Get the number of words that start with 'j' and display the count");
            Console.WriteLine("7 - Get and display of words that end with 'd' and display the count");
            Console.WriteLine("8 - Get and display of words that are greater than 4 characters long, and display the count");
            Console.WriteLine("9 - Get and display of words that are less than 3 characters long and start with the letter 'a', and display the count");
            Console.WriteLine("x - Exit");
            Console.WriteLine();
            Console.WriteLine("Make a selection: ");
        }
        private static IList<string> CreateFileIList(IList<string> words)
        {
            using StreamReader sr = new StreamReader(@"C:\Users\Yanni\source\repos\CST8359\CST8359-Yanni-Li-Labs\Lab1\CST8359-Lab1\Words.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                words.Add(line);
            }
            sr.Close();
            
            return words;
        }
        private static string ReadFile()
        {
            try
            {
                words.Clear();
                CreateFileIList(words);
                Console.WriteLine("Reading file ... ");
                string message = "There are " + words.Count + " words in the file.";
                Console.WriteLine("Reading complete. ");
                return message;
            }
            catch (Exception e)
            {
                string errorMessage = "Exception when reading file: " + e.Message;
                return errorMessage;
            }

        }
        public static IList<string> BubbleSort(IList<string> arr)
        {
            int n = arr.Count;
            String temp;
            for (int j = 0; j < n - 1; j++)
            {
                for (int i = j + 1; i < n; i++)
                {
                    if (arr[j].CompareTo(arr[i]) > 0)
                    {
                        temp = arr[j];
                        arr[j] = arr[i];
                        arr[i] = temp;
                    }
                }
            }
            return arr;
        }

        private static void LambdaSort()
        {
            IEnumerable<string> sortedEnum = words.OrderBy(f => f.Length);
            IList<string> sortedList = sortedEnum.ToList();
        }
        private static void CountDistinct()
        {
            int count = words.Distinct().Count();
            Console.WriteLine("There are " + count + " dictinct words.");
        }
        private static void FirstTen()
        {
            string[] selectedWords = words.Take(10).Select(i => i.ToString()).ToArray();
            foreach (var n in selectedWords)
                Console.WriteLine("First ten words are : " + n);
        }
        private static void WordStartWithJ()
        {
            var resultList = words.Where(r => r.StartsWith("j"));
            Console.WriteLine("Number of words start with j are : " + resultList.Count());
            foreach (var n in resultList)
                Console.WriteLine("Words start with j are : " + n);
        }
        private static void WordEndWithD()
        {
            var resultList = words.Where(r => r.EndsWith("d"));
            Console.WriteLine("Words end with d are : " + resultList.Count());
            //foreach (var n in resultList)
            //    Console.WriteLine("Words end with d are : " + n);
        }
        private static void WordLongerThanFour()
        {
            var query = words.Where(s => s.Length > 4);
            foreach (var n in query)
                Console.WriteLine("Words longer than four are : " + n);
            Console.WriteLine("Number of words longer than four are : " + query.Count());
        }
        private static void WordLessThanThreeAndStartWithA()
        {
            var query = words.Where(s => s.Length < 3 && s.StartsWith("a"));
            foreach (var n in query)
                Console.WriteLine("Words less than three and start with a are: " + n);
            Console.WriteLine("Number of words less than three and start with a are: " + query.Count());
        }
        private static void TimeCount(Action myMethod)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            myMethod.Invoke();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time elapsed: " + elapsedMs);
        }

    }
}
