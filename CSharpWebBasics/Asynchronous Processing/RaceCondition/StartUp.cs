namespace RaceCondition
{
    public class StartUp
    {
        private static object lockObject = new object();

        public static void Main()
        {
            List<int> numbers = Enumerable
                .Range(0, 11)
                .ToList();

            RunNumbersLock(numbers);
            RunNumbersNoLock(numbers);
        }

        private static void RunNumbersNoLock(List<int> numbers)
        {
            for (int i = 0; i < 4; i++)
            {
                new Thread(() =>
                {
                    while (numbers.Count > 0)
                    {
                        numbers.RemoveAt(numbers.Count - 1);
                    }
                }).Start();
            }
        }

        private static void RunNumbersLock(List<int> numbers)
        {
            for (int i = 0; i < 4; i++)
            {

                Thread thread = new Thread(() =>
                {
                    while (numbers.Count > 0)
                    {
                        lock (lockObject)
                        {
                            if (numbers.Count == 0)
                            {
                                Console.WriteLine("Time to break");
                                break;
                            }

                            numbers.RemoveAt(numbers.Count - 1);
                            Console.WriteLine(numbers.Count);
                        }
                    }
                });

                thread.Start();
            }
        }
    }
}

