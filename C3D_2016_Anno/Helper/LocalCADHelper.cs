using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = C3D_2016_Anno.Global.variables;
using GH = C3D_2016_Anno.Helper.GenHelper;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System.Runtime.InteropServices;
using CadDb = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.DatabaseServices;
using System.Collections.ObjectModel;
using Autodesk.AutoCAD.GraphicsInterface;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.Runtime;
using System.ComponentModel;
using DS = Autodesk.AutoCAD.DatabaseServices;

namespace C3D_2016_Anno.Helper
{
    public class LocalCADHelper
    {
        public static Apps.MainControl MC = new Apps.MainControl();
        private static bool boolRes;
        public static void createMtextwithJIG(string text)
        {
            try
            {
                using (GV.Doc.LockDocument())
                {
                    // Variables for our MText entity's identity
                    // and location
                    ObjectId mtId;
                    Point3d mtLoc = Point3d.Origin;

                    using (var trans = GV.Db.TransactionManager.StartTransaction())
                    {
                        //MText mt = new MText();
                        //mt.Location = mtLoc;
                        //mt.Contents = mTextLabel;

                        // Open the block table, the model space and
                        // add our MText

                        BlockTable bt = (BlockTable)trans.GetObject(GV.Db.BlockTableId, OpenMode.ForRead);
                        BlockTableRecord btr;
                        if ((short)AcAp.GetSystemVariable("CVPORT") > 1)
                        {

                            btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                        }
                        else
                        {
                            btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForWrite);
                        }

                        // Create the text object, set its normal and contents
                        MText txt = new MText();
                        txt.Normal = GV.ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis;
                        txt.Contents = text;

                        // We'll add the text to the database before jigging
                        // it - this allows alignment adjustments to be
                        // reflected
                        btr.AppendEntity(txt);
                        trans.AddNewlyCreatedDBObject(txt, true);

                        // Create our jig
                        Helper.TextPlacementJig pj = new Helper.TextPlacementJig(trans, GV.Db, txt);

                        // Loop as we run our jig, as we may have keywords
                        PromptStatus stat = PromptStatus.Keyword;
                        
                        while (stat == PromptStatus.Keyword)
                        {
                            PromptResult res = GV.ed.Drag(pj);
                            stat = res.Status;
                            if (
                              stat != PromptStatus.OK &&
                              stat != PromptStatus.Keyword
                            )
                                return;
                        }

                        // Finally we commit our transaction
                        trans.Commit();
                    }
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

        public static void getCurrentDwgVars()
        {
            try
            {
                GV.Doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                if (GV.Doc.IsActive == false)
                {

                }
                GV.ed = GV.Doc.Editor;
                GV.Db = GV.Doc.Database;
                HostApplicationServices.WorkingDatabase = GV.Db;
                GV.CDoc = Autodesk.Civil.ApplicationServices.CivilApplication.ActiveDocument;
                //Application.DocumentManager.DocumentActivationEnabled = true;
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
        public static TypedValue[] selFilterRes;
        public static TypedValue[] selectionFilter(string objType)
        {
            try
            {

                switch (objType)
                {
                    #region Filter
                    case "allLabels":
                        {
                            //select only labels - filter using the details available in the mapper.
                            // Build a conditional filter list so that only
                            // entities with the specified properties are
                            // selected
                            selFilterRes = new TypedValue[]
                            {
                                new TypedValue((int)DxfCode.Operator, "<or"),
                                new TypedValue((int)DxfCode.Start, "AECC_PIPE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STRUCTURE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_NOTE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_SEGMENT_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STATION_OFFSET_LABEL"),
                                new TypedValue((int)DxfCode.Start, "MULTILEADER"),
                                new TypedValue((int)DxfCode.Operator, "or>"),
                                };
                        }
                        break;

                    case "civilLabels":
                        {
                            //select only labels - filter using the details available in the mapper.
                            // Build a conditional filter list so that only
                            // entities with the specified properties are
                            // selected
                            selFilterRes = new TypedValue[]
                            {
                                new TypedValue((int)DxfCode.Operator, "<or"),
                                new TypedValue((int)DxfCode.Start, "AECC_PIPE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STRUCTURE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_NOTE_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_GENERAL_SEGMENT_LABEL"),
                                new TypedValue((int)DxfCode.Start, "AECC_STATION_OFFSET_LABEL"),
                                new TypedValue((int)DxfCode.Operator, "or>"),
                                };
                        }
                        break;
                        #endregion
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
            return selFilterRes;
        }

        [DllImport("accore.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "acedTrans")]
        static extern int acedTrans(double[] point, IntPtr fromRb, IntPtr toRb, int disp, double[] result);
        public static void getViewportObj(ObjectId viewportobjID)
        {
            try
            {
                int vpNumber = 0;
                using (Transaction trans = GV.Db.TransactionManager.StartTransaction())
                {
                    // extract the viewport points
                    Point3dCollection psVpPnts = new Point3dCollection();
                    using (Autodesk.AutoCAD.DatabaseServices.Viewport psVp = (Autodesk.AutoCAD.DatabaseServices.Viewport)trans.GetObject(viewportobjID, OpenMode.ForWrite))// Autodesk.AutoCAD.DatabaseServices.Viewport psVp = viewportobjID.GetObject(OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Viewport)
                    {
                        // get the vp number
                        vpNumber = psVp.Number;

                        // now extract the viewport geometry
                        psVp.GetGripPoints(psVpPnts, new IntegerCollection(), new IntegerCollection());

                    }

                    // let's assume a rectangular vport for now, make the cross-direction grips square
                    Point3d tmp = psVpPnts[2];
                    psVpPnts[2] = psVpPnts[1];
                    psVpPnts[1] = tmp;

                    // Transform the PS points to Modal space points
                    ResultBuffer rbFrom = new ResultBuffer(new TypedValue(5003, 3));
                    ResultBuffer rbTo = new ResultBuffer(new TypedValue(5003, 2));
                    double[] retPoint = new double[] { 0, 0, 0 };

                    // loop the ps points 
                    Point3dCollection msVpPnts = new Point3dCollection();

                    BlockTable acBlkTbl;
                    acBlkTbl = trans.GetObject(GV.Db.BlockTableId,
                                                    OpenMode.ForRead) as BlockTable;

                    // Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = trans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    foreach (Point3d pnt in psVpPnts)
                    {
                        // translate from from the DCS of Paper Space (PSDCS) RTSHORT=3 and 
                        // the DCS of the current model space viewport RTSHORT=2
                        acedTrans(pnt.ToArray(), rbFrom.UnmanagedObject, rbTo.UnmanagedObject, 0, retPoint);
                        // add the resulting point to the ms pnt array
                        msVpPnts.Add(new Point3d(retPoint));
                        GV.ed.WriteMessage("\n" + new Point3d(retPoint).ToString());
                    }

                    // Create a circle that is at 2,3 with a radius of 4.25
                    using (CadDb.Polyline acPoly = new CadDb.Polyline())
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

                    // now switch to MS
                    GV.ed.SwitchToModelSpace();

                    // set the CVPort
                    Application.SetSystemVariable("CVPORT", vpNumber);

                    // once switched, we can use the normal selection mode to select
                    PromptSelectionResult selectionresult = GV.ed.SelectCrossingWindow(msVpPnts[0], msVpPnts[2], new SelectionFilter(selectionFilter("allLabels")));

                    // If the prompt status is OK, objects were selected
                    if (selectionresult.Status == PromptStatus.OK)
                    {
                        SelectionSet acSSet = selectionresult.Value;

                        Application.ShowAlertDialog("Number of objects selected: " +
                                                    acSSet.Count.ToString());
                    }
                    else
                    {
                        Application.ShowAlertDialog("Number of objects selected: 0");
                    }

                    // now switch back to PS
                    GV.ed.SwitchToPaperSpace();
                    trans.Commit();
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
        public static void getlabelvalueSpecific(ObjectId objID)
        {
            
            //create background worker
            

            try
            {
                using (Transaction trans = GV.Db.TransactionManager.StartTransaction())
                {
                    #region Selection
                    
                    string labelNamePrefix;
                    string labelName;
                    Dictionary<string, string> dictRes = new Dictionary<string, string>();
                    Global.labelItem LI = new Global.labelItem();

                    //check if the object is Multileader
                    if (objID.ObjectClass.DxfName.ToString() == "MULTILEADER")
                    {
                        LI.name = "MULTILEADER";
                        labelNamePrefix = Helper.MultiLeaderHelper.getMultiLeaderName(objID);
                        dictRes = Helper.MultiLeaderHelper.getMultiLeaderAttValColl(objID, GV.MultiLeaderAtt);
                    }
                    else
                    {
                        labelName = Helper.LabelTextExtractor.getLabelName(objID);
                        labelNamePrefix = labelName.Split('-')[0];
                        GH.qprint(labelName + " | > >" + labelNamePrefix);
                        dictRes = Helper.LabelTextExtractor.getLabelVals(objID);
                        LI.name = labelName;
                    }
                    GH.printDebug("", "", true, false);

                    //get label values

                    //get the name prefix and compare it with mapper.
                    //check the key exists in the Mapper collection.
                    if (!GV.allnotes.Keys.Contains<string>(labelNamePrefix))
                    {
                        GH.qprint("labelNamePrefix not in all notes= =   " + labelNamePrefix);
                        GV.allnotes.Add(labelNamePrefix, dictRes);
                    }

                    #region add all the labels to a collection,


                    LI.prefix = labelNamePrefix;
                    LI.objID = objID;
                    LI.properties = dictRes;
                    foreach (string noteKey in GV.notenumberKey)
                    {
                        GH.qprint("--noteKey =   " + noteKey);
                        string result;

                        if (dictRes.TryGetValue(noteKey, out result))
                        {
                            GH.qprint("LI.noteNumber =   " + result);
                            LI.noteNumber = Convert.ToInt32(result);

                        }
                    }
                    if (GV.Mapper.ContainsKey(labelNamePrefix))
                    {
                        LI.objType = GV.Mapper[labelNamePrefix];
                        
                        if (GV.notesDict.ContainsKey(LI.objType))
                        {
                            if (GV.notesDict[LI.objType].ContainsKey(LI.noteNumber))
                            {
                                LI.noteFound = true;
                                LI.note = GV.notesDict[LI.objType][LI.noteNumber];
                                if (!GV.SelectedObjTypes.Contains(LI.objType))
                                    GV.SelectedObjTypes.Add(LI.objType);
                            }
                            else
                            {
                                LI.noteFound = false;
                                Apps.MainControl mc = new Apps.MainControl();
                                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)mc.cBox_template.SelectedItem;
                                LI.note = "Note details not found!, Check definition file";// + FI.fileName + ".xml";
                                if (!GV.SelectedObjTypes.Contains(LI.objType))
                                    GV.SelectedObjTypes.Add(LI.objType);
                            }
                        }
                        GV.all_label_coll.Add(LI);
                    }
                   
                    

                    #endregion
                    
                    #endregion

                    
                    trans.Commit();
                }
            }
            catch (System.AccessViolationException ex)
            {
                GH.errorBox(ex.ToString());
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
        public static void getlabelvalues()
        {
            MC = new Apps.MainControl();
            //create background worker
            

            try
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.WorkerReportsProgress = true;
                bw.DoWork += new DoWorkEventHandler(MC.bw_UpdateProgressBar);
                //start work
                if (bw.IsBusy != true)
                {
                    bw.RunWorkerAsync();
                }


                using (Transaction trans = GV.Db.TransactionManager.StartTransaction())
                {
                    #region Selection
                    int objCount = GV.selObjects_forProcessing.Count();
                    int index = 1;
                    foreach (ObjectId objID in GV.selObjects_forProcessing)
                    {

                        string labelNamePrefix;
                        string labelName;
                        Dictionary<string, string> dictRes = new Dictionary<string, string>();
                        Global.labelItem LI = new Global.labelItem();

                        //check if the object is Multileader
                        if (objID.ObjectClass.DxfName.ToString() == "MULTILEADER")
                        {
                            LI.name = "MULTILEADER";
                            labelNamePrefix = Helper.MultiLeaderHelper.getMultiLeaderName(objID);
                            dictRes = Helper.MultiLeaderHelper.getMultiLeaderAttValColl(objID, GV.MultiLeaderAtt);
                        }
                        else
                        {
                            labelName = Helper.LabelTextExtractor.getLabelName(objID);
                            labelNamePrefix = labelName.Split('-')[0];
                            GH.qprint(labelName + " | > >" + labelNamePrefix);
                            dictRes = Helper.LabelTextExtractor.getLabelVals(objID);
                            LI.name = labelName;
                        }
                        GH.printDebug("", "", true, false);

                        //get label values

                        //get the name prefix and compare it with mapper.
                        //check the key exists in the Mapper collection.
                        if (!GV.allnotes.Keys.Contains<string>(labelNamePrefix))
                        {
                            GH.qprint("labelNamePrefix not in all notes= =   " + labelNamePrefix);
                            GV.allnotes.Add(labelNamePrefix, dictRes);
                        }

                        #region add all the labels to a collection,


                        LI.prefix = labelNamePrefix;

                        LI.properties = dictRes;
                        foreach (string noteKey in GV.notenumberKey)
                        {
                            GH.qprint("--noteKey =   " + noteKey);
                            string result;

                            if (dictRes.TryGetValue(noteKey, out result))
                            {
                                GH.qprint("LI.noteNumber =   " + result);
                                LI.noteNumber = Convert.ToInt32(result);

                            }
                        }
                        if (GV.Mapper.ContainsKey(labelNamePrefix))
                        {
                            LI.objType = GV.Mapper[labelNamePrefix];
                        }
                        LI.objID = objID;
                        if (GV.notesDict.ContainsKey(LI.objType))
                        {
                            if (GV.notesDict[LI.objType].ContainsKey(LI.noteNumber))
                            {
                                LI.note = GV.notesDict[LI.objType][LI.noteNumber];
                                if (!GV.SelectedObjTypes.Contains(LI.objType))
                                    GV.SelectedObjTypes.Add(LI.objType);
                            }
                        }
                        GV.all_label_coll.Add(LI);

                        #endregion

                        #region ProgressBAR
                        GH.printDebug("", "", false, true);
                        GV.pBarStatus = "Labels Processed: " + index + @"/" + objCount;
                        MC.UpdateProgressBar(index, objCount, GV.pBarStatus);
                        Helper.UIHelper.DoEvents();
                        index++; 
                        #endregion
                    }
                    #endregion

                    #region Collection

                    #endregion
                    trans.Commit();
                }
            }
            catch(System.AccessViolationException ex)
            {
                GH.errorBox(ex.ToString());
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
        public static ObservableCollection<ObjectId> convertObjArraytoCollection(ObjectId[] objids)
        {

            ObservableCollection<ObjectId> selObjIds = new ObservableCollection<ObjectId>();
            foreach (ObjectId objid in objids)
            {
                if (!selObjIds.Contains(objid))
                    selObjIds.Add(objid);
            }
            return selObjIds;
        }

        private static void ZoomWindow(Editor ed, Point3d ptMin, Point3d ptMax, double dMargin)
        {
            Point2d ptMin2d = new Point2d(ptMin.X - dMargin, ptMin.Y - dMargin);
            Point2d ptMax2d = new Point2d(ptMax.X + dMargin, ptMax.Y + dMargin);

            ViewTableRecord view = new ViewTableRecord();
            view.CenterPoint = ptMin2d + ((ptMax2d - ptMin2d) / 2.0);
            view.Width = ptMax2d.X - ptMin2d.X;
            view.Height = ptMax2d.Y - ptMin2d.Y;

            ed.SetCurrentView(view);
        }

        public static bool ZoomEntity(Editor ed, CadDb.Entity ent, double dMargin)
        {
            try
            {
                Extents3d ext = ent.GeometricExtents;
                ext.TransformBy(ed.CurrentUserCoordinateSystem.Inverse());
                ZoomWindow(ed, ext.MinPoint, ext.MaxPoint, dMargin);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ZoomObjects(ObjectIdCollection idCol)
        {
            try
            {
                
                using (Transaction tr = GV.Db.TransactionManager.StartTransaction())
                using (ViewTableRecord view = GV.ed.GetCurrentView())
                {
                    Matrix3d WCS2DCS = Matrix3d.PlaneToWorld(view.ViewDirection);
                    WCS2DCS = Matrix3d.Displacement(view.Target - Point3d.Origin) * WCS2DCS;
                    WCS2DCS = Matrix3d.Rotation(-view.ViewTwist, view.ViewDirection, view.Target) * WCS2DCS;
                    WCS2DCS = WCS2DCS.Inverse();
                    CadDb.Entity ent = (CadDb.Entity)tr.GetObject(idCol[0], OpenMode.ForRead);
                    //Autodesk.Aec.DatabaseServices.Entity ent;// = (Autodesk.Aec.DatabaseServices.Entity)tr.GetObject(idCol[0], OpenMode.ForRead);
                    Extents3d ext = ent.GeometricExtents;
                    for (int i = 1; i < idCol.Count; i++)
                    {
                        if (idCol[i].ObjectClass.DxfName.ToString() == "MULTILEADER")
                        {

                        }
                        
                        ent = (Autodesk.Aec.DatabaseServices.Entity)tr.GetObject(idCol[i], OpenMode.ForRead);
                        Extents3d tmp = ent.GeometricExtents;
                        ext.AddExtents(tmp);
                    }
                    ext.TransformBy(WCS2DCS);
                    view.Width = ext.MaxPoint.X - ext.MinPoint.X;
                    view.Height = ext.MaxPoint.Y - ext.MinPoint.Y;
                    view.CenterPoint =
                        new Point2d((ext.MaxPoint.X + ext.MinPoint.X) / 2.0, (ext.MaxPoint.Y + ext.MinPoint.Y) / 2.0);
                    GV.ed.SetCurrentView(view);
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

        public static void getCurrentFont()
        {
            try
            {
                using (Transaction acTrans = GV.Db.TransactionManager.StartTransaction())
                {
                    // Open the current text style for write
                    TextStyleTableRecord acTextStyleTblRec;
                    acTextStyleTblRec = acTrans.GetObject(GV.Db.Textstyle, OpenMode.ForRead) as TextStyleTableRecord;

                    // Get the current font settings
                    Autodesk.AutoCAD.GraphicsInterface.FontDescriptor acFont;
                    acFont = acTextStyleTblRec.Font;
                    GV.currentFont = acFont.TypeFace;
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
    }

    public static class ViewportExtensions
    {
        public static Matrix3d PaperToModel(CadDb.Viewport vp)
        {
            Matrix3d mx = ModelToPaper(vp);
            return mx.Inverse();
        }

        public static Matrix3d ModelToPaper(CadDb.Viewport vp)
        {
            Vector3d vd = vp.ViewDirection;
            Point3d vc = new Point3d(vp.ViewCenter.X, vp.ViewCenter.Y, 0);
            Point3d vt = vp.ViewTarget;
            Point3d cp = vp.CenterPoint;
            double ta = -vp.TwistAngle;
            double vh = vp.ViewHeight;
            double height = vp.Height;
            double width = vp.Width;
            double scale = vh / height;
            double lensLength = vp.LensLength;
            Vector3d zaxis = vd.GetNormal();
            Vector3d xaxis = Vector3d.ZAxis.CrossProduct(vd);
            Vector3d yaxis;

            if (!xaxis.IsZeroLength())
            {
                xaxis = xaxis.GetNormal();
                yaxis = zaxis.CrossProduct(xaxis);
            }
            else if (zaxis.Z < 0)
            {
                xaxis = Vector3d.XAxis * -1;
                yaxis = Vector3d.YAxis;
                zaxis = Vector3d.ZAxis * -1;
            }
            else
            {
                xaxis = Vector3d.XAxis;
                yaxis = Vector3d.YAxis;
                zaxis = Vector3d.ZAxis;
            }
            Matrix3d pcsToDCS = Matrix3d.Displacement(Point3d.Origin - cp);
            pcsToDCS = pcsToDCS * Matrix3d.Scaling(scale, cp);
            Matrix3d dcsToWcs = Matrix3d.Displacement(vc - Point3d.Origin);
            Matrix3d mxCoords = Matrix3d.AlignCoordinateSystem(
                Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis,
                Vector3d.ZAxis, Point3d.Origin,
                xaxis, yaxis, zaxis);
            dcsToWcs = mxCoords * dcsToWcs;
            dcsToWcs = Matrix3d.Displacement(vt - Point3d.Origin) * dcsToWcs;
            dcsToWcs = Matrix3d.Rotation(ta, zaxis, vt) * dcsToWcs;

            Matrix3d perspectiveMx = Matrix3d.Identity;
            if (vp.PerspectiveOn)
            {
                double vSize = vh;
                double aspectRatio = width / height;
                double adjustFactor = 1.0 / 42.0;
                double adjstLenLgth = vSize * lensLength * System.Math.Sqrt(1.0 + aspectRatio * aspectRatio) * adjustFactor;
                double iDist = vd.Length;
                double lensDist = iDist - adjstLenLgth;
                double[] dataAry = new double[]
                 {
                     1,0,0,0,0,1,0,0,0,0,
                     (adjstLenLgth-lensDist)/adjstLenLgth,
                     lensDist*(iDist-adjstLenLgth)/adjstLenLgth,
                     0,0,-1.0/adjstLenLgth,iDist/adjstLenLgth
                 };

                perspectiveMx = new Matrix3d(dataAry);
            }

            Matrix3d finalMx = pcsToDCS.Inverse() * perspectiveMx * dcsToWcs.Inverse();

            return finalMx;
        }
        public static void togglespace()
        {
            
            try
            {
                if ((short)AcAp.GetSystemVariable("CVPORT") != 1)
                {
                    GV.ed.SwitchToPaperSpace();
                }
                else
                {
                    GV.ed.SwitchToModelSpace();
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

        public static void toggleModelspace()
        {

            try
            {
                if ((short)AcAp.GetSystemVariable("CVPORT") != 1)
                {
                    //GV.ed.SwitchToPaperSpace();
                }
                else
                {
                    GV.ed.SwitchToModelSpace();
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

        public static void selVPort()
        {
            try
            {
                GV.ed.SwitchToPaperSpace();

                PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");
                eo.AllowNone = false;
                eo.SetRejectMessage("\nNeed to select a viewport!");
                eo.AddAllowedClass(typeof(CadDb.Viewport), true);
                //next lines are to allow for non-rectangular viewport selection
                eo.AddAllowedClass(typeof(Circle), true);
                eo.AddAllowedClass(typeof(CadDb.Polyline), true);
                eo.AddAllowedClass(typeof(Polyline2d), true);
                eo.AddAllowedClass(typeof(Polyline3d), true);
                eo.AddAllowedClass(typeof(Ellipse), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
                eo.AddAllowedClass(typeof(Spline), true);
                eo.AddAllowedClass(typeof(Face), true);

                GV.propmptRes = GV.ed.GetEntity(eo);

                if (GV.propmptRes.Status != PromptStatus.OK)
                    return;

                GV.processStatus = true;
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
        public static void getvPortCoordinates()
        {
            try
            {
                #region Select a viewport
                //AcAp.SetSystemVariable("CVPORT", 0);


                // switch to paper space if a viewport is activated
                //AcAp.SetSystemVariable("CVPORT", 2);
                var view = (short)AcAp.GetSystemVariable("CVPORT");
                Point3d test = (Point3d)AcAp.GetSystemVariable("VIEWCTR");
                GV.ed.SwitchToModelSpace();

                if ((short)AcAp.GetSystemVariable("CVPORT") != 1)
                {
                    GV.processStatus = false;
                    GV.ed.SwitchToPaperSpace();
                }
                
                GV.processStatus = true;
                GH.writeLog("\n Running command : btn_selectViewport_Click");

                // select a viewport
                PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");

                eo.AllowNone = false;
                eo.SetRejectMessage("\nNeed to select a viewport!");
                eo.AddAllowedClass(typeof(CadDb.Viewport), true);
                //next lines are to allow for non-rectangular viewport selection
                eo.AddAllowedClass(typeof(Circle), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Polyline), true);
                eo.AddAllowedClass(typeof(Polyline2d), true);
                eo.AddAllowedClass(typeof(Polyline3d), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Ellipse), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
                eo.AddAllowedClass(typeof(Spline), true);
                eo.AddAllowedClass(typeof(Face), true);

                PromptEntityResult er = GV.ed.GetEntity(eo);
                #endregion

                if (er.Status != PromptStatus.OK)
                    return;
                else
                {
                    using (var tr = GV.Db.TransactionManager.StartTransaction())
                    {
                        DS.Entity newEnt = null;
                        CadDb.Viewport vpEnt = null;

                        #region Clasify Viewport shape
                        DS.Entity selEnt = (DS.Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                        if (selEnt.GetType() == typeof(CadDb.Viewport))
                        {
                            //Viewport is rectangular
                            vpEnt = (CadDb.Viewport)selEnt;
                            Extents3d vpExt = vpEnt.GeometricExtents;
                            double w = vpExt.MaxPoint.X - vpExt.MinPoint.X;
                            double h = vpExt.MaxPoint.Y - vpExt.MinPoint.Y;
                            DS.Polyline poly = new DS.Polyline(4);
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
                                vpEnt = (CadDb.Viewport)vpId.GetObject(OpenMode.ForRead);
                                newEnt = (DS.Entity)selEnt.Clone();
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
                        
                        #endregion

                        #region Coordinate Conversion
                        //Activate a vport, set CVPORT to the selected vport, and get VIEWCTR of vport
                        GV.ed.SwitchToModelSpace();
                        Matrix3d ucs = GV.ed.CurrentUserCoordinateSystem;
                        CoordinateSystem3d cs = ucs.CoordinateSystem3d;
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
                        #endregion

                        #region Get Model space Polygon
                        var polygon = new Point3dCollection();
                        DS.Polyline pl = (DS.Polyline)newEnt;
                        int vn = pl.NumberOfVertices;
                        for (int i = 0; i < vn; i++)
                        {
                            // Could also get the 3D point here
                            Point3d pt = pl.GetPoint3dAt(i);
                            polygon.Add(pt);
                            GV.ed.WriteMessage("\n" + pt.ToString());
                        }
                        pl.Closed = true;

                        if (GV.showViewportBoundary)
                        {
                            BlockTable bt = (BlockTable)GV.Db.BlockTableId.GetObject(OpenMode.ForRead);
                            BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                            ms.AppendEntity(newEnt);
                            tr.AddNewlyCreatedDBObject(newEnt, true);
                            tr.Commit();
                        }
                        else
                        {
                            tr.Dispose();
                        }

                        #endregion

                        SelectionFilter filter = new SelectionFilter(LocalCADHelper.selectionFilter(GV.labelFilterType));
                        PromptSelectionResult selection = GV.ed.SelectWindowPolygon(polygon, filter);
                        if (selection.Status != PromptStatus.OK)
                            return;

                        GV.selObjects_forProcessing = selection.Value.GetObjectIds();

                        //create the boudnary box of the viewport in the model space if turned on.
                        
                        //BlockTableRecord ms1 = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                        //ms1.AppendEntity(pl);
                        //tr.AddNewlyCreatedDBObject(pl, true);


                        GV.ed.SwitchToPaperSpace();
                        
                    }
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

        public static void getvPortCoordinatesALT()
        {
            try
            {
                GV.Doc.Window.Focus();

                #region Select a viewport
                // switch to paper space if a viewport is activated
                if ((short)AcAp.GetSystemVariable("CVPORT") != 1)
                {
                    GV.processStatus = false;
                    
                }

                GV.ed.SwitchToPaperSpace();
                GV.processStatus = true;
                GH.writeLog("\n Running command : btn_selectViewport_Click");

                // select a viewport
                PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");

                eo.AllowNone = false;
                eo.SetRejectMessage("\nNeed to select a viewport!");
                eo.AddAllowedClass(typeof(CadDb.Viewport), true);
                //next lines are to allow for non-rectangular viewport selection
                eo.AddAllowedClass(typeof(Circle), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Polyline), true);
                eo.AddAllowedClass(typeof(Polyline2d), true);
                eo.AddAllowedClass(typeof(Polyline3d), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Ellipse), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
                eo.AddAllowedClass(typeof(Spline), true);
                eo.AddAllowedClass(typeof(Face), true);

                PromptEntityResult er = GV.ed.GetEntity(eo);
                #endregion

                if (er.Status != PromptStatus.OK)
                    return;
                else
                {
                    using (var tr = GV.Db.TransactionManager.StartTransaction())
                    {
                        DS.Entity newEnt = null;
                        CadDb.Viewport vpEnt = null;

                        #region Clasify Viewport shape
                        DS.Entity selEnt = (DS.Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                        if (selEnt.GetType() == typeof(CadDb.Viewport))
                        {
                            //Viewport is rectangular
                            vpEnt = (CadDb.Viewport)selEnt;
                            Extents3d vpExt = vpEnt.GeometricExtents;
                            double minX = vpExt.MinPoint.X;
                            double minY = vpExt.MinPoint.Y;
                            double maxX = vpExt.MaxPoint.X;
                            double maxY = vpExt.MaxPoint.Y;
                            CadDb.Polyline poly = new CadDb.Polyline(4);
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
                                vpEnt = (CadDb.Viewport)vpId.GetObject(OpenMode.ForRead);
                                newEnt = (DS.Entity)selEnt.Clone();
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

                        #endregion

                        #region Coordinate Conversion
                        //Activate a vport, set CVPORT to the selected vport, and get VIEWCTR of vport
                        GV.ed.SwitchToModelSpace();
                        AcAp.SetSystemVariable("CVPORT", vpEnt.Number);
                        Matrix3d transMat = Helper.ViewportExtensions.PaperToModel(vpEnt);
                        newEnt.TransformBy(transMat);
                        #endregion

                        #region Get Model space Polygon
                        var polygon = new Point3dCollection();
                        DS.Polyline pl = (DS.Polyline)newEnt;
                        int vn = pl.NumberOfVertices;
                        for (int i = 0; i < vn; i++)
                        {
                            // Could also get the 3D point here
                            Point3d pt = pl.GetPoint3dAt(i);
                            polygon.Add(pt);
                            GV.ed.WriteMessage("\n" + pt.ToString());
                        }
                        pl.Closed = true;

                        if (GV.showViewportBoundary)
                        {
                            BlockTable bt = (BlockTable)GV.Db.BlockTableId.GetObject(OpenMode.ForRead);
                            BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                            ms.AppendEntity(newEnt);
                            tr.AddNewlyCreatedDBObject(newEnt, true);
                            tr.Commit();
                        }
                        else
                        {
                            tr.Dispose();
                        }

                        #endregion

                        SelectionFilter filter = new SelectionFilter(LocalCADHelper.selectionFilter(GV.labelFilterType));
                        PromptSelectionResult selection = GV.ed.SelectWindowPolygon(polygon, filter);
                        if (selection.Status != PromptStatus.OK)
                            return;

                        GV.selObjects_forProcessing = selection.Value.GetObjectIds();

                        //create the boudnary box of the viewport in the model space if turned on.

                        //BlockTableRecord ms1 = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                        //ms1.AppendEntity(pl);
                        //tr.AddNewlyCreatedDBObject(pl, true);


                        GV.ed.SwitchToPaperSpace();

                    }
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

        public static void getvPortCoordinatesADV()
        {
            try
            {
                GV.Doc.Window.Focus();

                #region Select a viewport
                // switch to paper space if a viewport is activated
                

                GV.ed.SwitchToPaperSpace();
                GV.processStatus = true;
                GH.writeLog("\n Running command : btn_selectViewport_Click");

                // select a viewport
                PromptEntityOptions eo = new PromptEntityOptions("\nSelect a viewport: ");

                eo.AllowNone = false;
                eo.SetRejectMessage("\nNeed to select a viewport!");
                eo.AddAllowedClass(typeof(CadDb.Viewport), true);
                //next lines are to allow for non-rectangular viewport selection
                eo.AddAllowedClass(typeof(Circle), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Polyline), true);
                eo.AddAllowedClass(typeof(Polyline2d), true);
                eo.AddAllowedClass(typeof(Polyline3d), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Ellipse), true);
                eo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Region), true);
                eo.AddAllowedClass(typeof(Spline), true);
                eo.AddAllowedClass(typeof(Face), true);

                PromptEntityResult er = GV.ed.GetEntity(eo);
                #endregion

                if (er.Status != PromptStatus.OK)
                    return;
                else
                {
                    using (var tr = GV.Db.TransactionManager.StartTransaction())
                    {
                        DS.Entity newEnt = null;
                        CadDb.Viewport vpEnt = null;

                        #region Clasify Viewport shape
                        DS.Entity selEnt = (DS.Entity)er.ObjectId.GetObject(OpenMode.ForRead);
                        if (selEnt.GetType() == typeof(CadDb.Viewport))
                        {
                            //Viewport is rectangular
                            vpEnt = (CadDb.Viewport)selEnt;
                            Extents3d vpExt = vpEnt.GeometricExtents;
                            double minX = vpExt.MinPoint.X;
                            double minY = vpExt.MinPoint.Y;
                            double w = vpExt.MaxPoint.X - vpExt.MinPoint.X;
                            double h = vpExt.MaxPoint.Y - vpExt.MinPoint.Y;
                            CadDb.Polyline poly = new CadDb.Polyline(4);
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
                                vpEnt = (CadDb.Viewport)vpId.GetObject(OpenMode.ForRead);
                                newEnt = (DS.Entity)selEnt.Clone();
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

                        #endregion

                        #region Coordinate Conversion
                        Matrix3d transMat = Helper.ViewportExtensions.PaperToModel(vpEnt);
                        newEnt.TransformBy(transMat);
                        CadDb.Polyline lwp = newEnt as CadDb.Polyline;

                        #endregion

                        #region Get Model space Polygon
                        if (lwp != null && lwp.NumberOfVertices > 2)
                        {
                            BlockTable bt = (BlockTable)GV.Db.BlockTableId.GetObject(OpenMode.ForRead);
                            BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);
                            //GV.viewportShpObj = ms.AppendEntity(newEnt);
                            ms.AppendEntity(newEnt);
                            tr.AddNewlyCreatedDBObject(newEnt, true);

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
                            Helper.LocalCADHelper.ZoomEntity(GV.ed, newEnt, margin);


                            SelectionFilter filter = new SelectionFilter(LocalCADHelper.selectionFilter(GV.labelFilterType));
                           
                            PromptSelectionResult selection = GV.ed.SelectWindowPolygon(p3dColl, filter);
                            if (selection.Status != PromptStatus.OK)
                            {
                                GV.ed.WriteMessage("\nCann't select any objects.");
                                GV.processStatus = false;
                                return;
                            }
                            else if (selection.Status == PromptStatus.Error)
                            {
                                // Switch to paperspace
                                LayoutManager.Current.CurrentLayout = cLayout;
                                GV.selObjects_forProcessing = null;
                                GV.processStatus = false;
                            }
                            else
                            {
                                // Switch to paperspace
                                LayoutManager.Current.CurrentLayout = cLayout;
                                GV.selObjects_forProcessing = selection.Value.GetObjectIds();
                                GV.ed.WriteMessage("\nSelected {0} objects.", GV.selObjects_forProcessing.Length);
                                GV.processStatus = true;
                            }

                            

                            //ed.SetImpliedSelection(selection.Value.GetObjectIds());
                        }

                        #endregion

                        
                        GV.ed.SwitchToPaperSpace();

                    }
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
        /// <summary>
        /// Gets the transformation matrix from the specified model space viewport Display Coordinate System (DCS)
        /// to the World Coordinate System (WCS).
        /// </summary>
        /// <param name="vp">The instance to which this method applies.</param>
        /// <returns>The DCS to WDCS transformation matrix.</returns>
        public static Matrix3d DCS2WCS(this CadDb.Viewport vp)
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
        public static Matrix3d WCS2DCS(this CadDb.Viewport vp)
        {
            return vp.DCS2WCS().Inverse();
        }

        /// <summary>
        /// Gets the transformation matrix from the specified paper space viewport Display Coordinate System (DCS)
        /// to the paper space Display Coordinate System (PSDCS).
        /// </summary>
        /// <param name="vp">The instance to which this method applies.</param>
        /// <returns>The DCS to PSDCS transformation matrix.</returns>
        public static Matrix3d DCS2PSDCS(this CadDb.Viewport vp)
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
        public static Matrix3d PSDCS2DCS(this CadDb.Viewport vp)
        {
            return vp.DCS2PSDCS().Inverse();
        }

        
    }

    public static class LabelTextExtractor
    {
        private static Dictionary<string, string> dictRes = new Dictionary<string, string>();
        private static string result;
        private static ObjectId objStyleId;
        public static string getLabelName(ObjectId objID)
        {
            try
            {
                using (Transaction transnew = GV.Db.TransactionManager.StartTransaction())
                {
                    GH.writeLog("\nstyle.trans");
                    GH.writeLog("\nstyle.objID: " + objID);


                    #region Label classification
                    switch (objID.ObjectClass.Name.ToString())
                    {
                        case "AeccDbPipeLabel":
                            {
                                //dispaly details about the selected object
                                PipeLabel pstyle = (PipeLabel)transnew.GetObject(objID, OpenMode.ForWrite);
                                result = pstyle.StyleName.ToString();
                                //Helper.LocalCADHelper.getTextComponentNames(objID);
                                //// + pstyle..ToString() + " | " + pstyle.DisplayName.ToString());
                                //LabelStyle component = objID.GetObject(OpenMode.ForWrite) as LabelStyle;
                                //GH.qprint("\nstyle.Info : " + pstyle.StyleName.ToString() + " | " + Helper.LabelTextExtractor.GetDisplayedLabelText(objID));// component.Properties.Label.ToString() + " |" + component..Text.Contents.Value.ToString() + " | " );
                                //GH.printDebug("", "", false, true);
                            }
                            break;
                        case "AeccDbNoteLabel":
                            {
                                //dispaly details about the selected object
                                NoteLabel pstyle = (NoteLabel)transnew.GetObject(objID, OpenMode.ForWrite);
                                result = pstyle.StyleName.ToString();
                                GH.qprint("\nstyle.Info : " + pstyle.StyleName.ToString() + " | " + Helper.LabelTextExtractor.GetDisplayedLabelText(objID));// + " | " + pstyle.Description.ToString() + " | " + pstyle.DisplayName.ToString());
                            }
                            break;
                        case "AeccDbGeneralLabel":
                            {
                                //dispaly details about the selected object
                                GeneralSegmentLabel pstyle = (GeneralSegmentLabel)transnew.GetObject(objID, OpenMode.ForWrite);
                                result = pstyle.StyleName.ToString();
                                GH.qprint("\nstyle.Info : " + pstyle.StyleName.ToString() + " | " + Helper.LabelTextExtractor.GetDisplayedLabelText(objID));// + " | " + pstyle.Description.ToString() + " | " + pstyle.DisplayName.ToString());
                            }
                            break;
                        case "AeccDbStructureLabel":
                            {
                                //dispaly details about the selected object
                                StructureLabel pstyle = (StructureLabel)transnew.GetObject(objID, OpenMode.ForWrite);
                                result = pstyle.StyleName.ToString();
                                GH.qprint("\nstyle.Info : " + pstyle.StyleName.ToString() + " | " + Helper.LabelTextExtractor.GetDisplayedLabelText(objID));// + " | " + pstyle.Description.ToString() + " | " + pstyle.DisplayName.ToString());
                            }
                            break;
                        case "AeccDbStaOffsetLabel":
                            {
                                //dispaly details about the selected object
                                StationOffsetLabel pstyle = (StationOffsetLabel)transnew.GetObject(objID, OpenMode.ForWrite);
                                result = pstyle.StyleName.ToString();
                                GH.qprint("\nstyle.Info : " + pstyle.StyleName.ToString() + " | " + Helper.LabelTextExtractor.GetDisplayedLabelText(objID));// + " | " + pstyle.Description.ToString() + " | " + pstyle.DisplayName.ToString());
                            }
                            break;
                    }
                    #endregion

                    transnew.Commit();
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
            return result;
        }

        public static ObjectId getLabelStytleID(ObjectId objID)
        {
            try
            {
                using (Transaction trans = GV.Db.TransactionManager.StartTransaction())
                {
                    GH.writeLog("\nstyle.trans");
                    GH.writeLog("\nstyle.objID: " + objID);


                    #region Label classification
                    switch (objID.ObjectClass.Name.ToString())
                    {
                        case "AeccDbPipeLabel":
                            {
                                //dispaly details about the selected object
                                PipeLabel pstyle = (PipeLabel)trans.GetObject(objID, OpenMode.ForWrite);
                                objStyleId = pstyle.StyleId;

                            }
                            break;
                        case "AeccDbNoteLabel":
                            {
                                //dispaly details about the selected object
                                NoteLabel pstyle = (NoteLabel)trans.GetObject(objID, OpenMode.ForWrite);
                                objStyleId = pstyle.StyleId;
                            }
                            break;
                        case "AeccDbGeneralLabel":
                            {
                                //dispaly details about the selected object
                                GeneralSegmentLabel pstyle = (GeneralSegmentLabel)trans.GetObject(objID, OpenMode.ForWrite);
                                objStyleId = pstyle.StyleId;
                            }
                            break;
                        case "AeccDbStructureLabel":
                            {
                                //dispaly details about the selected object
                                StructureLabel pstyle = (StructureLabel)trans.GetObject(objID, OpenMode.ForWrite);
                                objStyleId = pstyle.StyleId;
                            }
                            break;
                        case "AeccDbStaOffsetLabel":
                            {
                                //dispaly details about the selected object
                                StationOffsetLabel pstyle = (StationOffsetLabel)trans.GetObject(objID, OpenMode.ForWrite);
                                objStyleId = pstyle.StyleId;
                            }
                            break;
                    }
                    #endregion

                    trans.Dispose();
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
            return objStyleId;
        }
        private static Dictionary<string, string> CompNameVals = new Dictionary<string, string>();

        public static Dictionary<string, string> getLabelVals(ObjectId labelObject)
        {
            try
            {
                CompNameVals.Clear();
                using (Transaction tr = GV.Db.TransactionManager.StartTransaction())
                {
                    LabelStyle style = getLabelStytleID(labelObject).GetObject(OpenMode.ForRead) as LabelStyle;

                    //get label values
                    List<string> resVal = LabelTextExtractor.GetDisplayedLabelText(labelObject);

                    //components within the selected style
                    ObjectId[] objDrawOrder = style.GetComponentsDrawOrder();
                    int i = 0;

                    foreach (ObjectId objID in objDrawOrder)
                    {
                        LabelStyleComponent component = objID.GetObject(OpenMode.ForRead) as LabelStyleComponent;

                        //add components only if it is preset in the labelcomponenttypes - which is from settings
                        if (GV.labelcomponenttypes.Contains(component.GetType().Name))
                        {
                            if (GV.notenumberKey.Contains(component.Name))
                            {
                                if(resVal.Count > 0)
                                    CompNameVals.Add(component.Name, resVal[i]);// resVal[i]);
                            }
                            i++;
                        }


                    }

                    //check if they are in the correct order
                    GH.printDebug("+++", "", true, false);
                    foreach (var comp in CompNameVals)
                    {
                        GH.printDebug("COMP: ", comp.Key + " | > >" + comp.Value, false, false);
                    }
                    GH.printDebug("+++", "", false, true);
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
            return CompNameVals;
        }

        public static Dictionary<string, string> getLabelValsold(ObjectId labelObject)
        {
            try
            {
                using (Transaction tr = GV.Db.TransactionManager.StartTransaction())
                {

                    LabelStyle style = getLabelStytleID(labelObject).GetObject(OpenMode.ForRead)
                      as LabelStyle;

                    LabelStyle lblStylComp = labelObject.GetObject(OpenMode.ForWrite) as LabelStyle;
                    List<string> resVal = LabelTextExtractor.GetDisplayedLabelText(labelObject);
                    int i = 0;
                    List<string> AllCommponentNames = new List<string>();
                    ObjectIdCollection objColl = new ObjectIdCollection();
                    ObjectId[] objDrawOrder = style.GetComponentsDrawOrder();
                    foreach (ObjectId objID in objDrawOrder)
                    {
                        LabelStyleComponent component = objID.GetObject(OpenMode.ForRead) as LabelStyleComponent;
                        AllCommponentNames.Add(component.Name);
                    }
                    foreach (ObjectId id in style.GetComponents(LabelStyleComponentType.TextForEach))
                    {
                        LabelStyleComponent component = id.GetObject(OpenMode.ForRead) as LabelStyleComponent;
                        //AllCommponentNames.Add(component.Name);
                        objColl.Add(id);
                    }
                    foreach (ObjectId id in style.GetComponents(LabelStyleComponentType.ReferenceText))
                    {
                        LabelStyleComponent component = id.GetObject(OpenMode.ForRead) as LabelStyleComponent;
                        //AllCommponentNames.Add(component.Name);
                    }
                    foreach (ObjectId id in style.GetComponents(LabelStyleComponentType.Text))
                    {
                        LabelStyleComponent component = id.GetObject(OpenMode.ForRead) as LabelStyleComponent;
                        //AllCommponentNames.Add(component.Name);
                        objColl.Add(id);
                    }


                    foreach (ObjectId id in objColl)
                    {
                        LabelStyleComponent component = id.GetObject(OpenMode.ForRead) as LabelStyleComponent;

                        //check if the component name is note number component - if so it should be only numbers
                        if (GV.notenumberKey.Contains(component.Name))
                        {
                            //check which value contains only numbers and add that.
                            string correctNumonlyVal = GH.getnumberonlyValuefromList(resVal);
                            CompNameVals.Add(component.Name, correctNumonlyVal);
                        }
                        else
                        {
                            CompNameVals.Add(component.Name, resVal[i]);
                        }

                        i++;
                    }

                    foreach (var comp in CompNameVals)
                    {
                        GH.printDebug("COMP: ", comp.Key + " | > >" + comp.Value, false, false);
                    }

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
            return CompNameVals;
        }

        public static List<string> GetDisplayedLabelText(ObjectId labelId)
        {
            //if (labelId.ObjectClass.DxfName.ToUpper() != "AECC_GENERAL_SEGMENT_LABEL")
            //{
            //    throw new ArgumentException(
            //        "argument mismatch: not an \"AECC_GENERAL_SEGMENT_LABEL\"");
            //}
            List<string> resVal = new List<string>();
            StringBuilder lblText = new StringBuilder();

            using (var tran = labelId.Database.TransactionManager.StartTransaction())
            {
                var label = tran.GetObject(labelId, OpenMode.ForRead) as Label;
                if (label != null)
                {
                    bool changed = !label.Dragged && label.AllowsDragging;
                    try
                    {
                        if (changed)
                        {
                            label.UpgradeOpen();
                            double delta = label.StartPoint.DistanceTo(label.EndPoint);
                            label.LabelLocation =
                                new Point3d(label.LabelLocation.X +
                                    delta, label.LabelLocation.Y +
                                    delta, label.LabelLocation.Z);
                        }

                        var dbObjs = FullExplode(label);
                        GH.printDebug("COMP Value Count: ", dbObjs.Count().ToString(), true, true);
                        foreach (var obj in dbObjs)
                        {
                            if (obj.GetType() == typeof(DBText))
                            {
                                string val = (obj as DBText).TextString;
                                GH.printDebug("COMP Value Before Filter =>>: ", val, true, true);
                                if (!resVal.Contains(val))
                                {
                                    resVal.Add(val);
                                    //lblText.Append(" " + val);
                                    GH.printDebug("COMP Value: ", val, true, true);
                                }

                            }

                            obj.Dispose();
                        }
                    }
                    finally
                    {
                        if (changed) label.ResetLocation();
                    }
                }

                tran.Commit();
            }
            //resVal.Reverse();
            return resVal;
        }

        #region private methods
        private static List<CadDb.DBObject> FullExplode(CadDb.Entity ent)
        {
            // final result
            List<CadDb.DBObject> fullList = new List<CadDb.DBObject>();

            // explode the entity
            DBObjectCollection explodedObjects = new DBObjectCollection();
            ent.Explode(explodedObjects);
            foreach (CadDb.Entity explodedObj in explodedObjects)
            {
                // if the exploded entity is a blockref or mtext
                // then explode again
                if (explodedObj.GetType() == typeof(CadDb.BlockReference))
                {
                    fullList.AddRange(FullExplode(explodedObj));
                    object otype = explodedObj.GetType();

                }
                else if (explodedObj.GetType() == typeof(CadDb.MText))
                {
                    using (Transaction Tx = GV.Db.TransactionManager.StartTransaction())
                    {
                        object otype = explodedObj.GetType();
                        MText mtext = (MText)explodedObj;// Tx.GetObject(explodedObj.ObjectId, OpenMode.ForRead);
                        DBText dtTxt = new DBText();
                        dtTxt.TextString = mtext.Text.Replace("\r\n", " "); ;
                        GH.qprint("MText EXplode: " + mtext.Text);

                        fullList.Add(dtTxt);
                    }
                }
                else
                    fullList.Add(explodedObj);
            }
            return fullList;
        }

        #endregion
    }

    public static class MultiLeaderHelper
    {
        private static string strResult;
        public static string getMultiLeaderName(ObjectId objectID)
        {
            try
            {
                using (Transaction trans = GV.Db.TransactionManager.StartTransaction())
                {
                    CadDb.Entity ent = (CadDb.Entity)trans.GetObject(objectID, OpenMode.ForRead);
                    MLeader pl = (MLeader)ent;
                    MLeaderStyle currentStyle = trans.GetObject(pl.MLeaderStyle, OpenMode.ForWrite) as MLeaderStyle;
                    strResult = currentStyle.Name;
                    trans.Dispose();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                //GH.errorBox(ex.ToString());
            }
            catch (System.Exception ee)
            {
                //GH.errorBox(ee.ToString());
            }
            return strResult;
        }

        public static string getMultiLeaderAttVal(ObjectId objectID, string attName)
        {

            try
            {
                using (Transaction tr = GV.Db.TransactionManager.StartTransaction())
                {
                    CadDb.Entity ent = (CadDb.Entity)tr.GetObject(objectID, OpenMode.ForRead);
                    MLeader pl = (MLeader)ent;
                    ObjectId blkid = pl.BlockContentId;
                    BlockTableRecord mbtr = (BlockTableRecord)tr.GetObject(blkid, OpenMode.ForRead);

                    if (mbtr.HasAttributeDefinitions)
                    {
                        foreach (ObjectId atid in mbtr)
                        {
                            CadDb.DBObject obj = tr.GetObject(atid, OpenMode.ForRead, false) as CadDb.DBObject;
                            if (atid.ObjectClass.IsDerivedFrom(RXClass.GetClass(typeof(AttributeDefinition))))
                            {
                                AttributeDefinition attdef = (AttributeDefinition)obj as AttributeDefinition;

                                if (attdef.Tag == attName) // check your existing tag
                                {
                                    AttributeReference attref = pl.GetBlockAttribute(attdef.ObjectId);
                                    MLeaderStyle currentStyle = tr.GetObject(pl.MLeaderStyle, OpenMode.ForWrite) as MLeaderStyle;
                                    //GV.ed.WriteMessage("\n ATT 2 = " + attref.Tag + "\n    Attribute String: " + attref.TextString + "|" + currentStyle.Name);
                                    strResult = attref.TextString;
                                }

                            }
                        }
                    }

                    tr.Dispose();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                //GH.errorBox(ex.ToString());
            }
            catch (System.Exception ee)
            {
                //GH.errorBox(ee.ToString());
            }
            return strResult;
        }

        public static Dictionary<string, string> getMultiLeaderAttValColl(ObjectId objectID, string attName)
        {
            Dictionary<string, string> dictRes = new Dictionary<string, string>();
            try
            {
                using (Transaction tr = GV.Db.TransactionManager.StartTransaction())
                {
                    CadDb.Entity ent = (CadDb.Entity)tr.GetObject(objectID, OpenMode.ForRead);
                    MLeader pl = (MLeader)ent;
                    ObjectId blkid = pl.BlockContentId;
                    BlockTableRecord mbtr = (BlockTableRecord)tr.GetObject(blkid, OpenMode.ForRead);

                    if (mbtr.HasAttributeDefinitions)
                    {
                        foreach (ObjectId atid in mbtr)
                        {
                            CadDb.DBObject obj = tr.GetObject(atid, OpenMode.ForRead, false) as CadDb.DBObject;
                            if (atid.ObjectClass.IsDerivedFrom(RXClass.GetClass(typeof(AttributeDefinition))))
                            {
                                AttributeDefinition attdef = (AttributeDefinition)obj as AttributeDefinition;

                                AttributeReference attref = pl.GetBlockAttribute(attdef.ObjectId);
                                MLeaderStyle currentStyle = tr.GetObject(pl.MLeaderStyle, OpenMode.ForWrite) as MLeaderStyle;
                                //GV.ed.WriteMessage("\n ATT 2 = " + attref.Tag + "\n    Attribute String: " + attref.TextString + "|" + currentStyle.Name);
                                dictRes.Add(attref.Tag, attref.TextString);

                            }
                        }
                    }

                    tr.Dispose();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                //GH.errorBox(ex.ToString());
            }
            catch (System.Exception ee)
            {
                //GH.errorBox(ee.ToString());
            }
            return dictRes;
        }
    }
    public class TextPlacementJig : EntityJig
    {
        // Declare some internal state

        Database _db;
        Transaction _tr;
        Point3d _position;
        double _angle, _txtSize;
        bool _toggleBold, _toggleItalic;
        private TextHorizontalMode _align;
        // Constructor

        public TextPlacementJig(Transaction tr, Database db, CadDb.Entity ent) : base(ent)
        {
            _db = db;
            _tr = tr;
            _angle = 0;
            _txtSize = GV.textHeight;
            
        }

        protected override SamplerStatus Sampler(JigPrompts jp)
        {
            // We acquire a point but with keywords

            JigPromptPointOptions po = new JigPromptPointOptions("\nPosition of text");

            po.UserInputControls =
              (UserInputControls.Accept3dCoordinates |
                UserInputControls.NullResponseAccepted |
                UserInputControls.NoNegativeResponseAccepted |
                UserInputControls.GovernedByOrthoMode);

            po.SetMessageAndKeywords(
              "\nSpecify position of text or " +
              "[Bold/Italic/LArger/Smaller/" +
               "ROtate90/LEft/Middle/RIght]: ",
              "Bold Italic LArger Smaller " +
              "ROtate90 LEft Middle RIght"
            );

            PromptPointResult ppr = jp.AcquirePoint(po);

            if (ppr.Status == PromptStatus.Keyword)
            {
                switch (ppr.StringResult)
                {
                    case "Bold":
                        {
                            // TODO
                            _toggleBold = true;
                            break;
                        }
                    case "Italic":
                        {
                            // TODO
                            _toggleItalic = true;
                            break;
                        }
                    case "LArger":
                        {
                            // Multiple the text size by two

                            _txtSize *= 2;
                            break;
                        }
                    case "Smaller":
                        {
                            // Divide the text size by two

                            _txtSize /= 2;
                            break;
                        }
                    case "ROtate90":
                        {
                            // To rotate clockwise we subtract 90 degrees and
                            // then normalise the angle between 0 and 360

                            _angle -= Math.PI / 2;
                            while (_angle < Math.PI * 2)
                            {
                                _angle += Math.PI * 2;
                            }
                            break;
                        }
                    case "LEft":
                        {
                            // TODO
                            _align = TextHorizontalMode.TextLeft;
                            break;
                        }
                    case "RIght":
                        {
                            // TODO
                            _align = TextHorizontalMode.TextRight;
                            break;
                        }
                    case "Middle":
                        {
                            // TODO
                            _align = TextHorizontalMode.TextMid;
                            break;
                        }
                }

                return SamplerStatus.OK;
            }
            else if (ppr.Status == PromptStatus.OK)
            {
                // Check if it has changed or not (reduces flicker)

                if (
                  _position.DistanceTo(ppr.Value) <
                    Tolerance.Global.EqualPoint
                )
                    return SamplerStatus.NoChange;

                _position = ppr.Value;
                return SamplerStatus.OK;
            }

            return SamplerStatus.Cancel;
        }

        protected override bool Update()
        {
            // Set properties on our text object


            MText txt = (MText)Entity;

            txt.Location = _position;
            txt.TextHeight = _txtSize;
            txt.Rotation = _angle;
            
            // Set the bold and/or italic properties on the style
            if (_toggleBold || _toggleItalic)
            {

                TextStyleTable tab =
                  (TextStyleTable)_tr.GetObject(
                    _db.TextStyleTableId, OpenMode.ForRead
                  );

                TextStyleTableRecord style =
                  (TextStyleTableRecord)_tr.GetObject(
                    txt.TextStyleId, OpenMode.ForRead
                  );

                // A bit convoluted, but this check will tell us
                // whether the new style is bold/italic

                bool bold = !(style.Font.Bold == _toggleBold);
                bool italic = !(style.Font.Italic == _toggleItalic);
                _toggleBold = false;
                _toggleItalic = false;

                // Get the new style name based on the old name and
                // a suffix ("_BOLD", "_ITALIC" or "_BOLDITALIC")

                var oldName = style.Name.Split(new[] { '_' });
                string newName =
                  oldName[0] +
                  (bold || italic ? "_" +
                    (bold ? "BOLD" : "") +
                    (italic ? "ITALIC" : "")
                    : "");

                // We only create a duplicate style if one doesn't
                // already exist

                if (tab.Has(newName))
                {
                    txt.TextStyleId = tab[newName];
                }
                else
                {
                    // We have to create a new style - clone the old one

                    TextStyleTableRecord newStyle =
                      (TextStyleTableRecord)style.Clone();

                    // Set a new name to avoid duplicate keys

                    newStyle.Name = newName;

                    // Create a new font based on the old one, but with
                    // our values for bold & italic

                    FontDescriptor oldFont = style.Font;
                    FontDescriptor newFont =
                      new FontDescriptor(
                        oldFont.TypeFace, bold, italic,
                        oldFont.CharacterSet, oldFont.PitchAndFamily
                      );

                    // Set it on the style

                    newStyle.Font = newFont;

                    // Add the new style to the text style table and
                    // the transaction

                    tab.UpgradeOpen();
                    ObjectId styleId = tab.Add(newStyle);
                    _tr.AddNewlyCreatedDBObject(newStyle, true);

                    // And finally set the new style on our text object

                    txt.TextStyleId = styleId;
                }
            }

            return true;
        }
    }

}

