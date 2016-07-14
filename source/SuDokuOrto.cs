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
    /// Classic + every value is in different position in each square.
    /// </summary>
    public class SuDokuOrto : SuDokuClassic
    {
        
        public SuDokuOrto(int[,] array):base(array)
        {
            AddBoxes();
        }

        public SuDokuOrto(int max = 9)
            : base(max)
        {
            AddBoxes();
        }

        private void AddBoxes()
        {
            for (int i = 0; i < SqrtSize; ++i) for (int j = 0; j < SqrtSize; ++j)
                {
                    List<SuDokuField> b = Enumerable.Range(0, _max).Select(n => Field(SqrtSize * (n / SqrtSize) + i, SqrtSize * (n % SqrtSize) + j)).ToList();
                    _groups.Add(new SuDokuGroup(b));
                }
        }


    }
}
