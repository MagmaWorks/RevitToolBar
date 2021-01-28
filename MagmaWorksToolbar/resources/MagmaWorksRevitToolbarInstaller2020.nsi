; CalcsInstaller.nsi
;
; This script remembers the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,

;--------------------------------

; The name of the installer
Name "Magma Works Revit Toolbar"

; The file to write
OutFile "MagmaWorksRevitToolBar2020.exe"

Function .onInit
SetShellVarContext all
StrCpy $InstDir $APPDATA\Autodesk\Revit\Addins\2020
FunctionEnd

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\MagmaWorksToolbar2020" "Install_Dir"

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
Section "MagmaWorksRevitToolbar (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "MagmaWorksToolbar.addin"

  
  SetOutPath "$INSTDIR\MagmaWorksToolbar"
  File /nonfatal /a /r "MagmaWorksToolbar\"

  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\MagmaWorksToolbar2020 "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MagmaWorksToolbar2020" "DisplayName" "Magma Works Revit Toolbar"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MagmaWorksToolbar2020" "UninstallString" '"$INSTDIR\MagmaWorksToolbarUninstaller.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MagmaWorksToolbar2020" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MagmaWorksToolbar2020" "NoRepair" 1
  WriteUninstaller "$INSTDIR\MagmaWorksToolbarUninstaller.exe"
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MagmaWorksToolbar2020"
  DeleteRegKey HKLM SOFTWARE\MagmaWorksToolbar2020

  ; Remove files and uninstaller
  Delete $INSTDIR\MagmaWorksToolbar.addin
  Delete $INSTDIR\MagmaWorksToolbarUninstaller.exe

  ; Remove directories used
  RMDir $INSTDIR\MagmaWorksToolbar
  RMDir /r /REBOOTOK $INSTDIR\MagmaWorksToolbar

SectionEnd
