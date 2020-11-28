using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Drawer
{
    class Parser
    {
        private static readonly Parser singleInstance = new Parser();
        public static Parser Instance { get {return singleInstance;} }
        public List<Line> parse(string filePath)
        {
            string[] File = System.IO.File.ReadAllLines(filePath);
            var lines = new List<Line>();
            foreach (string line in File)
            {
                string[] numbers = line.Split(',');
                UInt64 lineNumber = UInt64.Parse(numbers[0]);
                decimal xCoordinate = decimal.Parse(numbers[1]);
                decimal yCoordinate = decimal.Parse(numbers[2]);
                decimal zCoordinate = decimal.Parse(numbers[3]);
                UInt64 code = UInt64.Parse(numbers[4]);

                lines.Add(new Line(lineNumber, code, new Point(xCoordinate, yCoordinate)));
            }
            return lines;
        }
    }
}
