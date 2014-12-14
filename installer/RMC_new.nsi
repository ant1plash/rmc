  !include "MUI2.nsh"
  !include "DotNetChecker.nsh"
  !include "nsProcess.nsh"
  !define MUI_ICON "rmc.ico"
  !define MUI_UNICON "rmc.ico"
;--------------------------------
;General

  ;Name and file
  Name "Remote Media Control"
  OutFile "RemoteMediaControl_latest.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\Remote Media Control"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKLM "Software\Remote Media Control" ""

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
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKLM" 
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

 Function .onInit
		Call CheckPrevInstall ; Check and execute uninstall
 FunctionEnd
 
Section "Remote Media Control" SecRMC


SetOutPath "$INSTDIR"

  
  !insertmacro CheckNetFramework 40Client ;Download the framework
  
  File "Gma.QrCodeNet.Encoding.dll"
  File "Gma.QrCodeNet.Encoding.xml"
  File "ICSharpCode.SharpZipLib.dll"
  File "RMCConfig.exe"
  File "RMCSrv.exe"
  File "RMCUpdater.exe"

  ;Store installation folder
  WriteRegStr HKLM "Software\Remote Media Control" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
	CreateShortCut "$DESKTOP\Remote Media Control.lnk"  "$INSTDIR\RMCConfig.exe" ""
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd


Section "Start server when a user logs in." SecAutostart

 !include WinVer.nsh

  ${IfNot} ${AtLeastWinVista}
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Run""RemoteMediaControlServer" "$INSTDIR\RMCSrv.exe"
  ${Else}
	ExecWait "SCHTASKS /Create /TN RemoteMediaControlServer /RL HIGHEST /sc ONLOGON /TR $\"$INSTDIR\RMCSrv.exe$\""
  ${EndIf}
SectionEnd

;Remote Media Control
;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecRMC ${LANG_ENGLISH} "Server & Configuration tool"
  LangString DESC_SecAutostart ${LANG_ENGLISH} "Start server when a user logs in. This is optional."

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
  Delete "$INSTDIR\Gma.QrCodeNet.Encoding.xml"
  Delete "$INSTDIR\ICSharpCode.SharpZipLib.dll"
  Delete "$INSTDIR\RMCConfig.exe"
  Delete "$INSTDIR\RMCSrv.exe"
  Delete "$INSTDIR\RMCUpdateTool.exe"
  Delete "$INSTDIR\Uninstall.exe"
  
  RMDir "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  
  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  
  DeleteRegKey /ifempty HKLM "Software\Remote Media Control"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Run\Remote Media Control"

SectionEnd

Function LaunchLink


  ExecShell "" "$INSTDIR\RMCConfig.exe"
  
  ; WriteLog
  StrCpy $0 "$INSTDIR\install.log"
  Push $0
  Call DumpLog
FunctionEnd

Function CheckPrevInstall
 
  ReadRegStr $R0 HKLM "Software\Remote Media Control" ""
  StrCmp $R0 "" done
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "Remote Media Control is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
;Run the uninstaller
uninst:

  ${nsProcess::KillProcess} "RemoteMediaControlServer.exe" $R0
  ${nsProcess::KillProcess} "RMCSrv.exe" $R0
  ${nsProcess::KillProcess} "RMCConfig.exe" $R0
  ${nsProcess::KillProcess} "RMCUpdater.exe" $R0
  
  ClearErrors

  ExecWait '$R0/Uninstall.exe _?=$INSTDIR' ;Do not copy the uninstaller to a temp file
  



  
 
  IfErrors no_remove_uninstaller done
    ;You can either use Delete /REBOOTOK in the uninstaller or add some code
    ;here to remove the uninstaller. Use a registry key to check
    ;whether the user has chosen to uninstall. If you are using an uninstaller
    ;components page, make sure all sections are uninstalled.
  no_remove_uninstaller:
 
done:
 
FunctionEnd




!define LVM_GETITEMTEXT 0x102D
 
Function DumpLog
  Exch $5
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  Push $6
 
  FindWindow $0 "#32770" "" $HWNDPARENT
  GetDlgItem $0 $0 1016
  StrCmp $0 0 exit
  FileOpen $5 $5 "w"
  StrCmp $5 "" exit
    SendMessage $0 ${LVM_GETITEMCOUNT} 0 0 $6
    System::Alloc ${NSIS_MAX_STRLEN}
    Pop $3
    StrCpy $2 0
    System::Call "*(i, i, i, i, i, i, i, i, i) i \
      (0, 0, 0, 0, 0, r3, ${NSIS_MAX_STRLEN}) .r1"
    loop: StrCmp $2 $6 done
      System::Call "User32::SendMessageA(i, i, i, i) i \
        ($0, ${LVM_GETITEMTEXT}, $2, r1)"
      System::Call "*$3(&t${NSIS_MAX_STRLEN} .r4)"
      FileWrite $5 "$4$\r$\n"
      IntOp $2 $2 + 1
      Goto loop
    done:
      FileClose $5
      System::Free $1
      System::Free $3
  exit:
    Pop $6
    Pop $4
    Pop $3
    Pop $2
    Pop $1
    Pop $0
    Exch $5
FunctionEnd


