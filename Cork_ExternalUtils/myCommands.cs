// (C) Copyright 2018 by  
//
using System;
using System.Diagnostics;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(Cork_ExternalUtils.MyCommands))]

namespace Cork_ExternalUtils
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // Material Converter
        [CommandMethod("ConvertMaterials", CommandFlags.Modal |
                                           CommandFlags.NoPaperSpace)]

        public void CmdConvertMaterial()
        {
            // Initialize Document
            var acDoc = Application.DocumentManager.MdiActiveDocument;
            var acCurDb = acDoc.Database;
            var acCurEd = acDoc.Editor;

            // Open a Transaction
            using (var acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                try
                {
                    // Create a TypedValue array to define the filter criteria
                    var acTypValAr = new TypedValue[1];
                    acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "3DSOLID"), 0);

                    // Assign the filter criteria to a SelectionFilter object
                    var acSelFtr = new SelectionFilter(acTypValAr);

                    // Request for objects to be selected in the drawing area
                    var acSsPrompt = acCurEd.SelectAll(acSelFtr);

                    // If the prompt status is OK, objects were selected
                    if (acSsPrompt.Status == PromptStatus.OK)
                    {
                        var acSSet = acSsPrompt.Value;

                        acCurEd.WriteMessage("\nNumber of Solids to be converted: " + acSSet.Count);

                        // Get Layer List For Converting
                        var layDict = new CorkLayerDictionary();
                        var matDict = layDict.InitLayers();

                        // Get the layer table from the drawing
                        var layTab = (LayerTable)acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead);

                        foreach (SelectedObject obj in acSSet)
                        {
                            var en = (Entity)acTrans.GetObject(obj.ObjectId, OpenMode.ForWrite);
                            var sol3D = en as Solid3d;

                            foreach (var mat in matDict)
                            {
                                if (sol3D?.Material != mat.Name) continue;
                                if (!layTab.Has(mat.Name))
                                {
                                    var layTabRec = new LayerTableRecord();
                                    layTabRec.Name = mat.Name;
                                    layTabRec.Color = Color.FromColorIndex(ColorMethod.ByAci, mat.Color);
                                    layTab.UpgradeOpen();                             
                                }

                                if (sol3D != null) sol3D.Layer = mat.Name;
                            }

                        }

                        acCurEd.WriteMessage("\nAll Solids that match a standard layer have been converted.\n");
                    }
                    else
                    {
                        acCurEd.WriteMessage("\nNo Solid bodies exist in drawing. Are they in blocks?\n");
                    }

                    acTrans.Commit();
                }

                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog(ex.Message);
                }
            }
        }
    }
}
