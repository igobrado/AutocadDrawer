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
using System.IO;
using MgdAcApplication = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Drawer
{
    public class Drawer
    {
        protected void drawPolyline(List<Point> drawPoints)
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                   OpenMode.ForRead) as BlockTable;
                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                      OpenMode.ForWrite) as BlockTableRecord;

                Polyline acPoly = new Polyline();
                acPoly.SetDatabaseDefaults();

                int i = 0;
                foreach (var pt in drawPoints)
                {
                    acPoly.AddVertexAt(i++, new Point2d(pt.CoordinateX, pt.CoordinateY), 0, 0, 0);
                    AddText(pt.CoordinateX, pt.CoordinateY, pt.LineNumber);
                }

                // Add the new object to the block table record and the transaction
                acBlkTblRec.AppendEntity(acPoly);
                acTrans.AddNewlyCreatedDBObject(acPoly, true);
                // Save the new object to the database
                acTrans.Commit();
            }
        }
        protected void AddText(double xCoordinate, double yCoordinate, string text)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a single-line text object
                DBText acText = new DBText();
                acText.SetDatabaseDefaults();
                acText.Position = new Point3d(xCoordinate, yCoordinate, 0);
                acText.Height = 1;
                acText.TextString = text;

                acBlkTblRec.AppendEntity(acText);
                acTrans.AddNewlyCreatedDBObject(acText, true);

                // Save the changes and dispose of the transaction
                acTrans.Commit();
            }
        }
        public void AddDwg(Database db, string dwgPathname)
        {
            ObjectId dwgBlkId = ObjectId.Null;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                string blockName = Path.GetFileNameWithoutExtension(dwgPathname);
                if (bt.Has(blockName))
                    dwgBlkId = bt[blockName];
                else
                {
                    if (!File.Exists(dwgPathname))
                        return;
                    using (Database dwgDb = new Database(false, true))
                    {
                        dwgDb.ReadDwgFile(dwgPathname, FileShare.Read, true, null);
                        dwgDb.CloseInput(true);
                        dwgBlkId = db.Insert(blockName, dwgDb, true);
                        if (dwgBlkId == ObjectId.Null)
                            return;
                    }
                }
                tr.Commit();
            }
        }

        public void InsertBlk(ObjectId blockTableRecordId, string blockPathName, Point3d insertionPoint, double insScale, double insRotation)
        {
            Database db = blockTableRecordId.Database;

            AddDwg(db, blockPathName);
            drawBlock(db, insertionPoint, Path.GetFileNameWithoutExtension(blockPathName));
        }
        
        void drawBlock(Database db, Point3d insertionPoint, string blockName)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // check if the block table already has the 'blockName'" block
                var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                // create a new block reference
                using (var br = new BlockReference(insertionPoint, bt[blockName]))
                {
                    var space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    space.AppendEntity(br);
                    tr.AddNewlyCreatedDBObject(br, true);
                }
                tr.Commit();
            }
        }

        [CommandMethod("DrawBlocks")]
        public void DrawBlocks()
        {
            var codeAndSign = Parser.Instance.parseBlocks("C:\\Users\\Igor Obradovic\\Desktop\\kiz.txt");
            var lines = Parser.Instance.parse("D:\\dev\\private\\marko\\Drawer\\Drawer\\SvetaNedjelja.txt");

            Database database = HostApplicationServices.WorkingDatabase;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blkTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                foreach (var line in lines)
                {
                    UInt64 fullSign = line.FullCode;
                    if (codeAndSign.ContainsKey(fullSign) == true)
                    {
                        string path = codeAndSign[fullSign];
                        var point = line.Coordinate;
                        InsertBlk(database.BlockTableId, path, new Point3d(point.CoordinateX, point.CoordinateY, 0), 100, 0);
                    }
                }
                transaction.Commit();

            }
        }
        [CommandMethod("DrawPredefined")]
        public void Test()
        {
            // TODO: send parameters via cmd arguments
            var lines = Parser.Instance.parse("D:\\dev\\private\\marko\\Drawer\\Drawer\\SvetaNedjelja.txt");
            if (lines.Count == 0)
            {
                return;
            }

            for ( int i = 0; i < lines.Count; ++i)
            { 
                List<Point> linesToDraw = new List<Point>();

                var firstID = lines[i].getFirstCombination();
                
                if (firstID == 11 || firstID == 14 || firstID == 21)
                {
                    for (int j = i; j < lines.Count; ++j)
                    {
                        if (j == lines.Count)
                        {
                            break;
                        }

                        if ((lines[j].getFirstCombination() == firstID) && (lines[j].IsDrawn == false))
                        {
                            linesToDraw.Add(new Point(lines[j].Coordinate));
                            if (lines[j].hasCombination(39))
                            {
                                break;
                            }
                        }
                    }
                }
                drawPolyline(linesToDraw);
            }
        }
    }
}
