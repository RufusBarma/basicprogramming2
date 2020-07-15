using System;

namespace delegate_syntax_2
{
    class Program
    {
        public delegate bool TryGet<T1, T2>(T1 questionText, Action<T1> tellUser, out T2 age);

        public static void Main()
        {
            Run(AskUser, Console.WriteLine);
        }

        public static void Run(TryGet<string, int> askUser, Action<string> tellUser)
        {
            int age;
            if (askUser("What is your age?", tellUser, out age))
                tellUser("Age: " + age);
        }

        public static bool AskUser(string questionText, Action<string> tellUser, out int age)
        {
            tellUser(questionText);
            var answer = Console.ReadLine();
            return int.TryParse(answer, out age);
        }
    }
}
