// (C) Copyright 2016 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Autodesk.AutoCAD.Windows;
using GV = C3D_2016_Anno.Global.variables;
using GH = C3D_2016_Anno.Helper.GenHelper;
using LCH = C3D_2016_Anno.Helper.LocalCADHelper;
using Autodesk.Civil.DatabaseServices.Styles;
using cad_ent = Autodesk.AutoCAD.DatabaseServices.Entity;
using Open_mode = Autodesk.AutoCAD.DatabaseServices.OpenMode;
using System.Drawing;
using System.Runtime.InteropServices;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.ComponentModel;
using Notifications.Wpf;
using System.Diagnostics;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(C3D_2016_Anno.MyCommands))]

namespace C3D_2016_Anno
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {

        static PaletteSet palSet = null;
        static string templateWarning;
        public static Apps.MainControl mc;
        //public static Apps.MainControl mw = new Apps.MainControl();

        // Modal Command with localized name
        [CommandMethod("AAnno")]
        [CommandMethod("AutoAnno")]
        public void AutoAnno() // This method can have any name
        {
            try
            {

                LCH.getCurrentDwgVars();

                GH.writeLog("\n Running command : AAnno");
                if (GV.Doc != null)
                {
                    //check and load dll
                    
                    Helper.GenHelper.initializeSettings();
                    if (GV.loaddll)
                    {
                        if (GV.dllLoadStatus == false)
                        {
                            GV.Doc.SendStringToExecute("_loadLib ", true, false, false);
                        }
                    }

                    GH.writeLog("\n Starting Tool Pallette");
                    #region Create Pallette
                    if (palSet == null)
                    {

                        // Create the palette set
                        palSet = new PaletteSet("Note Creator");

                        palSet.Style = PaletteSetStyles.ShowPropertiesMenu |
                                        PaletteSetStyles.ShowAutoHideButton |
                                        PaletteSetStyles.ShowCloseButton;
                        palSet.MinimumSize = new System.Drawing.Size(500, 800);
                        palSet.Size = new System.Drawing.Size(500, 800);

                        palSet.DockEnabled =
                          (DockSides)((int)DockSides.Left + (int)DockSides.Right);

                        Apps.MainControl mc = new Apps.MainControl();
                        ElementHost mcHost = new ElementHost();
                        mcHost.AutoSize = true;
                        mcHost.Dock = DockStyle.Fill;
                        //mcHost.Width = 500;
                        //mcHost.Height = 800;
                        //mcHost.Size = new System.Drawing.Size(500, 800);
                        //mcHost.MinimumSize = new System.Drawing.Size(500, 800);
                        mcHost.Child = mc;
                        //mcHost.Child.RenderSize = new System.Windows.Size(500, 800);
                        palSet.Add("Note Creator", mcHost);
                        // Display our palette set
                        palSet.KeepFocus = true;
                        palSet.Visible = true;
                        //palSet.Dock = DockSides.;

                        palSet.SetSize(new System.Drawing.Size(500, 800));
                        palSet.Dock = DockSides.Left;
                    }
                    else
                    {
                        palSet.KeepFocus = true;
                        palSet.Visible = true;
                    }
                    #endregion

                }
                else
                {
                    GH.writeLog("\n Tool P not started");
                }
            }
            catch (System.Exception ex)
            {
                GH.writeLog("\n ERROR: " + ex.ToString());
            }
        }

        [CommandMethod("xmlMan")]
        public void xmlMan() // This method can have any name
        {
            try
            {
                //open xml manager
                Process.Start(GV.xmlManPath);
            }
            catch (System.Exception ex)
            {
                GH.writeLog("\n ERROR: " + ex.ToString());
            }
        }

        [CommandMethod("_loadLib")]
        public void loadLib() // This method can have any name
        {
            try
            {
                string metrodll = GV.appPath + @"\MahApps.Metro.dll";
                GH.writeLog(">> dll loading (" + metrodll + ")");
                GH.loaddlls(metrodll);
                GV.dllLoadStatus = true;
            }
            catch (System.Exception ex)
            {
                GH.writeLog("\n ERROR: " + ex.ToString());
            }
        }

        [CommandMethod("__eman")]
        public void eman() // This method can have any name
        {
            try
            {
                Apps.Editor eman = new Apps.Editor();
                eman.Show();
            }
            catch (System.Exception ex)
            {
                GH.writeLog("\n ERROR: " + ex.ToString());
            }
        }

        [CommandMethod("annopick")]
        public void annopick() // This method can have any name
        {
            try
            {
                // Put your command code here
                LCH.getCurrentDwgVars(); if (GV.Doc != null)
                {
                    Helper.GenHelper.initializeSettings();

                    #region Filter
                    //select only labels - filter using the details available in the mapper.
                    // Build a conditional filter list so that only
                    // entities with the specified properties are
                    // selected
                    TypedValue[] tvArr = new TypedValue[]
                    {
                    new TypedValue((int)DxfCode.Operator, "<or"),
                    new TypedValue((int)DxfCode.Start, "AECC_PIPE_LABEL"),
                    new TypedValue((int)DxfCode.Start, "AECC_STRUCTURE_LABEL"),
                    new TypedValue((int)DxfCode.Start, "AECC_GENERAL_NOTE_LABEL"),
                    new TypedValue((int)DxfCode.Start, "AECC_GENERAL_SEGMENT_LABEL"),
                    new TypedValue((int)DxfCode.Start, "AECC_STATION_OFFSET_LABEL"),
                    new TypedValue((int)DxfCode.Operator, "or>"),
                        };

                    #endregion

                    PromptSelectionResult psRes = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.GetSelection(new SelectionFilter(tvArr));

                    if (psRes.Status == PromptStatus.OK)
                    {
                        SelectionSet acSSet = psRes.Value;
                        GV.selObjIds = Helper.LocalCADHelper.convertObjArraytoCollection(acSSet.GetObjectIds());

                        //update ui
                        //mw.fetchDATA();
                    }
                }
            }
            catch (System.Exception ex)
            {
                GV.ed.WriteMessage("\n ERROR: " + ex.ToString());
            }
        }

        [DllImport("accore.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "acedTrans")]
        static extern int acedTrans(double[] point, IntPtr fromRb, IntPtr toRb, int disp, double[] result);
        [CommandMethod("sPs", CommandFlags.NoTileMode)]
        static public void selectMsFromPs()
        {
            LCH.getCurrentDwgVars();
            // pick a PS Viewport
            PromptEntityOptions opts = new PromptEntityOptions("Pick PS Viewport");
            opts.SetRejectMessage("Must select PS Viewport objects only");
            opts.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Viewport), false);
            PromptEntityResult res = GV.ed.GetEntity(opts);
            if (res.Status == PromptStatus.OK)
            {
                int vpNumber = 0;
                // extract the viewport points
                Point3dCollection psVpPnts = new Point3dCollection();
                using (Transaction trans = GV.Db.TransactionManager.StartTransaction())
                {
                    using (Autodesk.AutoCAD.DatabaseServices.Viewport psVp = (Autodesk.AutoCAD.DatabaseServices.Viewport)trans.GetObject(res.ObjectId, OpenMode.ForWrite))// Autodesk.AutoCAD.DatabaseServices.Viewport psVp = viewportobjID.GetObject(OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Viewport)
                    {
                        // get the vp number
                        vpNumber = psVp.Number;
                        // now extract the viewport geometry
                        psVp.GetGripPoints(psVpPnts, new IntegerCollection(), new IntegerCollection());


                        // let's assume a rectangular vport for now, make the cross-direction grips square
                        Point3d tmp = psVpPnts[2];
                        psVpPnts[2] = psVpPnts[1];
                        psVpPnts[1] = tmp;

                        // Transform the PS points to MS points
                        ResultBuffer rbFrom = new ResultBuffer(new TypedValue(5003, 3));
                        ResultBuffer rbTo = new ResultBuffer(new TypedValue(5003, 2));
                        double[] retPoint = new double[] { 0, 0, 0 };
                        // loop the ps points 
                        Point3dCollection msVpPnts = new Point3dCollection();
                        foreach (Point3d pnt in psVpPnts)
                        {
                            // translate from from the DCS of Paper Space (PSDCS) RTSHORT=3 and 
                            // the DCS of the current model space viewport RTSHORT=2
                            acedTrans(pnt.ToArray(), rbFrom.UnmanagedObject, rbTo.UnmanagedObject, 3, retPoint);
                            // add the resulting point to the ms pnt array
                            msVpPnts.Add(new Point3d(retPoint));
                            GV.ed.WriteMessage("\n" + new Point3d(retPoint).ToString());
                        }

                        // now switch to MS
                        GV.ed.SwitchToModelSpace();
                        BlockTable acBlkTbl;
                        acBlkTbl = trans.GetObject(GV.Db.BlockTableId,
                                                        OpenMode.ForRead) as BlockTable;

                        // Open the Block table record Model space for write
                        BlockTableRecord acBlkTblRec;
                        acBlkTblRec = trans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                        OpenMode.ForWrite) as BlockTableRecord;

                        // set the CVPort
                        Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("CVPORT", vpNumber);

                        using (Polyline acPoly = new Polyline())
                        {
                            int i = 0;
                            foreach (Point3d pnt in msVpPnts)
                            {
                                acPoly.AddVertexAt(i, new Point2d(pnt.X, pnt.Y), 0, 0, 0);
                                i++;
                            }

                            // Add the new object to the block table record and the transaction
                            acBlkTblRec.AppendEntity(acPoly);
                            trans.AddNewlyCreatedDBObject(acPoly, true);
                        }

                        trans.Commit();
                        // once switched, we can use the normal selection mode to select
                        PromptSelectionResult selectionresult = GV.ed.SelectCrossingPolygon(msVpPnts);
                        // now switch back to PS
                        GV.ed.SwitchToPaperSpace();
                    }
                }
            }
        }

        [CommandMethod("bbb")]
        public void MyTEST2() // This method can have any name
        {
            try
            {
                // Put your command code here
                LCH.getCurrentDwgVars();
                Helper.GenHelper.initializeSettings();
                TypedValue[] tvArr = new TypedValue[]
                {
                    new TypedValue((int)DxfCode.Operator, "<AND"),
                    new TypedValue((int)DxfCode.Start, "AeccDbPipeLabel"),
                    new TypedValue((int)DxfCode.Start, "AeccDbStructureLabel"),
                    new TypedValue((int)DxfCode.Start, "AeccDbNoteLabel"),
                    new TypedValue((int)DxfCode.Start, "AeccDbGeneralLabel"),
                    new TypedValue((int)DxfCode.Start, "AeccDbStaOffsetLabel")
                    };

                PromptSelectionResult psRes = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.GetSelection(new SelectionFilter(tvArr));

                if (psRes.Status == PromptStatus.OK)
                {
                    GV.ed.WriteMessage("Number of objects selected: {0}", psRes.Value.Count);
                    GH.writeLog("\nNumber of objects selected: " + psRes.Value.Count);
                    psRes.Value.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                GV.ed.WriteMessage("\n ERROR: " + ex.ToString());
            }
        }

        [CommandMethod("mll")]
        public static void FilterSelectionSet()
        {
            LCH.getCurrentDwgVars();

            // Create a TypedValue array to define the filter criteria
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "MULTILEADER"), 0);

            // Assign the filter criteria to a SelectionFilter object
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            // Request for objects to be selected in the drawing area
            PromptSelectionResult acSSPrompt;

            acSSPrompt = GV.ed.GetSelection(acSelFtr);

            // If the prompt status is OK, objects were selected
            if (acSSPrompt.Status == PromptStatus.OK)
            {
                SelectionSet acSSet = acSSPrompt.Value;

                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Number of objects selected: " +
                                            acSSet.Count.ToString());
                using (Transaction tr = GV.Db.TransactionManager.StartTransaction())
                {
                    ObjectId id = acSSet.GetObjectIds()[0];
                    Entity ent = (Entity)tr.GetObject(id, OpenMode.ForRead);
                    MLeader pl = (MLeader)ent;

                    ObjectId blkid = pl.BlockContentId;
                    BlockTableRecord mbtr = (BlockTableRecord)tr.GetObject(blkid, OpenMode.ForRead);
                    if (mbtr.HasAttributeDefinitions)
                    {
                        foreach (ObjectId atid in mbtr)
                        {
                            DBObject obj = tr.GetObject(atid, OpenMode.ForRead, false) as DBObject;

                            if (atid.ObjectClass.IsDerivedFrom(RXClass.GetClass(typeof(AttributeDefinition))))
                            {
                                AttributeDefinition attdef = (AttributeDefinition)obj as AttributeDefinition;

                                if (attdef.Tag == "TAGNUMBER") // check your existing tag
                                {
                                    AttributeReference attref = pl.GetBlockAttribute(attdef.ObjectId);
                                    MLeaderStyle currentStyle = tr.GetObject(pl.MLeaderStyle, OpenMode.ForWrite) as MLeaderStyle;
                                    GV.ed.WriteMessage("\n ATT 2 = " + attref.Tag + "\n    Attribute String: " + attref.TextString + "|" + currentStyle.Name);
                                }

                            }
                        }
                    }
                    //BlockReference blkRef = (BlockReference)tr.GetObject(pl.bloc, OpenMode.ForRead);
                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(pl.BlockId, OpenMode.ForRead);

                    //AttributeCollection attCol = blkRef.AttributeCollection;

                    //foreach (ObjectId attId in attCol)
                    //{
                    //    AttributeReference attRef =
                    //      (AttributeReference)tr.GetObject(attId,
                    //        OpenMode.ForRead);

                    //    string str =
                    //      ("\n  Attribute Tag: "
                    //        + attRef.Tag
                    //        + "\n    Attribute String: "
                    //        + attRef.TextString
                    //      );
                    //    GV.ed.WriteMessage(str);

                    //    AttributeReference a1 = pl.GetBlockAttribute(attRef.Id);

                    //    GV.ed.WriteMessage("\n ATT 2 = " + a1.Tag + "\n    Attribute String: " + a1.TextString);
                    //}




                    //GV.ed.WriteMessage("\n= = " + pl.)
                }
            }

            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Number of objects selected: 0");
            }
        }

        [CommandMethod("cmk", CommandFlags.NoTileMode)]
        public void CenterMark()
        {
            try
            {
                LCH.getCurrentDwgVars();

                // switch to paper space if a viewport is activated
                if ((short)AcAp.GetSystemVariable("CVPORT") != 1)
                    GV.ed.SwitchToPaperSpace();

                // select a viewport
                var options = new PromptSelectionOptions();
                options.MessageForAdding = "\nSelect a rectangular viewport: ";
                options.SelectEverythingInAperture = true;
                options.SingleOnly = true;
                var filter = new SelectionFilter(new[] { new TypedValue(0, "VIEWPORT") });
                var selection = GV.ed.GetSelection(options, filter);
                if (selection.Status != PromptStatus.OK)
                    return;

                using (var tr = GV.Db.TransactionManager.StartTransaction())
                {
                    Entity newEnt = null;
                    // get the viewport extents (paper space coordinates)
                    var vp = (Viewport)tr.GetObject(selection.Value[0].ObjectId, OpenMode.ForRead);
                    var extents = vp.GeometricExtents;
                    double x1 = extents.MinPoint.X, y1 = extents.MinPoint.Y;
                    double x2 = extents.MaxPoint.X, y2 = extents.MaxPoint.Y;

                    // activate the selected viewport
                    GV.ed.SwitchToModelSpace();
                    AcAp.SetSystemVariable("CVPORT", vp.Number);

                    // compute the transformation matrix from paper space coordinates to model space current UCS
                    var wcs2ucs = GV.ed.CurrentUserCoordinateSystem.Inverse();
                    var psdcs2ucs = wcs2ucs * vp.DCS2WCS() * vp.PSDCS2DCS();

                    // compute the model space UCS polygon vertices corresponding to the viewport extents
                    var polygon = new Point3dCollection();
                    polygon.Add(new Point3d(x1, y1, 0.0).TransformBy(psdcs2ucs));
                    polygon.Add(new Point3d(x2, y1, 0.0).TransformBy(psdcs2ucs));
                    polygon.Add(new Point3d(x2, y2, 0.0).TransformBy(psdcs2ucs));
                    polygon.Add(new Point3d(x1, y2, 0.0).TransformBy(psdcs2ucs));

                    Polyline poly = new Polyline(4);
                    Point3d pttemp = new Point3d(x1, y1, 0.0).TransformBy(psdcs2ucs);
                    //poly.AddVertexAt(0, new Point3d(x1, y1, 0.0).TransformBy(psdcs2ucs).Convert2d, 0, -1, -1);
                    poly.AddVertexAt(0, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    pttemp = new Point3d(x2, y1, 0.0).TransformBy(psdcs2ucs);
                    poly.AddVertexAt(1, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    pttemp = new Point3d(x2, y2, 0.0).TransformBy(psdcs2ucs);
                    poly.AddVertexAt(2, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    pttemp = new Point3d(x1, y2, 0.0).TransformBy(psdcs2ucs);
                    poly.AddVertexAt(3, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;

                    // select circles within the polygon
                    filter = new SelectionFilter(new[] { new TypedValue(0, "CIRCLE") });
                    selection = GV.ed.SelectWindowPolygon(polygon, filter);
                    if (selection.Status != PromptStatus.OK)
                        return;

                    // collect the selected circles centers transformed into paper space coordinates
                    var centers = new Point3dCollection();
                    var wcs2psdcs = vp.DCS2PSDCS() * vp.WCS2DCS();
                    foreach (SelectedObject so in selection.Value)
                    {
                        var circle = (Circle)tr.GetObject(so.ObjectId, OpenMode.ForRead);
                        centers.Add(circle.Center.TransformBy(wcs2psdcs));
                    }

                    // switch back to paper space
                    GV.ed.SwitchToPaperSpace();

                    // add a point (nodal) in paper space on the center of the selected circles
                    var curSpace = (BlockTableRecord)tr.GetObject(GV.Db.CurrentSpaceId, OpenMode.ForWrite);
                    foreach (Point3d pt in centers)
                    {
                        DBPoint point = new DBPoint(pt);
                        curSpace.AppendEntity(point);
                        tr.AddNewlyCreatedDBObject(point, true);
                    }

                    BlockTable bt = (BlockTable)GV.Db.BlockTableId.GetObject(OpenMode.ForRead);
                    BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                    ms.AppendEntity(newEnt);
                    tr.AddNewlyCreatedDBObject(newEnt, true);
                    tr.Commit();
                }
            }
            catch (Autodesk.Civil.CivilException ex)
            {
                GH.errorBox(ex.ToString());
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                GH.errorBox(ex.ToString());
            }
            catch (System.Exception ee)
            {
                GH.errorBox(ee.ToString());
            }
        }

        [CommandMethod("vpp", CommandFlags.NoTileMode)]
        public void vpp()
        {
            try
            {
                LCH.getCurrentDwgVars();

                // switch to paper space if a viewport is activated
                if ((short)AcAp.GetSystemVariable("CVPORT") != 1)
                    GV.ed.SwitchToPaperSpace();

                // select a viewport
                var options = new PromptSelectionOptions();
                options.MessageForAdding = "\nSelect a rectangular viewport: ";
                options.SelectEverythingInAperture = true;
                options.SingleOnly = true;
                var filter = new SelectionFilter(new[] { new TypedValue(0, "VIEWPORT") });
                var selection = GV.ed.GetSelection(options, filter);
                if (selection.Status != PromptStatus.OK)
                    return;

                using (var tr = GV.Db.TransactionManager.StartTransaction())
                {
                    Entity newEnt = null;
                    // get the viewport extents (paper space coordinates)
                    var vp = (Viewport)tr.GetObject(selection.Value[0].ObjectId, OpenMode.ForRead);
                    var extents = vp.GeometricExtents;
                    double x1 = extents.MinPoint.X, y1 = extents.MinPoint.Y;
                    double x2 = extents.MaxPoint.X, y2 = extents.MaxPoint.Y;

                    // activate the selected viewport
                    GV.ed.SwitchToModelSpace();
                    AcAp.SetSystemVariable("CVPORT", vp.Number);

                    // compute the transformation matrix from paper space coordinates to model space current UCS
                    var wcs2ucs = GV.ed.CurrentUserCoordinateSystem.Inverse();
                    var psdcs2ucs = wcs2ucs * vp.DCS2WCS() * vp.PSDCS2DCS();

                    // compute the model space UCS polygon vertices corresponding to the viewport extents
                    var polygon = new Point3dCollection();
                    polygon.Add(new Point3d(x1, y1, 0.0).TransformBy(psdcs2ucs));
                    polygon.Add(new Point3d(x2, y1, 0.0).TransformBy(psdcs2ucs));
                    polygon.Add(new Point3d(x2, y2, 0.0).TransformBy(psdcs2ucs));
                    polygon.Add(new Point3d(x1, y2, 0.0).TransformBy(psdcs2ucs));

                    Polyline poly = new Polyline(4);
                    Point3d pttemp = new Point3d(x1, y1, 0.0).TransformBy(psdcs2ucs);
                    //poly.AddVertexAt(0, new Point3d(x1, y1, 0.0).TransformBy(psdcs2ucs).Convert2d, 0, -1, -1);
                    poly.AddVertexAt(0, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    pttemp = new Point3d(x2, y1, 0.0).TransformBy(psdcs2ucs);
                    poly.AddVertexAt(1, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    pttemp = new Point3d(x2, y2, 0.0).TransformBy(psdcs2ucs);
                    poly.AddVertexAt(2, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    pttemp = new Point3d(x1, y2, 0.0).TransformBy(psdcs2ucs);
                    poly.AddVertexAt(3, new Point2d(pttemp.X, pttemp.Y), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;




                    // switch back to paper space
                    GV.ed.SwitchToPaperSpace();

                    BlockTable bt = (BlockTable)GV.Db.BlockTableId.GetObject(OpenMode.ForRead);
                    BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                    ms.AppendEntity(newEnt);
                    tr.AddNewlyCreatedDBObject(newEnt, true);
                    tr.Commit();
                }
            }
            catch (Autodesk.Civil.CivilException ex)
            {
                GH.errorBox(ex.ToString());
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                GH.errorBox(ex.ToString());
            }
            catch (System.Exception ee)
            {
                GH.errorBox(ee.ToString());
            }
        }

        [CommandMethod("cmk2", CommandFlags.NoTileMode)]
        public void CenterMark2()
        {
            LCH.getCurrentDwgVars();

            // switch to paper space if a viewport is activated
            if ((short)AcAp.GetSystemVariable("CVPORT") != 1)
                GV.ed.SwitchToPaperSpace();

            // select a viewport
            PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");

            eo.AllowNone = false;
            eo.SetRejectMessage("\nNeed to select a viewport!");
            eo.AddAllowedClass(typeof(Viewport), true);
            //next lines are to allow for non-rectangular viewport selection
            eo.AddAllowedClass(typeof(Circle), true);
            eo.AddAllowedClass(typeof(Polyline), true);
            eo.AddAllowedClass(typeof(Polyline2d), true);
            eo.AddAllowedClass(typeof(Polyline3d), true);
            eo.AddAllowedClass(typeof(Ellipse), true);
            eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
            eo.AddAllowedClass(typeof(Spline), true);
            eo.AddAllowedClass(typeof(Face), true);

            PromptEntityResult er = GV.ed.GetEntity(eo);
            if (er.Status != PromptStatus.OK)
                return;

            using (Transaction tr = GV.Db.TransactionManager.StartTransaction())
            {
                Entity newEnt = null;
                Viewport vpEnt = null;

                Entity selEnt = (Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                if (selEnt.GetType() == typeof(Viewport))
                {
                    //Viewport is rectangular
                    vpEnt = (Viewport)selEnt;
                    Extents3d vpExt = vpEnt.GeometricExtents;
                    double w = vpExt.MaxPoint.X - vpExt.MinPoint.X;
                    double h = vpExt.MaxPoint.Y - vpExt.MinPoint.Y;
                    Polyline poly = new Polyline(4);
                    poly.AddVertexAt(0, new Point2d(0, 0), 0, -1, -1);
                    poly.AddVertexAt(1, new Point2d(w, 0), 0, -1, -1);
                    poly.AddVertexAt(2, new Point2d(w, h), 0, -1, -1);
                    poly.AddVertexAt(3, new Point2d(0, h), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;
                }
                else
                {
                    //Viewport is non-rectangular, attempt to get it from the selected clip entity
                    ObjectId vpId = LayoutManager.Current.GetNonRectangularViewportIdFromClipId(selEnt.Id);
                    if (vpId == ObjectId.Null)
                        vpEnt = null;
                    else
                    {
                        vpEnt = (Viewport)vpId.GetObject(OpenMode.ForRead);
                        newEnt = (Entity)selEnt.Clone();
                    }
                }

                if (vpEnt == null)
                {
                    GV.ed.WriteMessage("\nSelected object is not a viewport!");
                    tr.Commit();
                    return;
                }

                // Turn viewport on if needed
                if (!vpEnt.On)
                {
                    vpEnt.UpgradeOpen();
                    vpEnt.On = true;
                }

                //Activate a vport, set CVPORT to the selected vport, and get VIEWCTR of vport
                GV.ed.SwitchToModelSpace();
                AcAp.SetSystemVariable("CVPORT", vpEnt.Number);
                Point3d cenPnt = (Point3d)AcAp.GetSystemVariable("VIEWCTR");

                Extents3d entExt = newEnt.GeometricExtents;
                Point3d p1 = entExt.MinPoint;
                Point3d p2 = entExt.MaxPoint;
                Point3d extMid = new Point3d((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                double entHeight = p2.Y - p1.Y;

                Vector3d zAxis = vpEnt.ViewDirection;
                Vector3d xAxis = zAxis.GetPerpendicularVector().GetNormal();
                Vector3d yAxis = zAxis.CrossProduct(xAxis).GetNormal();
                zAxis = zAxis.GetNormal();

                Matrix3d transMat;
                transMat = Matrix3d.AlignCoordinateSystem(extMid, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis,
                    cenPnt, xAxis, yAxis, zAxis);
                newEnt.TransformBy(transMat);

                transMat = Matrix3d.Scaling(vpEnt.ViewHeight / entHeight, cenPnt);
                newEnt.TransformBy(transMat);

                transMat = Matrix3d.Rotation(-vpEnt.TwistAngle, zAxis, cenPnt);
                newEnt.TransformBy(transMat);

                GV.ed.WriteMessage("\n Entity: " + newEnt.ToString());
                GV.ed.WriteMessage("\n ent1.Handle: " + newEnt.Handle.ToString());
                GV.ed.WriteMessage("\n ent1.Handle.Value: " + newEnt.Handle.Value.ToString());
                GV.ed.WriteMessage("\n acSSObj.ObjectId.Handle.Value: " + newEnt.ObjectId.Handle.Value.ToString());
                GV.ed.WriteMessage("\n acSSObj.ObjectId: " + newEnt.ObjectId.ToString());
                GV.ed.WriteMessage("\n acSSObj.ObjectId.ObjectClass.Name: " + newEnt.ObjectId.ObjectClass.Name.ToString());
                GV.ed.WriteMessage("\n acSSObj.ObjectId.ObjectClass.DxfName: " + newEnt.ObjectId.ObjectClass.DxfName.ToString());
                //// get the viewport extents (paper space coordinates)
                //var vp = (Viewport)tr.GetObject(selection.Value[0].ObjectId, OpenMode.ForRead);
                //var extents = vp.GeometricExtents;
                //double x1 = extents.MinPoint.X, y1 = extents.MinPoint.Y;
                //double x2 = extents.MaxPoint.X, y2 = extents.MaxPoint.Y;

                //// activate the selected viewport
                //GV.ed.SwitchToModelSpace();
                //AcAp.SetSystemVariable("CVPORT", vp.Number);

                // compute the transformation matrix from paper space coordinates to model space current UCS
                //var wcs2ucs = GV.ed.CurrentUserCoordinateSystem.Inverse();
                //var psdcs2ucs = wcs2ucs * vp.DCS2WCS() * vp.PSDCS2DCS();

                // compute the model space UCS polygon vertices corresponding to the viewport extents
                var polygon = new Point3dCollection();
                Polyline pl = (Polyline)newEnt;
                int vn = pl.NumberOfVertices;
                for (int i = 0; i < vn; i++)
                {
                    // Could also get the 3D point here
                    Point3d pt = pl.GetPoint3dAt(i);
                    polygon.Add(pt);
                    GV.ed.WriteMessage("\n" + pt.ToString());
                }
                pl.Closed = true;

                //polygon.Add(new Point3d(x1, y1, 0.0).TransformBy(psdcs2ucs));
                //polygon.Add(new Point3d(x2, y1, 0.0).TransformBy(psdcs2ucs));
                //polygon.Add(new Point3d(x2, y2, 0.0).TransformBy(psdcs2ucs));
                //polygon.Add(new Point3d(x1, y2, 0.0).TransformBy(psdcs2ucs));

                //select circles within the polygon
                var filter = new SelectionFilter(new[] { new TypedValue(0, "CIRCLE") });
                var selection = GV.ed.SelectWindowPolygon(polygon, filter);
                if (selection.Status != PromptStatus.OK)
                    return;

                // collect the selected circles centers transformed into paper space coordinates
                var centers = new Point3dCollection();
                var wcs2psdcs = vpEnt.DCS2PSDCS() * vpEnt.WCS2DCS();
                foreach (SelectedObject so in selection.Value)
                {
                    var circle = (Circle)tr.GetObject(so.ObjectId, OpenMode.ForRead);
                    centers.Add(circle.Center.TransformBy(wcs2psdcs));
                }

                // switch back to paper space
                GV.ed.SwitchToPaperSpace();

                // add a point (nodal) in paper space on the center of the selected circles
                var curSpace = (BlockTableRecord)tr.GetObject(GV.Db.CurrentSpaceId, OpenMode.ForWrite);
                foreach (Point3d pt in centers)
                {
                    DBPoint point = new DBPoint(pt);
                    curSpace.AppendEntity(point);
                    tr.AddNewlyCreatedDBObject(point, true);
                }

                tr.Commit();
            }
        }

        [CommandMethod("VPTOMS", CommandFlags.Modal | CommandFlags.NoTileMode)]
        static public void VPTOMS()
        {
            Document curDoc = AcAp.DocumentManager.MdiActiveDocument;
            Database db = curDoc.Database;
            Editor ed = curDoc.Editor;

            ed.SwitchToPaperSpace();

            PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");
            eo.AllowNone = false;
            eo.SetRejectMessage("\nNeed to select a viewport!");
            eo.AddAllowedClass(typeof(Viewport), true);
            //next lines are to allow for non-rectangular viewport selection
            eo.AddAllowedClass(typeof(Circle), true);
            eo.AddAllowedClass(typeof(Polyline), true);
            eo.AddAllowedClass(typeof(Polyline2d), true);
            eo.AddAllowedClass(typeof(Polyline3d), true);
            eo.AddAllowedClass(typeof(Ellipse), true);
            eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
            eo.AddAllowedClass(typeof(Spline), true);
            eo.AddAllowedClass(typeof(Face), true);

            PromptEntityResult er = ed.GetEntity(eo);
            if (er.Status != PromptStatus.OK)
                return;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity newEnt = null;
                Viewport vpEnt = null;

                Entity selEnt = (Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                if (selEnt.GetType() == typeof(Viewport))
                {
                    //Viewport is rectangular
                    vpEnt = (Viewport)selEnt;
                    Extents3d vpExt = vpEnt.GeometricExtents;
                    double w = vpExt.MaxPoint.X - vpExt.MinPoint.X;
                    double h = vpExt.MaxPoint.Y - vpExt.MinPoint.Y;
                    Polyline poly = new Polyline(4);
                    poly.AddVertexAt(0, new Point2d(0, 0), 0, -1, -1);
                    poly.AddVertexAt(1, new Point2d(w, 0), 0, -1, -1);
                    poly.AddVertexAt(2, new Point2d(w, h), 0, -1, -1);
                    poly.AddVertexAt(3, new Point2d(0, h), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;
                }
                else
                {
                    //Viewport is non-rectangular, attempt to get it from the selected clip entity
                    ObjectId vpId = LayoutManager.Current.GetNonRectangularViewportIdFromClipId(selEnt.Id);
                    if (vpId == ObjectId.Null)
                        vpEnt = null;
                    else
                    {
                        vpEnt = (Viewport)vpId.GetObject(OpenMode.ForRead);
                        newEnt = (Entity)selEnt.Clone();
                    }
                }

                if (vpEnt == null)
                {
                    ed.WriteMessage("\nSelected object is not a viewport!");
                    trans.Commit();
                    return;
                }

                // Turn viewport on if needed
                if (!vpEnt.On)
                {
                    vpEnt.UpgradeOpen();
                    vpEnt.On = true;
                }

                //Activate a vport, set CVPORT to the selected vport, and get VIEWCTR of vport
                ed.SwitchToModelSpace();
                AcAp.SetSystemVariable("CVPORT", vpEnt.Number);
                Point3d cenPnt = (Point3d)AcAp.GetSystemVariable("VIEWCTR");

                Extents3d entExt = newEnt.GeometricExtents;
                Point3d p1 = entExt.MinPoint;
                Point3d p2 = entExt.MaxPoint;
                Point3d extMid = new Point3d((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                double entHeight = p2.Y - p1.Y;

                Vector3d zAxis = vpEnt.ViewDirection;
                Vector3d xAxis = zAxis.GetPerpendicularVector().GetNormal();
                Vector3d yAxis = zAxis.CrossProduct(xAxis).GetNormal();
                zAxis = zAxis.GetNormal();

                Matrix3d transMat;
                transMat = Matrix3d.AlignCoordinateSystem(extMid, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis,
                    cenPnt, xAxis, yAxis, zAxis);
                newEnt.TransformBy(transMat);

                transMat = Matrix3d.Scaling(vpEnt.ViewHeight / entHeight, cenPnt);
                newEnt.TransformBy(transMat);

                transMat = Matrix3d.Rotation(-vpEnt.TwistAngle, zAxis, cenPnt);
                newEnt.TransformBy(transMat);

                BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                GV.viewportShpObj = ms.AppendEntity(newEnt);
                trans.AddNewlyCreatedDBObject(newEnt, true);

                ed.SwitchToPaperSpace();
                trans.Commit();
            }
        }

        [CommandMethod("vpshp", CommandFlags.Modal | CommandFlags.NoTileMode)]
        static public void vpshp()
        {
            LCH.getCurrentDwgVars();

            GV.ed.SwitchToPaperSpace();

            PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");
            eo.AllowNone = false;
            eo.SetRejectMessage("\nNeed to select a viewport!");
            eo.AddAllowedClass(typeof(Viewport), true);
            //next lines are to allow for non-rectangular viewport selection
            eo.AddAllowedClass(typeof(Circle), true);
            eo.AddAllowedClass(typeof(Polyline), true);
            eo.AddAllowedClass(typeof(Polyline2d), true);
            eo.AddAllowedClass(typeof(Polyline3d), true);
            eo.AddAllowedClass(typeof(Ellipse), true);
            eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
            eo.AddAllowedClass(typeof(Spline), true);
            eo.AddAllowedClass(typeof(Face), true);

            PromptEntityResult er = GV.ed.GetEntity(eo);
            if (er.Status != PromptStatus.OK)
                return;

            using (Transaction trans = GV.Db.TransactionManager.StartTransaction())
            {
                Entity newEnt = null;
                Viewport vpEnt = null;

                Entity selEnt = (Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                if (selEnt.GetType() == typeof(Viewport))
                {
                    //Viewport is rectangular
                    vpEnt = (Viewport)selEnt;
                    Extents3d vpExt = vpEnt.GeometricExtents;
                    double w = vpExt.MaxPoint.X - vpExt.MinPoint.X;
                    double h = vpExt.MaxPoint.Y - vpExt.MinPoint.Y;
                    Polyline poly = new Polyline(4);
                    poly.AddVertexAt(0, new Point2d(0, 0), 0, -1, -1);
                    poly.AddVertexAt(1, new Point2d(w, 0), 0, -1, -1);
                    poly.AddVertexAt(2, new Point2d(w, h), 0, -1, -1);
                    poly.AddVertexAt(3, new Point2d(0, h), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;
                }
                else
                {
                    //Viewport is non-rectangular, attempt to get it from the selected clip entity
                    ObjectId vpId = LayoutManager.Current.GetNonRectangularViewportIdFromClipId(selEnt.Id);
                    if (vpId == ObjectId.Null)
                        vpEnt = null;
                    else
                    {
                        vpEnt = (Viewport)vpId.GetObject(OpenMode.ForRead);
                        newEnt = (Entity)selEnt.Clone();
                    }
                }

                if (vpEnt == null)
                {
                    GV.ed.WriteMessage("\nSelected object is not a viewport!");
                    trans.Commit();
                    return;
                }

                // Turn viewport on if needed
                if (!vpEnt.On)
                {
                    vpEnt.UpgradeOpen();
                    vpEnt.On = true;
                }

                //Activate a vport, set CVPORT to the selected vport, and get VIEWCTR of vport
                GV.ed.SwitchToModelSpace();
                AcAp.SetSystemVariable("CVPORT", vpEnt.Number);
                Point3d cenPnt = (Point3d)AcAp.GetSystemVariable("VIEWCTR");

                Extents3d entExt = newEnt.GeometricExtents;
                Point3d p1 = entExt.MinPoint;
                Point3d p2 = entExt.MaxPoint;
                Point3d extMid = new Point3d((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                double entHeight = p2.Y - p1.Y;

                Vector3d zAxis = vpEnt.ViewDirection;
                Vector3d xAxis = zAxis.GetPerpendicularVector().GetNormal();
                Vector3d yAxis = zAxis.CrossProduct(xAxis).GetNormal();
                zAxis = zAxis.GetNormal();

                Matrix3d transMat;
                transMat = Matrix3d.AlignCoordinateSystem(extMid, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis,
                    cenPnt, xAxis, yAxis, zAxis);
                newEnt.TransformBy(transMat);

                transMat = Matrix3d.Scaling(vpEnt.ViewHeight / entHeight, cenPnt);
                newEnt.TransformBy(transMat);

                transMat = Matrix3d.Rotation(-vpEnt.TwistAngle, zAxis, cenPnt);
                newEnt.TransformBy(transMat);

                BlockTable bt = (BlockTable)GV.Db.BlockTableId.GetObject(OpenMode.ForRead);
                BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                GV.viewportShpObj = ms.AppendEntity(newEnt);
                trans.AddNewlyCreatedDBObject(newEnt, true);

                //get polygon vertices.
                Polyline lwp = newEnt as Polyline;
                Point3dCollection p3dColl = new Point3dCollection();
                if (lwp != null)
                {
                    // Use a for loop to get each vertex, one by one
                    int vn = lwp.NumberOfVertices;
                    for (int i = 0; i < vn; i++)
                    {
                        // Could also get the 3D point here
                        Point3d pt = lwp.GetPoint3dAt(i);
                        GV.ed.WriteMessage("\n" + pt.ToString());
                        p3dColl.Add(pt);
                    }
                }
                trans.Commit();
                SelectionFilter filter = new SelectionFilter(Helper.LocalCADHelper.selectionFilter(GV.labelFilterType));
                PromptSelectionResult selection = GV.ed.SelectWindowPolygon(p3dColl, filter);
                if (selection.Status != PromptStatus.OK)
                    return;

                GV.selObjects_forProcessing = selection.Value.GetObjectIds();

                GV.ed.SwitchToPaperSpace();

                mc = new Apps.MainControl();

                mc.bw.WorkerSupportsCancellation = true;
                mc.bw.WorkerReportsProgress = true;
                //bw.ProgressChanged += bw_ProgressChanged;

                mc.bw.DoWork += new DoWorkEventHandler(mc.bw_UpdateProgressBar);

                //start work
                if (mc.bw.IsBusy != true)
                {
                    mc.bw.RunWorkerAsync();
                }

                int index = 1;

                int objCount = GV.selObjects_forProcessing.Length;
                GV.pBarMaxVal = objCount;

                foreach (ObjectId objID in GV.selObjects_forProcessing)
                {
                    LCH.getlabelvalueSpecific(objID);

                    #region ProgressBAR

                    GH.printDebug("", "", false, true);
                    GV.pBarStatus = "Labels Processed: " + index + @"/" + objCount;
                    mc.UpdateProgressBar(index, objCount, GV.pBarStatus);
                    //GV.pmeter.MeterProgress();
                    Helper.UIHelper.DoEvents();

                    GV.pBarCurrentVal = index;

                    //assign it work
                    //bw.ReportProgress(index);
                    index++;
                    #endregion
                }

                mc.updateUIdata();

                Helper.UIHelper.toastIT("All selected labels processed successfully!", "Status", NotificationType.Success);

            }
        }

        [CommandMethod("VPS", CommandFlags.Modal | CommandFlags.NoTileMode)]
        static public void VPS()
        {
            Document curDoc = AcAp.DocumentManager.MdiActiveDocument;
            Database db = curDoc.Database;
            Editor ed = curDoc.Editor;

            ed.SwitchToPaperSpace();

            PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");
            eo.AllowNone = false;
            eo.SetRejectMessage("\nNeed to select a viewport!");
            eo.AddAllowedClass(typeof(Viewport), true);
            //next lines are to allow for non-rectangular viewport selection
            eo.AddAllowedClass(typeof(Circle), true);
            eo.AddAllowedClass(typeof(Polyline), true);
            eo.AddAllowedClass(typeof(Polyline2d), true);
            eo.AddAllowedClass(typeof(Polyline3d), true);
            eo.AddAllowedClass(typeof(Ellipse), true);
            eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
            eo.AddAllowedClass(typeof(Spline), true);
            eo.AddAllowedClass(typeof(Face), true);

            PromptEntityResult er = ed.GetEntity(eo);
            if (er.Status != PromptStatus.OK)
                return;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity newEnt = null;
                Viewport vpEnt = null;

                Entity selEnt = (Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                if (selEnt.GetType() == typeof(Viewport))
                {
                    //Viewport is rectangular
                    vpEnt = (Viewport)selEnt;
                    Extents3d vpExt = vpEnt.GeometricExtents;
                    double minX = vpExt.MinPoint.X;
                    double minY = vpExt.MinPoint.Y;
                    double maxX = vpExt.MaxPoint.X;
                    double maxY = vpExt.MaxPoint.Y;
                    Polyline poly = new Polyline(4);
                    poly.AddVertexAt(0, new Point2d(minX, minY), 0, -1, -1);
                    poly.AddVertexAt(1, new Point2d(maxX, minY), 0, -1, -1);
                    poly.AddVertexAt(2, new Point2d(maxX, maxY), 0, -1, -1);
                    poly.AddVertexAt(3, new Point2d(minX, maxY), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;
                }
                else
                {
                    //Viewport is non-rectangular, attempt to get it from the selected clip entity
                    ObjectId vpId = LayoutManager.Current.GetNonRectangularViewportIdFromClipId(selEnt.Id);
                    if (vpId == ObjectId.Null)
                        vpEnt = null;
                    else
                    {
                        vpEnt = (Viewport)vpId.GetObject(OpenMode.ForRead);
                        newEnt = (Entity)selEnt.Clone();
                    }
                }

                if (vpEnt == null)
                {
                    ed.WriteMessage("\nSelected object is not a viewport!");
                    trans.Commit();
                    return;
                }

                // Turn viewport on if needed
                if (!vpEnt.On)
                {
                    vpEnt.UpgradeOpen();
                    vpEnt.On = true;
                }

                //Activate a vport, set CVPORT to the selected vport, and get VIEWCTR of vport
                ed.SwitchToModelSpace();
                AcAp.SetSystemVariable("CVPORT", vpEnt.Number);
                //Point3d cenPnt = (Point3d)AcAp.GetSystemVariable("VIEWCTR");

                //Extents3d entExt = newEnt.GeometricExtents;
                //Point3d p1 = entExt.MinPoint;
                //Point3d p2 = entExt.MaxPoint;
                //Point3d extMid = new Point3d((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                //double entHeight = p2.Y - p1.Y;

                //Vector3d zAxis = vpEnt.ViewDirection;
                //Vector3d xAxis = zAxis.GetPerpendicularVector().GetNormal();
                //Vector3d yAxis = zAxis.CrossProduct(xAxis).GetNormal();
                //zAxis = zAxis.GetNormal();

                //Matrix3d transMat;
                //transMat = Matrix3d.AlignCoordinateSystem(extMid, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis,
                //    cenPnt, xAxis, yAxis, zAxis);
                //newEnt.TransformBy(transMat);

                //transMat = Matrix3d.Scaling(vpEnt.ViewHeight / entHeight, cenPnt);
                //newEnt.TransformBy(transMat);

                //transMat = Matrix3d.Rotation(-vpEnt.TwistAngle, zAxis, cenPnt);

                Matrix3d transMat = Helper.ViewportExtensions.PaperToModel(vpEnt);
                newEnt.TransformBy(transMat);

                BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                //GV.viewportShpObj = ms.AppendEntity(newEnt);
                ms.AppendEntity(newEnt);
                trans.AddNewlyCreatedDBObject(newEnt, true);

                ed.SwitchToPaperSpace();
                trans.Commit();
            }
        }

        [CommandMethod("vmsel", CommandFlags.Modal | CommandFlags.NoTileMode)]
        static public void vpstoModel()
        {
            Document curDoc = AcAp.DocumentManager.MdiActiveDocument;
            Database db = curDoc.Database;
            Editor ed = curDoc.Editor;

            ed.SwitchToPaperSpace();

            PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");
            eo.AllowNone = false;
            eo.SetRejectMessage("\nNeed to select a viewport!");
            eo.AddAllowedClass(typeof(Viewport), true);
            //next lines are to allow for non-rectangular viewport selection
            eo.AddAllowedClass(typeof(Circle), true);
            eo.AddAllowedClass(typeof(Polyline), true);
            eo.AddAllowedClass(typeof(Polyline2d), true);
            eo.AddAllowedClass(typeof(Polyline3d), true);
            eo.AddAllowedClass(typeof(Ellipse), true);
            eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
            eo.AddAllowedClass(typeof(Spline), true);
            eo.AddAllowedClass(typeof(Face), true);

            PromptEntityResult er = ed.GetEntity(eo);
            if (er.Status != PromptStatus.OK)
                return;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity newEnt = null;
                Viewport vpEnt = null;

                Entity selEnt = (Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                if (selEnt.GetType() == typeof(Viewport))
                {
                    //Viewport is rectangular
                    vpEnt = (Viewport)selEnt;
                    Extents3d vpExt = vpEnt.GeometricExtents;
                    double minX = vpExt.MinPoint.X;
                    double minY = vpExt.MinPoint.Y;
                    double maxX = vpExt.MaxPoint.X;
                    double maxY = vpExt.MaxPoint.Y;
                    Polyline poly = new Polyline(4);
                    poly.AddVertexAt(0, new Point2d(minX, minY), 0, -1, -1);
                    poly.AddVertexAt(1, new Point2d(maxX, minY), 0, -1, -1);
                    poly.AddVertexAt(2, new Point2d(maxX, maxY), 0, -1, -1);
                    poly.AddVertexAt(3, new Point2d(minX, maxY), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;
                }
                else
                {
                    //Viewport is non-rectangular, attempt to get it from the selected clip entity
                    ObjectId vpId = LayoutManager.Current.GetNonRectangularViewportIdFromClipId(selEnt.Id);
                    if (vpId == ObjectId.Null)
                        vpEnt = null;
                    else
                    {
                        vpEnt = (Viewport)vpId.GetObject(OpenMode.ForRead);
                        newEnt = (Entity)selEnt.Clone();
                    }
                }

                if (vpEnt == null)
                {
                    ed.WriteMessage("\nSelected object is not a viewport!");
                    trans.Commit();
                    return;
                }

                // Turn viewport on if needed
                if (!vpEnt.On)
                {
                    vpEnt.UpgradeOpen();
                    vpEnt.On = true;
                }

                Matrix3d transMat = Helper.ViewportExtensions.PaperToModel(vpEnt);
                newEnt.TransformBy(transMat);

                Polyline lwp = newEnt as Polyline;
                if (lwp != null && lwp.NumberOfVertices > 2)
                {
                    BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                    BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                    //GV.viewportShpObj = ms.AppendEntity(newEnt);
                    ms.AppendEntity(newEnt);
                    trans.AddNewlyCreatedDBObject(newEnt, true);

                    //Switch to modelspace
                    LayoutManager.Current.CurrentLayout = "Model";

                    Point3dCollection p3dColl = new Point3dCollection();
                    for (int i = 0; i < lwp.NumberOfVertices; i++)
                    {
                        // Could also get the 3D point here
                        Point3d pt = lwp.GetPoint3dAt(i);
                        //ed.WriteMessage("\n" + pt.ToString());
                        p3dColl.Add(pt);
                    }

                    Extents3d ext = newEnt.GeometricExtents;
                    double margin = 0.1 * ext.MinPoint.DistanceTo(ext.MaxPoint);
                    Helper.LocalCADHelper.ZoomEntity(ed, newEnt, margin);

                    #region Filter
                    //select only labels - filter using the details available in the mapper.
                    // Build a conditional filter list so that only
                    // entities with the specified properties are
                    // selected
                    TypedValue[] tvArr = new TypedValue[]
                                {
                                new TypedValue((int)DxfCode.Operator, "<or"),
                                new TypedValue((int)DxfCode.Start, "AECC_PIPE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STRUCTURE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_NOTE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_SEGMENT_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STATION_OFFSET_LABEL"),
                                new TypedValue((int)DxfCode.Start, "LINE"), // for debug only
                                new TypedValue((int)DxfCode.Operator, "or>"),
                                    };
                    #endregion

                    SelectionFilter filter = new SelectionFilter(tvArr);
                    PromptSelectionResult selection = ed.SelectWindowPolygon(p3dColl, filter);
                    if (selection.Status != PromptStatus.OK)
                    {
                        ed.WriteMessage("\nCann't select any objects.");
                        return;
                    }

                    ed.SetImpliedSelection(selection.Value.GetObjectIds());
                }
                trans.Commit();
            }
        }

        [CommandMethod("vvv", CommandFlags.Modal | CommandFlags.NoTileMode)]
        static public void VPTOMS1()
        {
            Document curDoc = AcAp.DocumentManager.MdiActiveDocument;
            Database db = curDoc.Database;
            Editor ed = curDoc.Editor;

            ed.SwitchToPaperSpace();

            PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");
            eo.AllowNone = false;
            eo.SetRejectMessage("\nNeed to select a viewport!");
            eo.AddAllowedClass(typeof(Viewport), true);
            //next lines are to allow for non-rectangular viewport selection
            eo.AddAllowedClass(typeof(Circle), true);
            eo.AddAllowedClass(typeof(Polyline), true);
            eo.AddAllowedClass(typeof(Polyline2d), true);
            eo.AddAllowedClass(typeof(Polyline3d), true);
            eo.AddAllowedClass(typeof(Ellipse), true);
            eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
            eo.AddAllowedClass(typeof(Spline), true);
            eo.AddAllowedClass(typeof(Face), true);

            PromptEntityResult er = ed.GetEntity(eo);
            if (er.Status != PromptStatus.OK)
                return;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                Entity newEnt = null;
                Viewport vpEnt = null;

                Entity selEnt = (Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                if (selEnt.GetType() == typeof(Viewport))
                {
                    //Viewport is rectangular
                    vpEnt = (Viewport)selEnt;
                    Extents3d vpExt = vpEnt.GeometricExtents;
                    double minX = vpExt.MinPoint.X;
                    double minY = vpExt.MinPoint.Y;
                    double w = vpExt.MaxPoint.X - vpExt.MinPoint.X;
                    double h = vpExt.MaxPoint.Y - vpExt.MinPoint.Y;
                    Polyline poly = new Polyline(4);
                    poly.AddVertexAt(0, new Point2d(minX, minY), 0, -1, -1);
                    poly.AddVertexAt(1, new Point2d(minX + w, minY), 0, -1, -1);
                    poly.AddVertexAt(2, new Point2d(minX + w, minY + h), 0, -1, -1);
                    poly.AddVertexAt(3, new Point2d(minX, minY + h), 0, -1, -1);
                    poly.Closed = true;
                    newEnt = poly;
                }
                else
                {
                    //Viewport is non-rectangular, attempt to get it from the selected clip entity
                    ObjectId vpId = LayoutManager.Current.GetNonRectangularViewportIdFromClipId(selEnt.Id);
                    if (vpId == ObjectId.Null)
                        vpEnt = null;
                    else
                    {
                        vpEnt = (Viewport)vpId.GetObject(OpenMode.ForRead);
                        newEnt = (Entity)selEnt.Clone();
                    }
                }

                if (vpEnt == null)
                {
                    ed.WriteMessage("\nSelected object is not a viewport!");
                    trans.Commit();
                    return;
                }

                // Turn viewport on if needed
                if (!vpEnt.On)
                {
                    vpEnt.UpgradeOpen();
                    vpEnt.On = true;
                }

                //Activate a vport, set CVPORT to the selected vport, and get VIEWCTR of vport
                //ed.SwitchToModelSpace();

                Matrix3d transMat = Helper.ViewportExtensions.PaperToModel(vpEnt);
                newEnt.TransformBy(transMat);


                Polyline lwp = newEnt as Polyline;
                if (lwp != null && lwp.NumberOfVertices > 2)
                {
                    BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                    BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                    //GV.viewportShpObj = ms.AppendEntity(newEnt);
                    ms.AppendEntity(newEnt);
                    trans.AddNewlyCreatedDBObject(newEnt, true);

                    string cLayout = LayoutManager.Current.CurrentLayout;
                    // Switch to modelspace
                    LayoutManager.Current.CurrentLayout = "Model";

                    Point3dCollection p3dColl = new Point3dCollection();
                    for (int i = 0; i < lwp.NumberOfVertices; i++)
                    {
                        // Could also get the 3D point here
                        Point3d pt = lwp.GetPoint3dAt(i);
                        //ed.WriteMessage("\n" + pt.ToString());
                        p3dColl.Add(pt);
                    }

                    Extents3d ext = newEnt.GeometricExtents;
                    double margin = 0.1 * ext.MinPoint.DistanceTo(ext.MaxPoint);

                    //double[] p1 = new double[] { ext.MinPoint.X - margin, ext.MinPoint.Y - margin, 0.00 };
                    //double[] p2 = new double[] { ext.MaxPoint.X + margin, ext.MaxPoint.Y + margin, 0.00 };
                    //dynamic acadApp = Application.AcadApplication;
                    //acadApp.ZoomWindow(p1, p2);
                    Helper.LocalCADHelper.ZoomEntity(ed, newEnt, margin);

                    #region Filter
                    //select only labels - filter using the details available in the mapper.
                    // Build a conditional filter list so that only
                    // entities with the specified properties are
                    // selected
                    TypedValue[] tvArr = new TypedValue[]
                                {
                                new TypedValue((int)DxfCode.Operator, "<or"),
                                new TypedValue((int)DxfCode.Start, "AECC_PIPE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STRUCTURE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_NOTE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_SEGMENT_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STATION_OFFSET_LABEL"),
                                new TypedValue((int)DxfCode.Start, "LINE"), // for debug only
                                new TypedValue((int)DxfCode.Operator, "or>"),
                                    };

                    #endregion

                    SelectionFilter filter = new SelectionFilter(tvArr);
                    PromptSelectionResult selection = ed.SelectWindowPolygon(p3dColl, filter);
                    if (selection.Status != PromptStatus.OK)
                    {
                        ed.WriteMessage("\nCann't select any objects.");
                        return;
                    }

                    // Switch to paperspace
                    LayoutManager.Current.CurrentLayout = cLayout;
                    ObjectId[] idArr = selection.Value.GetObjectIds();
                    ed.WriteMessage("\nSelected {0} objects.", idArr.Length);

                    //ed.SetImpliedSelection(selection.Value.GetObjectIds());
                }
                trans.Commit();
            }
        }
    }
    public static class ViewportExtensions
    {
        /// <summary>
        /// Gets the transformation matrix from the specified model space viewport Display Coordinate System (DCS)
        /// to the World Coordinate System (WCS).
        /// </summary>
        /// <param name="vp">The instance to which this method applies.</param>
        /// <returns>The DCS to WDCS transformation matrix.</returns>
        public static Matrix3d DCS2WCS(this Viewport vp)
        {
            return
                Matrix3d.Rotation(-vp.TwistAngle, vp.ViewDirection, vp.ViewTarget) *
                Matrix3d.Displacement(vp.ViewTarget - Point3d.Origin) *
                Matrix3d.PlaneToWorld(vp.ViewDirection);
        }

        /// <summary>
        /// Gets the transformation matrix from the World Coordinate System (WCS)
        /// to the specified model space viewport Display Coordinate System (DCS).
        /// </summary>
        /// <param name="vp">The instance to which this method applies.</param>
        /// <returns>The WCS to DCS transformation matrix.</returns>
        public static Matrix3d WCS2DCS(this Viewport vp)
        {
            return vp.DCS2WCS().Inverse();
        }

        /// <summary>
        /// Gets the transformation matrix from the specified paper space viewport Display Coordinate System (DCS)
        /// to the paper space Display Coordinate System (PSDCS).
        /// </summary>
        /// <param name="vp">The instance to which this method applies.</param>
        /// <returns>The DCS to PSDCS transformation matrix.</returns>
        public static Matrix3d DCS2PSDCS(this Viewport vp)
        {
            return
                Matrix3d.Scaling(vp.CustomScale, vp.CenterPoint) *
                Matrix3d.Displacement(vp.CenterPoint.GetAsVector()) *
                Matrix3d.Displacement(new Vector3d(-vp.ViewCenter.X, -vp.ViewCenter.Y, 0.0));
        }

        /// <summary>
        /// Gets the transformation matrix from the Paper Space Display Coordinate System (PSDCS)
        /// to the specified paper space viewport Display Coordinate System (DCS). 
        /// </summary>
        /// <param name="vp">The instance to which this method applies.</param>
        /// <returns>The PSDCS to DCS transformation matrix.</returns>
        public static Matrix3d PSDCS2DCS(this Viewport vp)
        {
            return vp.DCS2PSDCS().Inverse();
        }
    }

}
