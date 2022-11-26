using System;
using System.Collections.Generic;
using System.Data;
using SherlocksGambit.Game.Pieces;

namespace SherlocksGambit.Game
{
    public class PathObject
    {
        /// <summary> Lists of cells representing the path to follow </summary>
        /// <remarks> Enemy cells are not added to this list </remarks> 
        public readonly List<Cell> Path;
        
        /// Lists of enemy pieces encountered on the way
        public readonly List<BasePiece> Enemies;

        /**
         * <summary> Basic constructor for PathObject </summary>
         * <param name="path"> List of Cells representing the path, ready to assign </param>
         * <param name="enemies"> List of enemies as BasePieces, ready to assign </param>
         * Initialize all attributes on this class
         */
        public PathObject(List<Cell> path, List<BasePiece> enemies)
        {
            Path = path;
            Enemies = enemies;
        }

        /**
         * <summary> Creates a clone of this PathObject in a new referential, the targeted Board </summary>
         * <param name="targetBoard"> Board being the new referential of this PathObject </param>
         * <returns> The cloned PathObject </returns>
         * <remarks> Be very mindful of the difference of the board notation and the array notation </remarks>
         */
        public PathObject Translate(Board targetBoard)
        {
            List<Cell> pathC = new List<Cell>();
            List<BasePiece> enemiesC = new List<BasePiece>();

            foreach (Cell cell in Path)
            {
                int i = cell.BoardPosition - 1;
                Cell c = targetBoard.Positions[i];
                pathC.Add(c);
            }

            foreach (BasePiece piece in Enemies)
            {
                int i = piece.CurrentCell.BoardPosition - 1;
                BasePiece enemy = targetBoard.Positions[i].CurrentPiece;
                enemiesC.Add(enemy);
            }
            
            return new PathObject(pathC, enemiesC);
        }

        /**
         * <summary> [BONUS] Checks if a given object is equal to this PathObject </summary>
         * <param name="obj"> Board being the new referential of this PathObject </param>
         * <returns> A boolean whether the two objects were equal </returns>
         * <remarks>
         * `obj` can be null or of another type then PathObject... Have a look at conditional pattern matching!  
         * To test for equality, the paths need to be the same and must encounter the same type of enemies at the same
         * position. However, cells and cell positions are not in the same referential
         * </remarks>
         */
        public override bool Equals(object obj)
        {
            throw new NotImplementedException("TODO: REMOVE THIS!");
        }
        
        /// Returns an hash code for this PathObject
        public override int GetHashCode()
        {
            return HashCode.Combine(Path, Enemies);
        }
    }
}