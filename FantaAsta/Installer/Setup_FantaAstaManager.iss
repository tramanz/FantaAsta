[Setup]
#define ApplicationVersion GetFileVersion('Skeleton\FantaLegaManager.exe')

AppCopyright=Marco Tramarin © 2020
AppName=Fanta Lega Manager
AppVerName=Fanta Lega Manager
AppVersion={#ApplicationVersion}
WizardImageStretch=false
WindowResizable=false
WindowVisible=true
BackColor2=clWhite
SetupIconFile=FantaLegaManager.ico
RestartIfNeededByRun=false
DefaultDirName=C:\Program Files (x86)\FantaLegaManager
AllowRootDirectory=true
DirExistsWarning=yes
AllowUNCPath=false
OutputDir=.
OutputBaseFilename=FantaLegaManager_{#ApplicationVersion}_setup
SetupLogging=true
AppID={{4CF56F15-BA25-4179-BC78-F15231399B3A}
SolidCompression=true
ShowLanguageDialog=no
LanguageDetectionMethod=locale
DefaultGroupName=Marco Tramarin
VersionInfoCompany=Marco Tramarin
AppPublisher=Marco Tramarin
UninstallDisplayIcon={app}\FantaLegaManager.exe

[Files]
Source: Skeleton\FantaLegaManager.exe; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\FantaLegaManager.exe.config; DestDir: {app}; Flags: ignoreversion onlyifdoesntexist
Source: Skeleton\CommonServiceLocator.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\Prism.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\Prism.Unity.Wpf.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\Prism.Wpf.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\System.Runtime.CompilerServices.Unsafe.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\System.Threading.Tasks.Extensions.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\System.ValueTuple.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\System.Windows.Interactivity.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\Unity.Abstractions.dll; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\Unity.Container.dll; DestDir: {app}; Flags: ignoreversion

[UninstallDelete]
Type: filesandordirs; Name: "C:\ProgramData\FantaLegaManager"
    
[Icons]
Name: "{commondesktop}\Fanta Lega Manager"; Filename: {app}\FantaLegaManager.exe;
