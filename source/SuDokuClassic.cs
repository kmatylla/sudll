//Copyright (C) 2011 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuDoku
{
    /// <summary>
    /// Classic zudoku puzzle with rows, columns and small squares.
    /// </summary>
    public class SuDokuClassic : SuDokuAbstract
    {
        /// <summary>
        /// Sqare root of size. By default 3, which gives us 9 * 9 puzzle.
        /// </summary>
        public int SqrtSize { get { return (int)Math.Sqrt(_max); } }
        /// <summary>
        /// Creates a sudoku from given number array.
        /// </summary>
        /// <param name="array">SQUARE array. It should contain numbers from 0 to size (in one dimansion) - 0 means unknown (to be guessed). For example, 9*9 array should have numbers from 0 to 9.</param>
        public SuDokuClassic(int[,] array):base(array.GetLength(0),array.GetLength(0)*array.GetLength(1))
        {
            if (array.GetLength(0)!=array.GetLength(1)) throw new ArgumentException("Source array given for square sudoku is not square ("+array.GetLength(0)+" * "+array.GetLength(1)+").");
            for (int x = 0; x < _max;++x ) for(int y=0;y<_max;++y) if (array[x,y]!=0) Field(x,y).Set(array[x,y]);
            InitializeGroups();
        }
        /// <summary>
        /// Creates a sudoku of given size.
        /// </summary>
        /// <param name="max">Width (also, height --- they're equal).</param>
        public SuDokuClassic(int max)
            : base(max,max*max)
        {
            if ((int)(Math.Sqrt(max)*(int)Math.Sqrt(max))!=max) throw new ArgumentException("Size "+max+" given for square sudoku is not a square number.");
            InitializeGroups();
        }

        public SuDokuClassic() : this(9) { }
    
        /// <summary>
        /// Creates field groups - rows, columns, squares.
        /// </summary>
        protected void InitializeGroups()
        { 
            for (int i = 0; i < _max; ++i)
            {
                List<SuDokuField> r = Enumerable.Range(0,_max).Select(j=>Field(j,i)).ToList();
                _groups.Add(new SuDokuGroup(r));
            }
            for (int i = 0; i < _max; ++i)
            {
                List<SuDokuField> c = Enumerable.Range(0, _max).Select(j => Field(i, j)).ToList();
                _groups.Add(new SuDokuGroup(c));
            }


            for (int i = 0; i < SqrtSize; ++i) for (int j = 0; j < SqrtSize; ++j)
            {
                List<SuDokuField> b = Enumerable.Range(0, _max).Select(x => Field(i * SqrtSize + (x % SqrtSize), j * SqrtSize + (x / SqrtSize))).ToList();
                _groups.Add(new SuDokuGroup(b));
            }

        }

        public override string ToString()
        {
            return String.Join("\n" + String.Join("", Enumerable.Range(0, 2 * _max + 2 * SqrtSize - 3).Select(s => "-")) + "\n", Enumerable.Range(0, SqrtSize).Select(y => String.Join("\n", Enumerable.Range(0, SqrtSize).Select(j => String.Join(" | ", Enumerable.Range(0, SqrtSize).Select(x => String.Join(" ", Enumerable.Range(0, SqrtSize).Select(i => Field(x * SqrtSize + i, y * SqrtSize + j).CharVal))))))));
        }

        /// <summary>
        /// Returns field at posistion (x,y).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public SuDokuField Field(int x, int y)
        {
            return _fieldList[x+_max*y];
        }

        public int X(SuDokuField f)
        {
            return f.ID % _max;
        }

        public int Y(SuDokuField f)
        {
            return f.ID / _max;
        }
    }
}
