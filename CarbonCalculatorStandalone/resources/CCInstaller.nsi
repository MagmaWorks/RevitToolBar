; CalcsInstaller.nsi
;
; This script remembers the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,

;--------------------------------

; The name of the installer
Name "Magma Works Carbon Calculator"

; The file to write
OutFile "Carbon Calculator.exe"

; The default installation directory
InstallDir "$PROGRAMFILES64\Magma Works\CarbonCalculator"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\Magma Works\CarbonCalculator" "Install_Dir"

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
Section "Carbon Calculator for Windows (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "CarbonCalculatorStandalone.exe"
  File "CarbonCalculatorWPF.dll"
  File "CarbonMaterials.dll"
  File "LiveCharts.dll"
  File "LiveCharts.Wpf.dll"
  File "Newtonsoft.Json.dll" 

  
  SetOutPath "$INSTDIR\Resources"
  File /nonfatal /a /r "Resources\"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\CarbonCalculatorStandalone "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CarbonCalculatorStandalone" "DisplayName" "Carbon Calculator for Windows"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CarbonCalculatorStandalone" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CarbonCalculatorStandalone" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CarbonCalculatorStandalone" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\CarbonCalculator"
  CreateShortcut "$SMPROGRAMS\CarbonCalculator\CarbonCalculator.lnk" "$INSTDIR\CarbonCalculatorStandalone.exe" "" "$INSTDIR\CarbonCalculatorStandalone.exe" 0
  
SectionEnd

;Section "Desktop Shortcut"

;  CreateShortCut "$DESKTOP\SCaFFOLD.lnk" "$INSTDIR\Calcs.exe"
  
;SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CarbonCalculatorStandalone"
  DeleteRegKey HKLM SOFTWARE\CarbonCalculatorStandalone
  
  ; Remove files and uninstaller

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\CarbonCalculator\*.*"
  ;Delete "$DESKTOP\CarbonCalculator.lnk"

  ; Remove directories used
  RMDir "$SMPROGRAMS\CarbonCalculator"
  RMDir $INSTDIR
  RMDir $INSTDIR\data
  RMDir /r /REBOOTOK $INSTDIR
  RMDir /REBOOTOK $INSTDIR\DLLs

SectionEnd
