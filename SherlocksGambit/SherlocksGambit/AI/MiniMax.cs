using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SherlocksGambit.Game;
using SherlocksGambit.Utils;
using SherlocksGambit.Utils.Bonuses;
using SherlocksGambit.Utils.Encryption;
using SherlocksGambit.Utils.Helpers;
using SherlocksGambit.Utils.Runners.DotTree;

namespace SherlocksGambit.AI
{
    public class MiniMax : AiBase
    {
        /**
         * <summary> Basic constructor for MiniMax </summary>
         * <remarks> This should stay empty as we are simply calling the superclass' constructor </remarks>
         */
        public MiniMax(AiProperties properties) : base(properties)
        {
        }
        
        /**
         * <summary> Return the max of the 2 parameters </summary>
         * <param name="a"> First number to compare </param>
         * <param name="b"> Second number to compare </param>
         * <returns> The biggest out of the twp </returns>
         * <remarks> This function was added to support reflection used in the client and leaderboard </remarks>
         */
        private static double MaxDouble(double a, double b) => a > b ? a : b;
        
        /**
         * <summary> Return the min of the 2 parameters </summary>
         * <param name="a"> First number to compare </param>
         * <param name="b"> Second number to compare </param>
         * <returns> The biggest out of the twp </returns>
         * <remarks> This function was added to support reflection used in the client and leaderboard </remarks>
         */
        private static double MinDouble(double a, double b) => a < b ? a : b;

        /**
         * <summary> Generates a move </summary>
         * <param name="currBoard"> Current Board on which to perform the search </param>
         * <param name="currDepth"> Current depth of the search </param>
         * <param name="minimize"> Indicate whether you are trying to minimize or maximize the heuristic </param>
         * <param name="alpha"> [Optional, default = Min] Current Alpha of the search </param>
         * <param name="beta"> [Optional, default = Maz] Current Beta of the search </param>
         * <param name="root"> [Optional, default = null] Used for dot printing, current node of the tree </param>
         * <param name="quietMode"> [Optional, default = false] Indicate if you are in a Quiescence search </param>
         * <returns>
         * A tuple with the as key the heuristic returned from the search and as value the PathObject which yields this
         * heuristic
         * </returns>
         * <remarks>
         * You are alternating the search's PlayerColor depending on `minimize`. Consider using `GetOpponentColor()`  
         * Start by implementing the version without Alpha-Beta pruning. Adding this optimisation will be easier on code
         * that already works!  
         * To call the heuristic function use Func.Invoke(). This should only be called when max depth has been reach!
         * As a reminder, max depth 0 should not return any move  
         * We are showing you how to implement the DotVisualizer. Refer to the subject to add this tool to this function
         * Friendly reminder to no confuse board notation with array notation!  
         * Do not forget to copy the Board (if you are not doing the RevertMove bonus) because such Boards would be
         * passed as reference (which would, this node's children, change the current Board)  
         * You should update the AI's `ExploredPath` stat in here  
         * The MiniMax algorithm is by definition recursive!  
         * [BONUS]  
         * It is in this function that you are expected to implement the Quiescence Search, TranspositionTable,
         * MoveOrdering, MoveHistory and RevertMove
         * </remarks>
         */
        protected override Tuple<double, PathObject> GenerateMove(Board currBoard, int currDepth, bool minimize = false,
            double alpha = Min, double beta = Max, DotNode root = null, bool quietMode = false)
        {
            if (currDepth == 0)
            {
                return new Tuple<double, PathObject>(HeuristicFunction.Invoke(currBoard, Color), null);
            }
            if (!minimize)
            {
                double value = Min;
                PathObject obj = null;
                
                List<PathObject> paths = GeneratePaths(currBoard, Color);

                // iterate on the copy
                foreach (PathObject pathObject in paths)
                {
                    ExploredPaths += 1;
                    
                    Board b = currBoard.Copy();
                    PathObject p = pathObject.Translate(b);
                    // move the piece
                    p.Path[0].CurrentPiece.Move(p);
                    
                    // check the generated moves
                    Tuple<double, PathObject> j = GenerateMove(b, currDepth - 1, true, alpha, beta);
                    double n = j.Item1;
                    if (value < n)
                    {
                        value = n;
                        obj = p;
                    }

                    if (value >= beta)
                        break;
                    alpha = Math.Max(alpha, value);
                }
                
                return new Tuple<double, PathObject>(value, obj);
            }

            else
            {
                double value = Max;
                PathObject obj = null;
                List<PathObject> paths = GeneratePaths(currBoard, ColorHelper.GetOpponentColor(Color));

                foreach (PathObject pathObject in paths)
                { 
                    ExploredPaths += 1;
                    
                    Board b = currBoard.Copy();
                    PathObject p = pathObject.Translate(b);
                    p.Path[0].CurrentPiece.Move(p);

                    Tuple<double, PathObject> j = GenerateMove(b, currDepth - 1, alpha: alpha, beta: beta);
                    double n = j.Item1;
                    if (value > n)
                    {
                        value = n;
                        obj = p;
                    }

                    if (value <= alpha)
                        break;
                    beta = Math.Min(value, beta);
                }

                return new Tuple<double, PathObject>(value, obj);
            }
        }
    }
}