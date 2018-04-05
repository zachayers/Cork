// (C) Copyright 2018 by Zach Ayers
// CORK plugin for AutoCAD
// Email: zachayers@users.noreply.github.com

using Autodesk.AutoCAD.Runtime;
using Cork;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof (MyCommands))]

namespace Cork
{
    public class MyCommands
    {
    }
}
