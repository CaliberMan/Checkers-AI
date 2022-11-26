using System;
using System.Linq;
using SherlocksGambit.Game;
using SherlocksGambit.Game.Pieces;
using SherlocksGambit.Utils.Helpers;

namespace SherlocksGambit.Utils.Encryption
{
    public static class MoveEncryption
    {
        /**
         * <summary> Encrypts a move given a PlayerColor and a PathObject </summary>
         * <param name="playerColor"> Color of the player making the move </param>
         * <param name="path"> Object containing the path and enemies encountered </param>
         * <returns> A string representing the move </returns>
         * <example>
         * White's simple move: "22-17"  
         * Black's capture move: "(10x19)"
         * </example>
         */
        public static string Encrypt(PlayerColor playerColor, PathObject path)
        {
            string s = "";
            if (playerColor == PlayerColor.Black)
                s += "(";
            s += path.Path[0].BoardPosition;
            if (path.Enemies.Count != 0)
                s += "x";
            else
                s += "-";

            s += path.Path[path.Path.Count - 1].BoardPosition;
            if (playerColor == PlayerColor.Black)
                s += ")";
            return s;
        }
        
        // We assume that `input` is correct (meaning nor null, nor empty, with correct coordinates and format)
        /**
         * <summary> Parses a given move and executes it on a given Board </summary>
         * <param name="board"> Board on which to perform the move </param>
         * <param name="input"> Encoded move to decrypt </param>
         * <returns> A boolean indicating if the move was successful or not </returns>
         * <remarks>
         * If `input` is null, return true. It being null signify that this is is first move of the game  
         * Otherwise, we assume that `input` is correct (meaning not empty, with correct coordinates and format)
         * </remarks>
         */
        public static bool Decrypt(Board board, string input)
        {
            if (input is null or "")
                return true;
            PlayerColor color = input[0] == '(' ? PlayerColor.Black : PlayerColor.White;
            input = color == PlayerColor.Black ? input.Substring(1, input.Length - 2) : input;

            int i = 0;
            int pos = 0;
            while (i < input.Length)
            {
                if (Char.IsDigit(input[i]))
                    pos = pos * 10 + (int)Char.GetNumericValue(input[i]);
                else
                    break;
                i++;
            }

            char action = input[i];
            int target = Convert.ToInt32(input.Substring(i + 1));
            
            return board.Positions[pos - 1].CurrentPiece.Move(board.Positions[target - 1], action);
        }
    }
}