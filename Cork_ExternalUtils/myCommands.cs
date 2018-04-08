// (C) Copyright 2018 by  
//

using System.Diagnostics;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using Cork_ExternalUtils;

// This line is not mandatory, but improves loading performances

[assembly: CommandClass(typeof (MyCommands))]

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
                    acTypValAr.SetValue(new TypedValue((int) DxfCode.Start, "3DSOLID"), 0);

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
                        var layTab = (LayerTable) acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead);

                        // Cycle through objects
                        foreach (SelectedObject obj in acSSet)
                        {
                            // Open each entity for write
                            var en = (Entity) acTrans.GetObject(obj.ObjectId, OpenMode.ForWrite);
                            var sol3D = en as Solid3d;

                            // Check if material exists in layer dictionary
                            foreach (var mat in matDict)
                            {
                                // If material does not match a stock layer - exit
                                if (sol3D?.Material != mat.Name) continue;

                                // If material does match - check adn create it in drawing
                                if (!layTab.Has(mat.Name))
                                {
                                    var layTabRec = new LayerTableRecord
                                    {
                                        Name = mat.Name,
                                        Color = Color.FromColorIndex(ColorMethod.ByAci, mat.Color)
                                    };
                                    layTab.UpgradeOpen();

                                    layTab.Add(layTabRec);
                                    acTrans.AddNewlyCreatedDBObject(layTabRec, true);
                                }

                                // Set object layer to material name
                                if (sol3D != null) sol3D.Layer = mat.Name;
                            }
                        }

                        acCurEd.WriteMessage("\nAll solids that match a standard layer have been converted.\n");
                    }
                    else
                    {
                        acCurEd.WriteMessage("\nNo Solid bodies exist in drawing. Are they in blocks?\n");
                    }

                    acTrans.Commit();
                }

                catch (Exception ex)
                {
                    Application.ShowAlertDialog(ex.Message);
                }
            }
        }

        // Material Converter
        [CommandMethod("ConvertMaterialsV2", CommandFlags.Modal |
                                           CommandFlags.NoPaperSpace)]
        public void CmdConvertMaterial2()
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

                        // Get the layer table from the drawing
                        var layTab = (LayerTable)acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead);

                        // Cycle through objects
                        foreach (SelectedObject obj in acSSet)
                        {
                           
                            // Open each entity for write
                            var en = (Entity) acTrans.GetObject(obj.ObjectId, OpenMode.ForWrite);
                            var sol3D = en as Solid3d;

                            Debug.Assert(sol3D != null, "sol3D != null");

                            var solMat =
                                (Autodesk.AutoCAD.DatabaseServices.Material)
                                    acTrans.GetObject(sol3D.MaterialId, OpenMode.ForRead);

                            var matName = solMat.Name;
                            var matColor = solMat.Diffuse.Color.Color;
                                                    
                            if (!layTab.Has(matName))
                            {                         
                                var layTabRec = new LayerTableRecord
                                {
                                    Name = matName,
                                    Color = Color.FromEntityColor(matColor)
                                };

                                layTab.UpgradeOpen();

                                layTab.Add(layTabRec);
                                acTrans.AddNewlyCreatedDBObject(layTabRec, true);
                            }

                            // Set object layer to material name
                            sol3D.Layer = matName;
                        }

                        acCurEd.WriteMessage("\nAll solids have been converted.\n");
                    }
                    else
                    {
                        acCurEd.WriteMessage("\nNo Solid bodies exist in drawing. Are they in blocks?\n");
                    }

                    acTrans.Commit();
                }

                catch (Exception ex)
                {
                    Application.ShowAlertDialog(ex.Message);
                }
            }
        }
    }
}
