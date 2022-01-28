using System.Diagnostics;
using System.Threading.Tasks;

namespace PrintEvenNumbers
{
    class StartUp
    {
        static void Main(string[] args)
        {
            Thread threadEvenNumbers = new Thread(PrintEvenNumbers);

            Thread threadOddNumbers = new Thread(PrintOddNumbers);

            threadEvenNumbers.Start();
            threadOddNumbers.Start();

            threadEvenNumbers.Join();
            threadOddNumbers.Join();
        }

        private static void PrintOddNumbers()
        {
            for (int i = 0; i <= 10; i++)
            {
                if (i % 2 != 0)
                {
                    Console.WriteLine(i);
                }
            }

            Console.WriteLine("Thread EvenNumbers finished work");
        }

        private static void PrintEvenNumbers()
        {
            for (int i = 0; i <= 10; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }

            Console.WriteLine("Thread OddNumbers finished work");
        }
    }
}