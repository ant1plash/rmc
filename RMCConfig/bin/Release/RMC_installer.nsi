;NSIS Modern User Interface
;Start Menu Folder Selection Example Script
;Written by Joost Verburg

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  !define MUI_ICON "rmc.ico"
  !define MUI_UNICON "rmc.ico"
;--------------------------------
;General

  ;Name and file
  Name "Remote Media Control"
  OutFile "RemoteMediaControl_latest.exe"

  ;Default installation folder
  InstallDir "$LOCALAPPDATA\Remote Media Control"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\Remote Media Control" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin


;--------------------------------
;Variables

  Var StartMenuFolder

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_LICENSE "License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  
  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\Remote Media Control" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
  
  !insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
  
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

  
  !define MUI_FINISHPAGE_RUN
  !define MUI_FINISHPAGE_RUN_TEXT "Start Remote Media Control Server"
  !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
  !insertmacro MUI_PAGE_FINISH

;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section "Remote Media Control" SecRMC

  SetOutPath "$INSTDIR"
  
  File "Gma.QrCodeNet.Encoding.dll"
  File "Gma.QrCodeNet.Encoding.pdb"
  File "Gma.QrCodeNet.Encoding.xml"
  File "ICSharpCode.SharpZipLib.dll"
  File "RemoteMediaControlServer.application"
  File "RemoteMediaControlServer.exe"
  File "RemoteMediaControlServer.exe.config"
  File "RemoteMediaControlServer.exe.manifest"
  File "RemoteMediaControlServer.pdb"
  File "RemoteMediaControlUpdater.exe"
  File "RemoteMediaControlUpdater.pdb"
  ;Store installation folder
  WriteRegStr HKCU "Software\Remote Media Control" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
	CreateShortCut "$DESKTOP\Remote Media Control.lnk"  "$INSTDIR\RemoteMediaControlServer.exe" ""
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd


Section "Autorun with Windows (pretty quiet)" SecAutostart
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "RemoteMediaControl" "$INSTDIR\RemoteMediaControlServer.exe"

 ; !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
 ; !insertmacro MUI_STARTMENU_WRITE_END


SectionEnd

;Remote Media Control
;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecRMC ${LANG_ENGLISH} "Remote Media Control server itself. This is required."
  LangString DESC_SecAutostart ${LANG_ENGLISH} "Will autostart RMC with windows. This is optional."

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecRMC} $(DESC_SecRMC)
	!insertmacro MUI_DESCRIPTION_TEXT ${SecAutostart} $(DESC_SecAutostart)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END
 
;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...
  Delete "$INSTDIR\Gma.QrCodeNet.Encoding.dll"
  Delete "$INSTDIR\Gma.QrCodeNet.Encoding.pdb"
  Delete "$INSTDIR\Gma.QrCodeNet.Encoding.xml"
  Delete "$INSTDIR\ICSharpCode.SharpZipLib.dll"
  Delete "$INSTDIR\RemoteMediaControlServer.application"
  Delete "$INSTDIR\RemoteMediaControlServer.exe"
  Delete "$INSTDIR\RemoteMediaControlServer.exe.config"
  Delete "$INSTDIR\RemoteMediaControlServer.exe.manifest"
  Delete "$INSTDIR\RemoteMediaControlServer.pdb"
  Delete "$INSTDIR\RemoteMediaControlUpdater.exe"
  Delete "$INSTDIR\RemoteMediaControlUpdater.pdb"
  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  
  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  
  DeleteRegKey /ifempty HKCU "Software\Remote Media Control"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Run\Remote Media Control"

SectionEnd

Function LaunchLink
  ExecShell "" "$INSTDIR\RemoteMediaControlServer.exe"
FunctionEnd

/*
Delete autorun on uninstall+
CopyFiles+ TESTED
DelteFiles+
License+ TESTED
Icon+ TESTED
StartAfter install+ TESTED
*/