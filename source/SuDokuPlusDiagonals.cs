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
    /// Classic + one type of diagonal lines.
    /// </summary>
    public class SuDokuPlusDiagonals : SuDokuClassic
    {
          public SuDokuPlusDiagonals(int[,] array)
            :base(array)
        {
            AddDiagonals();
        }

          public SuDokuPlusDiagonals(int max = 9)
            : base(max)
        {
            AddDiagonals();
        }

          protected void AddDiagonals()
        {
            for (int i = 0; i < _max; ++i)
            {
                List<SuDokuField> left = Enumerable.Range(0,_max).Select(j=>Field(j,(i-j+_max)%_max)).ToList();
                _groups.Add(new SuDokuGroup(left));
            }
 /* for (int i = 0; i < _max; ++i)
            {
                List<SuDokuField> left = _fieldList.Where(f=>((f.X-f.Y+_max)%_max==i)).ToList();
                _groups.Add(new SuDokuGroup(left));
            }*/ //contradictory
          
        }

    }
}
