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

Function .onInit
SetShellVarContext all
StrCpy $InstDir $APPDATA\Autodesk\Revit\Addins\2018
FunctionEnd

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\WWToolbar" "Install_Dir"

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
Section "WWRevitToolbar (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "WWToolbar.addin"

  
  SetOutPath "$INSTDIR\WWToolbar"
  File /nonfatal /a /r "WWToolbar\"

  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\WWToolbar "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WWToolbar" "DisplayName" "Whitby Wood Revit Toolbar"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WWToolbar" "UninstallString" '"$INSTDIR\WWToolbarUninstaller.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WWToolbar" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WWToolbar" "NoRepair" 1
  WriteUninstaller "$INSTDIR\WWToolbarUninstaller.exe"
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WWToolbar"
  DeleteRegKey HKLM SOFTWARE\WWToolbar

  ; Remove files and uninstaller
  Delete $INSTDIR\WWToolbar.addin
  Delete $INSTDIR\WWToolbarUninstaller.exe

  ; Remove directories used
  RMDir $INSTDIR\WWToolbar
  RMDir /r /REBOOTOK $INSTDIR\WWToolbar
  ;RMDir /REBOOTOK $INSTDIR\DLLs

SectionEnd
