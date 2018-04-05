// (C) Copyright 2018 by Zach Ayers
// CORK plugin for AutoCAD
// Email: zachayers@users.noreply.github.com

using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Cork_3dUtils;

// This line is not mandatory, but improves loading performances

[assembly: CommandClass(typeof (MyCommands))]

namespace Cork_3dUtils
{
    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // Floor Command
        // Prompts user to select objects, then select a point to align with current Z Plane
        [CommandMethod("Floor", CommandFlags.Modal)]
        public static void CmdFloorMany() // This method can have any name
        {
            // Initialize Document
            var acDoc = Application.DocumentManager.MdiActiveDocument;
            var acCurDb = acDoc.Database;
            var acCurEd = acDoc.Editor;

            // Open a Transaction
            // Start a transaction
            using (var acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Set selection options for user
                var selOpt = new PromptSelectionOptions
                {
                    MessageForAdding = "\nSelect objects to move to Z Plane: ",
                    MessageForRemoval = "\nSelect objects to remove from selection: ",
                    RejectPaperspaceViewport = true,
                    RejectObjectsOnLockedLayers = true,
                    RejectObjectsFromNonCurrentSpace = true
                };

                // Get object selection from user
                var selRes = acCurEd.GetSelection(selOpt);

                // Exit if selection error
                if (selRes.Status != PromptStatus.OK) return;

                // Set selection set to user selection
                var acSSet = selRes.Value;

                // Get point from user to floor
                var ptOpt = new PromptPointOptions("Select Vertex to align with Z Plane: ") {AllowNone = false};
                var ptRes = acCurEd.GetPoint(ptOpt);

                // Exit if selection error
                if (ptRes.Status != PromptStatus.OK) return;

                // Create  matrix to move the objects from the current Z value to a '0' Z value
                var acPt3D = ptRes.Value;
                var floorPt3D = new Point3d(acPt3D.X, acPt3D.Y, 0);
                var acVec3D = acPt3D.GetVectorTo(floorPt3D);
                var transVec3D = acVec3D.TransformBy(acCurEd.CurrentUserCoordinateSystem);

                // Floor selected objects
                foreach (SelectedObject acSsObj in acSSet)
                {
                    // Check if a valid SelectedObject was returned
                    if (acSsObj != null)
                    {
                        // Open the selected object for write
                        var acEnt = acTrans.GetObject(acSsObj.ObjectId, OpenMode.ForWrite) as Entity;

                        acEnt?.TransformBy(Matrix3d.Displacement(transVec3D));
                    }
                }

                acCurEd.WriteMessage("\nSelection has been aligned with Z Plane.");

                // Dispose the transaction
                acTrans.Commit();
            }
        }
    }
}
