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
    /// Abstract class for sudoku puzzles - not creating any groups in it's constructor.
    /// </summary>
    public class SuDokuAbstract
    {
        /// <summary>
        /// Maximal field value (usually 9).
        /// </summary>
        protected int _max;
        /// <summary>
        /// List of all fields.
        /// </summary>
        protected List<SuDokuField> _fieldList;
        /// <summary>
        /// List of all groups. Group is a set of fields that must have different values - usually a row, column or small square.
        /// </summary>
        protected List<SuDokuGroup> _groups;

        /// <summary>
        /// Some necessary things to do in a constructor.
        /// </summary>
        private void Initialize()
        {
            _fieldList = new List<SuDokuField>();
            _groups = new List<SuDokuGroup>();
        }

        /// <summary>
        /// Creates an abstrach sudoku. 
        /// </summary>
        /// <param name="max">Maximal value - usually 9.</param>
        /// <param name="n">Number of fields - usually 81.</param>
        public SuDokuAbstract(int max, int n)
        {
            Initialize();
            _max = max;
            for (int i = 0; i < n; ++i)
                {
                    _fieldList.Add(new SuDokuField(_max, i));
                }
        }

        /// <summary>
        /// Solves the sudoku.
        /// </summary>
        /// <returns>Does a solution exist.</returns>
        public bool Solve(SuDokuAbstract copyguesses=null, Random r=null)
        {
            List<SuDokuField> undone = new List<SuDokuField>();
            foreach (var f in _fieldList) undone.Add(f);
            while (_fieldList.Exists(f => !f.IsSolved) && !_fieldList.Exists(x => x.IsContradictory))
            {
                int nroptions = _fieldList.Sum(f => f.Possible.Count);

                List<SuDokuField> todo = new List<SuDokuField>();
                foreach (var t in undone.Where(x => x.IsSolved)) todo.Add(t);

                foreach (var f in todo)
                {
                    int v = f.Value;
                    foreach (var group in _groups) if (group.Fields.Contains(f)) foreach (var field in group.Fields) if (field != f) field.Remove(v);
                    undone.Remove(f);
                }
                foreach (var group in _groups) group.Update();
                // Console.WriteLine("\n"+this+"\n");

                if (!_fieldList.Exists(x => x.IsContradictory) && nroptions == _fieldList.Sum(f => f.Possible.Count)) //brak postępu => zgadujemy
                {
                    SuDokuField f;
                    int min = _fieldList.Where(x => (x.Possible.Count > 1)).OrderBy(x => x.Possible.Count).First().Possible.Count;
                    var unknowns=_fieldList.Where(x => (x.Possible.Count == min)).ToList();
                    if (r==null) f = unknowns[0];
                    else f = unknowns[r.Next(unknowns.Count)];
                    SuDokuAbstract guess = new SuDokuAbstract(this);
                    int val;
                    if(r==null) val = f.Possible[0];
                    else val=f.Possible[r.Next(f.Possible.Count)];
                    guess._fieldList[f.ID].Set(val);
                    if (guess.Solve(copyguesses,r))
                    {
                        if (copyguesses != null) copyguesses._fieldList[f.ID].Set(val);
                        foreach (var fi in _fieldList) fi.Set(guess._fieldList[fi.ID].Value);
                        return true;
                    }
                    else f.Remove(val);
                }
            }
            foreach (var f in _fieldList)
            {
                int v = f.Value;
                foreach (var group in _groups) if (group.Fields.Contains(f)) foreach (var field in group.Fields) if (field != f) field.Remove(v);
            }
            if (_fieldList.Exists(x => x.IsContradictory)) return false;
            return true;
        }

        /// <summary>
        /// Copy constructor - clones everything.
        /// </summary>
        /// <param name="source">Object to copy.</param>
        public SuDokuAbstract(SuDokuAbstract source)
        {
            _max = source._max;
            Initialize();
            foreach (var fi in source._fieldList)
            {
                var f = new SuDokuField(_max, fi.ID);
                f.CopyPossible(fi.Possible);
                _fieldList.Add(f);
            }

            foreach (var group in source._groups)
            {
                List<SuDokuField> newg = new List<SuDokuField>();
                foreach (var f in group.Fields) newg.Add(_fieldList[f.ID]);
                _groups.Add(new SuDokuGroup(newg));
            }
        }

        /// <summary>
        /// Sets the object to a new, (usually) random sudoku, having exactly one solution and no unnecessary fields given.
        /// </summary>
        /// <param name="randomize">If set to true, new puzzle will be created randomly.</param>
        /// <param name="r">Random used in the creation, if not given, a new Random object will be created.</param>
        public void SetToRandomSuDoku(bool randomize=true, Random r=null)
        {
            if (randomize && r == null) r = new Random();
            foreach (var f in _fieldList) f.Reset();
            SuDokuAbstract blank = new SuDokuAbstract(this);

            if (!blank.Solve(this, r)) throw new SystemException("Impossible sudoku rules.");

            //Console.WriteLine(this);

            foreach (var f in _fieldList) //it would be nice to take them in random order, too.
                if (f.Value != 0)
                {
                    //Console.Write(f.ID+", ");
                    SuDokuAbstract copy = new SuDokuAbstract(this);
                    copy._fieldList[f.ID].Reset();
                    copy._fieldList[f.ID].Remove(f.Value);
                    if (!copy.Solve()) f.Reset();
                }
            //Console.WriteLine();

        }

     
    }
}
