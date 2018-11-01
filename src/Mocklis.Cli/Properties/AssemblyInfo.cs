// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives

using System.Reflection;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Mocklis.Cli")]
[assembly:
    AssemblyDescription(
        "Mocklis is a library and source code generator for .net, targeted at generating test doubles from interfaces. Mocklis.Cli is a command line interface for the code generator.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Mocklis")]
[assembly: AssemblyCopyright("Copyright © 2018 Esbjörn Redmo and contributors")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: AssemblyVersion("0.1.0.0")]
[assembly: AssemblyFileVersion("0.1.0.0")]
[assembly: AssemblyInformationalVersion("0.1.0-alpha")]
