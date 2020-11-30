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

        public void erase(UInt64 combination)
        {
            for (int i = 2; i < m_Combinations.Count; ++i)
            {
                if (m_Combinations[i] == 39 && m_Combinations[i-1] == combination)
                {
                    m_Combinations.RemoveAt(i);
                    m_Combinations.RemoveAt(i-1);
                }
            }
        }

        public bool shouldCloseFirstCombination()
        {
            if (m_Combinations.Count >= 2)
            {
                if (m_Combinations[1] == 39)
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool hasCombination(UInt64 combination)
        {
            for (int i = 1; i < m_Combinations.Count; ++i)
            {
                if (m_Combinations[i] == combination)
                {
                    return true;
                }
            }
            return false;
        }
        public bool shouldClose()
        {
            if (m_Combinations.Count == 2 && m_Combinations[1] == 39)
            {
                m_IsDrawn = true;
                return true;
            }
            else if (m_Combinations.Count == 1)
            {
                m_IsDrawn = true;
            }
            return false;
        }
        public UInt64 getFirstCombination()
        {
            return m_Combinations.First();
        }

        public void popCombination()
        {
            if (m_Combinations.Count > 0)
            {
                m_Combinations.RemoveAt(0);
                if (m_Combinations.Count == 0)
                {
                    m_IsDrawn = true;
                }
            }
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

        public string LineNumber
        {
            get { return m_Coordinate.LineNumber; }
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
