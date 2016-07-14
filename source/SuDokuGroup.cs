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
    /// Group of _max fields that should have different values (1.._max). In classic sudoku a row, column or small square.
    /// </summary>
    public class SuDokuGroup
    {
        /// <summary>
        /// Fields in the group.
        /// </summary>
        public List<SuDokuField> Fields { get; private set; }
        /// <summary>
        /// Maximal value. Also, the size of the group.
        /// </summary>
        private int _max;

        /// <summary>
        /// All fields that may have given value (possibly other values, too).
        /// </summary>
        /// <param name="n">Value</param>
        /// <returns>List of such fields.</returns>
        private List<SuDokuField> possible(int n)
        {
            return Fields.Where(f => f.Possible.Contains(n)).ToList();
        }
        
        /// <summary>
        /// Creates a group of fields.
        /// </summary>
        /// <param name="l">List of fields in the group.</param>
        public SuDokuGroup(List<SuDokuField> l)
        {
            _max = l.Count;
            Fields = l;
        }

        /// <summary>
        /// Checks for a value possible in only one field in the group. if such value exists, it's inserted into this field.
        /// </summary>
        public void Update()
        {
            for (int i = 1; i < _max+1; ++i)
            {
                List<SuDokuField> p = possible(i);
                if (p.Count == 1) p[0].Set(i);
            }
        }

        public override string ToString()
        {
            return String.Join(";  ",Fields.Select(f=>f.ID));
        }
    }
}
