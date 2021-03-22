using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var iterations = iterationsCount / threadsCount;
            var tasks = new Task<Tuple<Turn, double>>[threadsCount];
            for (var i = 0; i < threadsCount; i++)
            {
                tasks[i] = new Task<Tuple<Turn, double>>(
                    () => SearchBestMove(rocket, new Random(random.Next()), iterations));
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            var bestMove = tasks
                .OrderBy(o => o.Result.Item2)
                .First()
                .Result;
            var newRocket = rocket.Move(bestMove.Item1, level);
            return newRocket;
        }
    }
}