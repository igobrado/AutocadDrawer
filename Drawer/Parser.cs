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
                string lineNumber = numbers[0];
                double xCoordinate = double.Parse(numbers[1]);
                double yCoordinate = double.Parse(numbers[2]);
                double zCoordinate = double.Parse(numbers[3]);
                UInt64 code = UInt64.Parse(numbers[4]);

                lines.Add(new Line(code, new Point(lineNumber, xCoordinate, yCoordinate)));

            }
            return lines;
        }

        public Dictionary<UInt64, string> parseBlocks(string blockPath)
        {
            Dictionary<UInt64, string> blockMap = new Dictionary<UInt64, string>();


            string[] File = System.IO.File.ReadAllLines(blockPath);
            var lines = new List<Line>();
            foreach (string line in File)
            {
                string[] nums = line.Split('$');
                UInt64 code = UInt64.Parse(nums[0]);
                string fileName = nums[1];
                blockMap.Add(code, fileName);
            }

            return blockMap;
        }
    }
}
