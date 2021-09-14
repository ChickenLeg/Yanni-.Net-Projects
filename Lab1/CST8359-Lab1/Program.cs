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
                displayMenu();
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
                        Console.WriteLine(readFile());
                        break;
                    case '2':
                        timeCount(() => bubbleSortStrings());
                        break;
                    case '3':
                        timeCount(() => lambdaSort());
                        break;
                    case '4':
                        countDistinct();
                        break;
                    case '5':
                        firstTen();
                        break;
                    case '6':
                        wordStartWithJ();
                        break;
                    case '7':
                        wordEndWithD();
                        break;
                    case '8':
                        wordLongerThanFour();
                        break;
                    case '9':
                        wordLessThanThreeAndStartWithA();
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
        private static void displayMenu()
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
        private static void createFileIList(IList<string> words)
        {
            using StreamReader sr = new StreamReader(@"C:\Users\Yanni\source\repos\CST8359\Lab1\CST8359-Lab1\Words.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                words.Add(line);
            }
            sr.Close();
        }
        private static string readFile()
        {
            try
            {
                words.Clear();
                createFileIList(words);
                string message = "There are " + words.Count + " words in the file.";
                return message;
            }
            catch (Exception e)
            {
                string errorMessage = "Exception when reading file: " + e.Message;
                return errorMessage;
            }

        }
        private static void bubbleSortStrings()
        {
            string[] arr = File.ReadAllLines(@"C:\Users\Yanni\source\repos\CST8359\Lab1\CST8359-Lab1\Words.txt");
            int n = arr.Length;
            String temp;
            // Sorting strings using bubble sort
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
        }
        private static void lambdaSort()
        {
            IEnumerable<string> sortedEnum = words.OrderBy(f => f.Length);
            IList<string> sortedList = sortedEnum.ToList();
        }
        private static void countDistinct()
        {
            int count = words.Distinct().Count();
            Console.WriteLine("There are " + count + " dictinct words.");
        }
        private static void firstTen()
        {
            string[] selectedWords = words.Take(10).Select(i => i.ToString()).ToArray();
            foreach (var n in selectedWords)
                Console.WriteLine("First ten words are : " + n);
        }
        private static void wordStartWithJ()
        {
            var resultList = words.Where(r => r.StartsWith("j"));
            Console.WriteLine("Number of words start with j are : " + resultList.Count());
            foreach (var n in resultList)
                Console.WriteLine("Words start with j are : " + n);
        }
        private static void wordEndWithD()
        {
            var resultList = words.Where(r => r.EndsWith("d"));
            Console.WriteLine("Words end with d are : " + resultList.Count());
            //foreach (var n in resultList)
            //    Console.WriteLine("Words end with d are : " + n);
        }
        private static void wordLongerThanFour()
        {
            var query = words.Where(s => s.Length > 4);
            foreach (var n in query)
                Console.WriteLine("Words longer than four are : " + n);
            Console.WriteLine("Number of words longer than four are : " + query.Count());
        }
        private static void wordLessThanThreeAndStartWithA()
        {
            var query = words.Where(s => s.Length < 3 && s.StartsWith("a"));
            foreach (var n in query)
                Console.WriteLine("Words less than three and start with a are: " + n);
            Console.WriteLine("Number of words less than three and start with a are: " + query.Count());
        }
        private static void timeCount(Action myMethod)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            myMethod.Invoke();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time elapsed: " + elapsedMs);
        }

    }
}
