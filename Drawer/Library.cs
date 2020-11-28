using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Drawer
{
    public class Drawer
    {

        [CommandMethod("TEST")]
        public void Test()
        {
            var lines = Parser.Instance.parse("D:\\dev\\private\\marko\\Drawer\\Drawer\\SvetaNedjelja.txt");

            foreach (var l in lines)
            {

            }

            var doc = AcAp.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                tr.Commit();
            }

        }
    }
}
