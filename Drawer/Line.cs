using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer
{
    public class Point
    {
        public Point(string lineNumber, double xCoord, double yCoord)
        {
            m_xCoordinate = xCoord;
            m_yCoordinate = yCoord;
            m_LineNumber = lineNumber;
        }
        public Point(Point rhs)
        {
            m_xCoordinate = rhs.CoordinateX;
            m_yCoordinate = rhs.CoordinateY;
            m_LineNumber = rhs.LineNumber;
        }

        public double CoordinateX
        {
            get { return m_xCoordinate; }
        }
        public double CoordinateY
        {
            get { return m_yCoordinate; }
        }

        public string LineNumber
        {
            get { return m_LineNumber; }
        }
        private readonly double m_xCoordinate;
        private readonly double m_yCoordinate;
        private string m_LineNumber { get; set; }
    }

    class Line
    {
        public Line(UInt64 code, Point p)
        {
            m_Coordinate = new Point(p);
            m_Combinations = new List<UInt64>();
            toCombinations(code);
            m_IsDrawn = false;
            m_FullCode = code;
        }


        public UInt64 getFirstCombination()
        {
            return m_Combinations.First();
        }

        public void popCombination()
        {
            if (m_Combinations.Count > 0)
            {
                m_Combinations.Remove(m_Combinations.First());
            }
        }
        public bool hasCombination(UInt64 number)
        {
            return m_Combinations.IndexOf(number) != -1;
        }

        public bool shouldClose()
        {
            return m_Combinations.IndexOf(32) != -1;
        }

        private void toCombinations(UInt64 number)
        {
            while (number != 0)
            {
                m_Combinations.Add(number % 100);
                number /= 100;
            }
            m_Combinations.Reverse();
        }
        public List<UInt64> Combinations
        {
            get { return m_Combinations; }
        }
        public Point Coordinate
        {
            get { return m_Coordinate; }
        }

        public UInt64 FullCode
        {
            get { return m_FullCode; }
        }
        public bool IsDrawn
        {
            get { return m_IsDrawn; }
            set { m_IsDrawn = value; }
        }

        private Point m_Coordinate { get; set; }
        private List<UInt64> m_Combinations { get; set; }
        private bool m_IsDrawn {get; set; }
        private UInt64 m_FullCode { get; set; }
    }
}
