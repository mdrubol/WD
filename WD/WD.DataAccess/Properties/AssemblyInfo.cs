// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-13-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="AssemblyInfo.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary>
// As Web developers, our lives revolve around working with data. We create databases to store the data, code to retrieve and modify it, and web pages to collect and summarize it. There are some cases were we need to switch between different databases and changes application code to make code working with the database connectors.
// Sometime that task is too cumbersome as it increases load on developers for writing more line of codes for different database connectors.To avoid such scenarios and to make all applications loosely coupled WD came up with an idea of having one core repository (DataAccess Layer (DAL) which can communicate with any kind of database (Right now we support SQL,Oracle,Db2 and Tera data).
// Using DAL developers only need to create Business logic for their application and use DAL for database communication.
// Following tutorials will give an overlook about DAL and we further discuss different scenarios and implementation of DAL in our client applications.
// </summary>
// ***********************************************************************

using System.Reflection;
using System.Runtime.InteropServices;



// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WD.DataAccess")]
[assembly: AssemblyDescription("Data Access Layer Handling multiple databases")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Western Digital")]
[assembly: AssemblyProduct("WD.DataAccess")]
[assembly: AssemblyCopyright("Copyright © Western Digital 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Connect ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("27f846b2-65ba-4785-a313-43681dd406af")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.5")]
[assembly: AssemblyFileVersion("1.0.0.5")]
//Logger
[assembly: log4net.Config.XmlConfigurator(Watch = false)]

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(@"WD.DataAccess,PublicKey=
002400000480000094000000060200000024000052534131000400000100010013ef037b8f3d01
857a5b059c6f8628844974880867ecefebd06c5fc0e9d70ae7dade28052a863a944a1f39ec892a
4e9af189bfc2ab46a91ba69d669d296a9b8206c920733e7f789961bcd7d1cc3d3c6c450ccdc596
558774c72de73acc4d96221b7bed1f259cdb01ca6d63734b8d04ed750e6a930628634607d4baff
aed295b9")]


