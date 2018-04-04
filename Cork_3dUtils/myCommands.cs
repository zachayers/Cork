// (C) Copyright 2018 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(Cork_3dUtils.MyCommands))]

namespace Cork_3dUtils
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Modal Command with localized name
        [CommandMethod("FloorMany", CommandFlags.Modal)]
        public static void CmdFloorMany() // This method can have any name
        {
            // Initialize Document
            var acDoc = Application.DocumentManager.MdiActiveDocument;
            var acCurDb = acDoc.Database;
            var acCurEd = acDoc.Editor;

            // Check if current document exists
            if (acDoc == null) return;

            // Open a Transaction
            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Set selection options for user
                var selOpt = new PromptSelectionOptions();
                selOpt.MessageForAdding = "\nSelect objects to move to Z Plane: ";
                selOpt.MessageForRemoval = "\nSelect objects to remove from selection: ";
                selOpt.RejectPaperspaceViewport = true;
                selOpt.RejectObjectsOnLockedLayers = true;
                selOpt.RejectObjectsFromNonCurrentSpace = true;

                // Get object selection from user
                var selRes = acCurEd.GetSelection(selOpt);

                // Exit if selection error
                if (selRes.Status != PromptStatus.OK) return;

                // Set selection set to user selection
                var acSSet = selRes.Value;

                // Get point from user to floor
                var ptOpt = new PromptPointOptions("Select point to floor: ");
                ptOpt.AllowNone = true;
                var ptRes = acCurEd.GetPoint(ptOpt);

                // Exit if selection error
                if (ptRes.Status != PromptStatus.OK) return;

                // Create  matrix to move the objects from the current Z value to a '0' Z value
                var acPt3d = ptRes.Value;
                var floorPt3D = new Point3d(acPt3d.X, acPt3d.Y, 0);
                var acVec3d = acPt3d.GetVectorTo(floorPt3D);
                var transVec3D = acVec3d.TransformBy(acCurEd.CurrentUserCoordinateSystem);

                acCurEd.WriteMessage($"\nSelected Point: {acPt3d.X}, {acPt3d.Y}, {acPt3d.Z}");
                acCurEd.WriteMessage($"\nFloor Point: {floorPt3D.X}, {floorPt3D.Y}, {floorPt3D.Z}");
                acCurEd.WriteMessage($"\nDisplacement Vector: {acVec3d.X}, {acVec3d.Y}, {acVec3d.Z}");
                acCurEd.WriteMessage($"\nTransformed Vector: {transVec3D.X}, {transVec3D.Y}, {transVec3D.Z}");

                // Floor selected objects
                foreach (SelectedObject acSSObj in acSSet)
                {
                    // Check if a valid SelectedObject was returned
                    if (acSSObj != null)
                    {                       
                        // Open the selected object for write
                        var acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Entity;

                        if (acEnt != null)
                        {                          
                            acEnt.TransformBy(Matrix3d.Displacement(transVec3D));
                        }
                    }
                }

                acCurEd.WriteMessage("\nSelection has been floored");

                // Dispose the transaction
                acTrans.Commit();
            }

        }          

    }

}
