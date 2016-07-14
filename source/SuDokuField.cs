//Copyright (C) 2011 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuDoku
{
    public class SuDokuField
    {
        private readonly int _id;
        private readonly int _max;
        /// <summary>
        /// Field ID number.
        /// </summary>
        public int ID 
        {
            get
            {
            return _id;
            }
        }
        /// <summary>
        /// List of possible values.
        /// </summary>
        public List<int> Possible{get; private set;}

        /// <summary>
        /// Removes a possible value.
        /// </summary>
        /// <param name="k">Value to remove</param>
        public void Remove(int k)
        {
            Possible.Remove(k);
        }

        /// <summary>
        /// Copies the list of possible values into the field --- used in the copy constructor of SuDokuAbstract class.
        /// </summary>
        /// <param name="p">Lista of possible values.</param>
        public void CopyPossible(List<int> p)
        {
            Possible.Clear();
            foreach (var v in p)
            {
                if (v < 0 || v > _max) throw new ArgumentOutOfRangeException("Wrong value: " + v + " being copied to field " + ID + "; values should be from 1 to " + _max + ".");
                Possible.Add(v);
            }
        }

        /// <summary>
        /// Sets the field to a given value if possible; if impossible, sets it as contradictory. Used with SuDokuGroup.Update().
        /// </summary>
        /// <param name="k">Value that must be in this field.</param>
        public void Set(int k)
        {
            if (k < 0 || k > _max) throw new ArgumentOutOfRangeException("Wrong value: "+k+" being set to field "+ID+"; values should be from 1 to "+_max+".");
            if (Possible.Contains(k))
            {
                Possible.Clear();
                Possible.Add(k);
            }
            else Possible.Clear();
        }

        /// <summary>
        /// Is the field solved (has exactly one value possible).
        /// </summary>
        public bool IsSolved { get { return (Possible.Count == 1); } }
        /// <summary>
        /// Is the field contradictory (has no possible value).
        /// </summary>
        public bool IsContradictory { get { return (Possible.Count == 0); } }
        /// <summary>
        /// Value, or 0 if it's unknown.
        /// </summary>
        public int Value { get { if (IsSolved) return Possible[0]; return 0; } }

        public void Reset()
        {
            Possible = Enumerable.Range(1, _max).ToList();
        }

        /// <summary>
        /// Creates a new field.
        /// </summary>
        /// <param name="max">Maximal value.</param>
        /// <param name="id">Field number.</param>
        /// <param name="value">Value (the number in this field) or 0 if unknown.</param>
        public SuDokuField(int max, int id, int value=0)
        {
            _max = max;
            _id = id;
            if (value < 0 || value > _max) throw new ArgumentOutOfRangeException("Wrong value: " + value + " being set to field " + ID + " in it's constructor; values should be from 1 to " + _max + ".");
            if (value == 0) Possible = Enumerable.Range(1, max).ToList();
            else Possible = new List<int>() { value };
        }

        /// <summary>
        /// Field value expressed in one char, used in ToString() functions. ! - contradictory; ? - unknown; 1-9 (or A, B, ... if maximal value > 9) for known.
        /// </summary>
        public char CharVal
        {
            get
            {
            if (IsContradictory) return '!';
            if (IsSolved)
            {
                if (Value<10) return Convert.ToChar(Value + '0');
                else return  Convert.ToChar(Value + 'A'-10);
            }
            return '_';
            }
        }

        public override string ToString()
        {
            return ""+ID+":   "+String.Join(",  ",Possible);
        }
    }
}
