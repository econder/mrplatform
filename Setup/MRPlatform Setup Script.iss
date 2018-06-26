; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "MRPlatform"
#define MyAppVersion GetFileVersion("C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\MRPlatform.dll")
#define MyAppPublisher "MR Systems, Inc"
#define MyAppURL "https://www.mrsystems.com/"
#define MyCopyright "� MR Systems 2018"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{6E522AB6-FE71-4322-891F-CE20398995F5}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppCopyright={#MyCopyright}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\MR Systems\MRPlatform
DisableDirPage=yes
DefaultGroupName=MR Systems
OutputDir=C:\Users\mrsystems\Documents\VS Projects\MRPlatform\Setup
OutputBaseFilename=MRPlatform
Compression=lzma
SolidCompression=yes
VersionInfoVersion=2.2.2
VersionInfoCompany=MR Systems
VersionInfoCopyright=Copyright � 2018 MR Systems Inc
VersionInfoProductName=MRPlatform
DisableProgramGroupPage=auto

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
; Source: "C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\Microsoft.Synchronization.Data.dll"; DestDir: "{app}"; Flags: ignoreversion
; Source: "C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\Microsoft.Synchronization.Data.SqlServer.dll"; DestDir: "{app}"; Flags: ignoreversion
; Source: "C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\Microsoft.Synchronization.Data.SqlServer.xml"; DestDir: "{app}"; Flags: ignoreversion
; Source: "C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\Microsoft.Synchronization.Data.xml"; DestDir: "{app}"; Flags: ignoreversion
; Source: "C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\Microsoft.Synchronization.dll"; DestDir: "{app}"; Flags: ignoreversion
; Source: "C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\Microsoft.Synchronization.xml"; DestDir: "{app}"; Flags: ignoreversion
; Source: "C:\Users\mrsystems\Documents\VS Projects\MRPlatform\MRPlatform\bin\x86\Release\MRDbSync.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\MRPlatform\bin\x86\Release\MRPlatform.dll"; DestDir: "{pf}\MR Systems\MRPlatform\"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "..\MRPlatform\bin\x86\Release\Microsoft.VisualBasic.tlb"; DestDir: "{sys}"; Flags: 32bit regtypelib regserver; MinVersion: 0,6.1; Permissions: admins-modify
Source: "..\MRPlatform\bin\x86\Release\MRPlatform.tlb"; DestDir: "{sys}"; Flags: 32bit regtypelib regserver; MinVersion: 0,6.1; Permissions: admins-modify

[Run]
Filename: "{dotnet4032}\RegAsm.exe"; Parameters: "/tlb /unregister Microsoft.VisualBasic.dll"; WorkingDir: "{app}"; Flags: runminimized skipifdoesntexist; StatusMsg: "Unregistering Old Controls..."
Filename: "{dotnet4032}\RegAsm.exe"; Parameters: "/tlb /unregister MRPlatform.dll"; WorkingDir: "{app}"; Flags: runminimized skipifdoesntexist; StatusMsg: "Unregistering Old Controls..."
Filename: "{dotnet4032}\RegAsm.exe"; Parameters: "/codebase /tlb:Microsoft.VisualBasic.tlb Microsoft.VisualBasic.dll"; WorkingDir: "{app}"; Flags: runminimized; StatusMsg: "Registering New Controls..."
Filename: "{dotnet4032}\RegAsm.exe"; Parameters: "/codebase /tlb:MRPlatform.tlb MRPlatform.dll"; WorkingDir: "{app}"; Flags: runminimized; StatusMsg: "Registering New Controls..."

[UninstallRun]
Filename: "{dotnet4032}\RegAsm.exe"; Parameters: "/tlb /unregister Microsoft.VisualBasic.dll"; WorkingDir: "{app}"; Flags: runminimized skipifdoesntexist; StatusMsg: "Unregistering Old Controls..."
Filename: "{dotnet4032}\RegAsm.exe"; Parameters: "/tlb /unregister MRPlatform.dll"; WorkingDir: "{app}"; Flags: runminimized skipifdoesntexist; StatusMsg: "Unregistering Old Controls..."
