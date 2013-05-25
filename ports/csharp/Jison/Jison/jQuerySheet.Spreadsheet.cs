﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jison;
using System.Text.RegularExpressions;

namespace jQuerySheet
{
	public class Spreadsheet : FormulaDefinition
    {
		static public Dictionary<int, Dictionary<int,Dictionary<int, SpreadsheetCell>>> Spreadsheets = new Dictionary<int, Dictionary<int,Dictionary<int, SpreadsheetCell>>>()
			{
				{0, new Dictionary<int, Dictionary<int, SpreadsheetCell>>()
					{
						{0, new Dictionary<int, SpreadsheetCell>()
							{
								{0, new SpreadsheetCell(0,0,0, "50")},
								{1, new SpreadsheetCell(0,0,1, "777", "A1")}
							}
						}
					}
				}
			};

		public static DateTime CalcLast;

		public static void Main()
		{
			var parser = new Spreadsheet();
			var o = parser.Parse("SUM(SUM(A1) + 100)");
			o = o;
		}

		public static ParserValue UpdateCellValue(SpreadsheetCell cell)
        {
            if (cell.HasFormula && cell.State.Count < 1)
            {
                cell.State.Push("Parsing");
				cell.CalcCount++;
				cell.CalcLast = CalcLast;
                var parser = new Spreadsheet();
                var value = parser.Parse(cell.Formula);
                cell.State.Pop();
                return value;
            }
            return cell.ParserValue;
        }

		public Dictionary<int[], SpreadsheetCell> CellRangeValue(string start, string end)
        {
            var _start = new SpreadsheetCellLocation(start);
            var _end = new SpreadsheetCellLocation(end);
            var range = new Dictionary<int[], SpreadsheetCell>();
            
            for (var row = _start.Row; row < _end.Row; row++)
            {
                for (var col= _start.Col; col < _end.Col; col++)
                {
                    range.Add(new int[]{row, col}, new SpreadsheetCell(0, row, col));
                }
            }

            return range;
        }
    }

	public class SpreadsheetCell
    {
        public int Row;
        public int Cell;
        public int Spreadsheet;
        public string Value;
        public ParserValue ParserValue;
        public Boolean HasFormula;
		public string Formula;

		public DateTime CalcLast = new DateTime();
		public int CalcCount = 0;

		public Stack<string> State = new Stack<string>();

		public SpreadsheetCell(int spreadsheet, int row, int cell)
		{
			Spreadsheet = spreadsheet;
			Row = row;
			Cell = cell;
		}
		public SpreadsheetCell(int spreadsheet, int row, int cell, string value)
		{
			Spreadsheet = spreadsheet;
			Row = row;
			Cell = cell;
			Value = value;
		}
		public SpreadsheetCell(int spreadsheet, int row, int cell, string value, string formula)
		{
			Spreadsheet = spreadsheet;
			Row = row;
			Cell = cell;
			Value = value;
			Formula = formula;
		}
    }

    public class FormulaVariables
    {

    }

    public class SpreadsheetCellLocation
    {
        public int Row = -1;
        public int Col = -1;
		public Dictionary<string, int> Alphabet = new Dictionary<string, int>()
			{
				{"A", 0},
				{"B", 1},
				{"C", 2},
				{"D", 3},
				{"E", 4},
				{"F", 5},
				{"G", 6},
				{"H", 7},
				{"I", 8},
				{"J", 9},
				{"K", 10},
				{"L", 11},
				{"M", 12},
				{"N", 13},
				{"O", 14},
				{"P", 15},
				{"Q", 16},
				{"R", 17},
				{"S", 18},
				{"T", 19},
				{"U", 20},
				{"V", 21},
				{"W", 22},
				{"X", 23},
				{"Y", 24},
				{"Z", 25}
			};
        static readonly Regex Cell = new Regex("^([A-Z]+)([0-9]+)");

        public SpreadsheetCellLocation(string id)
        {
            var match = Cell.Match(id);
            if (match.Success)
            {
                Col = Alphabet[match.Groups[1].Value];
                Row = Convert.ToInt32(match.Groups[2].Value) - 1;
            }
        }
    }
}
