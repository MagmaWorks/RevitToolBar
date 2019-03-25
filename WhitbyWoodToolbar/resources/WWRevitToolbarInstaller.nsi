; CalcsInstaller.nsi
;
; This script remembers the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,

;--------------------------------

; The name of the installer
Name "Whitby Wood Revit Toolbar"

; The file to write
OutFile "WWRevitToolBar.exe"

; The default installation directory
InstallDir $PROGRAMFILES\WW_Calcs

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\WW_Calcs" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "WW_Calcs (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "Calcs.exe"
  File "BriefFiniteElementNet.dll"
  File "CalcCore.dll"
  File "DocumentFormat.OpenXml.dll"
  File "DynamicRelaxation.dll"
  File "HelixToolkit.dll"
  File "HelixToolkit.Wpf.dll"
  File "LiveCharts.dll"
  File "LiveCharts.Wpf.dll"
  File "netDxf.dll"
  File "Newtonsoft.Json.dll"
  File "TestCalcs.dll"
  File "WindowsBase.dll"
  File "WpfMath.dll"
  
  SetOutPath "$INSTDIR\Libraries"
  File /nonfatal /a /r "Libraries\"

  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\WW_Calcs "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WW_Calcs" "DisplayName" "Whitby Wood Calcs"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WW_Calcs" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WW_Calcs" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WW_Calcs" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\WW_Calcs"
  CreateShortcut "$SMPROGRAMS\WW_Calcs\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortcut "$SMPROGRAMS\WW_Calcs\WW_Calcs (MakeNSISW).lnk" "$INSTDIR\Calcs.exe" "" "$INSTDIR\Calcs.exe" 0
  
SectionEnd

Section "Desktop Shortcut"

  CreateShortCut "$DESKTOP\WW Calcs.lnk" "$INSTDIR\Calcs.exe"
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WW_Calcs"
  DeleteRegKey HKLM SOFTWARE\WW_Calcs

  ; Remove files and uninstaller

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\WW_Calcs\*.*"
  Delete "$DESKTOP\WW Calcs.lnk"

  ; Remove directories used
  RMDir "$SMPROGRAMS\WW_Calcs"
  RMDir $INSTDIR
  RMDir $INSTDIR\data
  RMDir /r /REBOOTOK $INSTDIR
  RMDir /REBOOTOK $INSTDIR\DLLs

SectionEnd
