using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer
{
    public class Point
    {
        public Point(decimal xCoord, decimal yCoord)
        {
            m_xCoordinate = xCoord;
            m_yCoordinate = yCoord;
        }
        public decimal CoordinateX
        {
            get { return m_xCoordinate; }
        }
        public decimal CoordinateY
        {
            get { return m_yCoordinate; }
        }

        private readonly decimal m_xCoordinate;
        private readonly decimal m_yCoordinate;
    }
    
    class Line
    {
        public Line(UInt64 lineNumber, UInt64 code, Point p)
        {
            m_LineNumber = lineNumber;
            m_Coordinate = new Point(p.CoordinateX, p.CoordinateY);
            m_Combinations = new List<UInt64>();
            toCombinations(code);
        }
 
        private void toCombinations(UInt64 number)
        {
            while(number != 0)
            {
                m_Combinations.Add(number % 100);
                number /= 100;
            }
        }
        public List<UInt64> Combinations
        {
            get { return m_Combinations; }
        }
        public Point Coordinate
        {
            get { return m_Coordinate; }
        }
        private UInt64 m_LineNumber;
        private Point m_Coordinate { get; set; }
        private List<UInt64> m_Combinations { get; set; }
    }
}
