[Setup]
#define ApplicationVersion GetFileVersion('Skeleton\FantaAstaManager.exe')

AppCopyright=Marco Tramarin © 2020
AppName=FantaAsta Manager
AppVerName=FantaAsta Manager
AppVersion={#ApplicationVersion}
WizardImageStretch=false
WindowResizable=false
WindowVisible=true
BackColor2=clWhite
SetupIconFile=..\Resources\FantaAstaManager.ico
RestartIfNeededByRun=false
DefaultDirName=C:\Program Files (x86)\FantaAstaManager
AllowRootDirectory=true
DirExistsWarning=yes
AllowUNCPath=false
OutputDir=.
OutputBaseFilename=FantaAsta Manager {#ApplicationVersion} setup
SetupLogging=true
AppID={{4CF56F15-BA25-4179-BC78-F15231399B3A}
SolidCompression=true
ShowLanguageDialog=no
LanguageDetectionMethod=locale
DefaultGroupName=Marco Tramarin
VersionInfoCompany=Marco Tramarin
AppPublisher=Marco Tramarin
UninstallDisplayIcon={app}\FantaAstaManager.exe

[Files]
Source: Skeleton\FantaAstaManager.exe; DestDir: {app}; Flags: ignoreversion
Source: Skeleton\FantaAstaManager.exe.config; DestDir: {app}; Flags: ignoreversion onlyifdoesntexist
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
Type: filesandordirs; Name: "C:\ProgramData\FantaAstaManager"
    
[Icons]
Name: "{commondesktop}\FantaAsta Manager"; Filename: {app}\FantaAstaManager.exe;
