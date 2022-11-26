using System;
using SherlocksGambit.Game;
using SherlocksGambit.Utils.Helpers;
using SherlocksGambit.Utils.Runners.DotTree;

namespace SherlocksGambit
{
    public static class Program
    {
        public static void Main()
        {
            // Write your tests here! (Please do remove them before handing in your work)

            Board b = new Board("mmmm/3m/mm2/4/1M2/M1Mm/3M/MMMM");
            
            var newBrain = new Utils.AiProperties((int)PlayerColor.Black, b, 7, AI.Heuristic.PositionWeightedHeuristics);
            var newBrainW = new Utils.AiProperties((int)PlayerColor.White, b, 7, AI.Heuristic.PositionWeightedHeuristics);
            
            var blackAI = new AI.MiniMax(newBrain);
            var whiteAI = new AI.MiniMax(newBrainW);
            var runner = new Utils.Runners.Runner(whiteAI, blackAI, print: true);
            
            runner.Run();
        }
    }
}