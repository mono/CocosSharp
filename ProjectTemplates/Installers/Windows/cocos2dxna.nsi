SetCompressor /SOLID /FINAL lzma

!define FrameworkPath "C:\Users\Jacob Anderson\Documents\GitHub\CocosSharp"
!define VERSION "3.0"
!define REVISION "0.0"
!define INSTALLERFILENAME "Cocos2dXNA"
!define APPNAME "Cocos2D XNA"

;Include Modern UI

!include "Sections.nsh"
!include "MUI2.nsh"
!include "InstallOptions.nsh"

!define MUI_ICON "${FrameworkPath}\ProjectTemplates\Installers\cocos2dxna.ico"

!define MUI_UNICON "${FrameworkPath}\ProjectTemplates\Installers\cocos2dxna.ico"

Name '${APPNAME} ${VERSION}'
OutFile '${INSTALLERFILENAME}Installer-${VERSION}.exe'
InstallDir '$PROGRAMFILES\${APPNAME}\v${VERSION}'
VIProductVersion "${VERSION}.${REVISION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "${APPNAME} Development Tools"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "Coco2D XNA"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductVersion" "${VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "${APPNAME} Installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "© Copyright Cocos2D XNA Development Team"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;Interface Configuration

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "${FrameworkPath}\ProjectTemplates\Installers\cocos2dxna_8bit.bmp"
!define MUI_ABORTWARNING

!define MUI_WELCOMEFINISHPAGE_BITMAP "${FrameworkPath}\ProjectTemplates\Installers\panel_8bit.bmp"
;Languages

!insertmacro MUI_PAGE_WELCOME

;!insertmacro MUI_PAGE_LICENSE "License.txt"

!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_INSTFILES

;Page custom XamarinPageEnter XamarinPageLeave

;  Function XamarinPageEnter
;      ReserveFile "xamarin.ini"
;      ReserveFile "mono_8bit.bmp"
;      ReserveFile "xamarin_8bit.bmp"
;      !insertmacro INSTALLOPTIONS_EXTRACT "xamarin.ini"
;      !insertmacro INSTALLOPTIONS_WRITE "xamarin.ini" "Field 1" "Text" "mono_8bit.bmp"
;      !insertmacro INSTALLOPTIONS_WRITE "xamarin.ini" "Field 2" "Text" "xamarin_8bit.bmp"
;      !insertmacro INSTALLOPTIONS_DISPLAY "xamarin.ini"
;  FunctionEnd
  
;  Function XamarinPageLeave
;  FunctionEnd

!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

;!insertmacro MUI_LANGUAGE "English"

;--------------------------------

Function CustomPageFunction ;Function name defined with Page command
  !insertmacro INSTALLOPTIONS_DISPLAY "xamarin.ini"
FunctionEnd


; The stuff to install
Section "Cocos2D XNA Core Components" CoreComponents ;No components page, name is not important
  SectionIn RO
  SetOutPath '$PROGRAMFILES32\MSBuild\${APPNAME}\v${VERSION}'
  File '..\..\..\docs\cocos2dxna.ico'
  
  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\Android'
  File /nonfatal '..\..\cocos2d\bin\Android\Release\*.dll'
  File /nonfatal ' ..\..\cocos2d\bin\Android\Release\*.xml'  

  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\OUYA'
  File /nonfatal '..\..\cocos2d\bin\OUYA\Release\*.dll'
  File /nonfatal ' ..\..\cocos2d\bin\OUYA\Release\*.xml'  
  File /nonfatal '..\..\ThirdParty\Libs\OUYA\*.dll'
  
  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\WindowsGL'
  File /nonfatal '..\..\cocos2d\bin\WindowsGL\Release\*.dll'
  File /nonfatal ' ..\..\cocos2d\bin\WindowsGL\Release\*.xml'
  
  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\Windows'
  File /nonfatal '..\..\..\cocos2d\bin\Windows\Release\*.dll'
  File /nonfatal '..\..\..\cocos2d\bin\Windows\Release\*.xml'

  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\Linux'
  File /nonfatal '..\..\cocos2d\bin\Linux\Release\*.dll'
  File /nonfatal ' ..\..\cocos2d\bin\Linux\Release\*.xml'
    
;  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\Windows8'
;  File '..\..\cocos2d\bin\Windows8\Release\cocos2d.dll'
;  File /nonfatal '..\..\cocos2d\bin\Windows8\Release\cocos2d.xml'

  ; Install Windows Phone ARM Assemblies
;  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\WindowsPhone\ARM'
;  File '..\..\cocos2d\bin\WindowsPhone\ARM\Release\cocos2d.dll'
;  File /nonfatal '..\..\cocos2d\bin\WindowsPhone\ARM\Release\cocos2d.xml'

  ; Install Windows Phone x86 Assemblies
;  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\WindowsPhone\x86'
;  File '..\..\cocos2d\bin\WindowsPhone\x86\Release\cocos2d.dll'
;  File /nonfatal '..\..\cocos2d\bin\WindowsPhone\86\Release\cocos2d.xml'

  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}\Assemblies\WindowsPhone'
;  File /r '..\..\ThirdParty\Libs\SharpDX\Windows Phone\*.dll'
;  File /r '..\..\ThirdParty\Libs\SharpDX\Windows Phone\*.xml'  

  WriteRegStr HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows GL' '' '$INSTDIR\Assemblies\WindowsGL'
  WriteRegStr HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows' '' '$INSTDIR\Assemblies\Windows'
  WriteRegStr HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Linux' '' '$INSTDIR\Assemblies\Linux'
  WriteRegStr HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.5.50709\AssemblyFoldersEx\${APPNAME} for Windows Store' '' '$INSTDIR\Assemblies\Windows8'
  WriteRegStr HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone ARM' '' '$INSTDIR\Assemblies\WindowsPhone\ARM'
  WriteRegStr HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone x86' '' '$INSTDIR\Assemblies\WindowsPhone\x86'
  WriteRegStr HKLM 'SOFTWARE\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for Android' '' '$INSTDIR\Assemblies\Android'
  WriteRegStr HKLM 'SOFTWARE\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for OUYA' '' '$INSTDIR\Assemblies\OUYA'

  IfFileExists $WINDIR\SYSWOW64\*.* Is64bit Is32bit
  Is32bit:
    GOTO End32Bitvs64BitCheck
  Is64bit:
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows GL' '' '$INSTDIR\Assemblies\WindowsGL'
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows' '' '$INSTDIR\Assemblies\Windows'
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.5.50709\AssemblyFoldersEx\${APPNAME} for Windows Store' '' '$INSTDIR\Assemblies\Windows8'
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for Android' '' '$INSTDIR\Assemblies\Android'
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for OUYA' '' '$INSTDIR\Assemblies\OUYA'
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.5.50709\AssemblyFoldersEx\${APPNAME} for Linux' '' '$INSTDIR\Assemblies\Linux'
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone ARM' '' '$INSTDIR\Assemblies\WindowsPhone\ARM'
    WriteRegStr HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone x86' '' '$INSTDIR\Assemblies\WindowsPhone\x86'

  End32Bitvs64BitCheck:
  ; Add remote programs
  WriteRegStr HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}' 'DisplayName' '${APPNAME}'
  WriteRegStr HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}' 'DisplayVersion' '${VERSION}'
  WriteRegStr HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}' 'DisplayIcon' '$INSTDIR\cocos2dxna.ico'
  WriteRegStr HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}' 'InstallLocation' '$INSTDIR\'
  WriteRegStr HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}' 'Publisher' 'Cocos2D-XNA'
  WriteRegStr HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}' 'UninstallString' '$INSTDIR\uninstall.exe'


  SetOutPath '$PROGRAMFILES\${APPNAME}\v${VERSION}'
  File '${FrameworkPath}\docs\cocos2dxna.ico'
  ; Uninstaller
  WriteUninstaller "uninstall.exe"


SectionEnd

; We do not have anything to install for OpenAL
;Section "OpenAL" OpenAL
  ; SetOutPath $INSTDIR
  ;File '${FrameworkPath}\ThirdParty\Libs\oalinst.exe'
  ;ExecWait '"$INSTDIR\oalinst.exe /S"'
;SectionEnd

Section "Visual Studio 2010 Templates" VS2010

  IfFileExists `$DOCUMENTS\Visual Studio 2010\Templates\ProjectTemplates\Visual C#` InstallTemplates CannotInstallTemplates
  InstallTemplates:
    ; Set output path to the installation directory.
    SetOutPath "$DOCUMENTS\Visual Studio 2010\Templates\ProjectTemplates\Visual C#\Cocos2D-XNA"

    ; install the Templates for MonoDevelop
    File /r '..\..\..\ProjectTemplates\GameStarterKit\VisualStudio2010\*.zip'
    GOTO EndTemplates
  CannotInstallTemplates:
  
    DetailPrint "Visual Studio 2010 not found"
  EndTemplates:

SectionEnd

Section "Visual Studio 2012 Templates" VS2012

  IfFileExists `$DOCUMENTS\Visual Studio 2012\Templates\ProjectTemplates\Visual C#\*.*` InstallTemplates CannotInstallTemplates
  InstallTemplates:
    ; Set output path to the installation directory.
    SetOutPath "$DOCUMENTS\Visual Studio 2012\Templates\ProjectTemplates\Visual C#\Cocos2D-XNA"

    ; install the Templates for MonoDevelop
    ;File /r '..\..\..\ProjectTemplates\VisualStudio2012\*.zip'
    ; Install the VS 2010 templates as well 
    File /r '..\..\..\ProjectTemplates\GameStarterKit\VisualStudio2010\*.zip'
    GOTO EndTemplates
  CannotInstallTemplates:

    DetailPrint "Visual Studio 2012 not found"
  EndTemplates:

SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts" Menu
	CreateDirectory "$SMPROGRAMS\Cocos2D XNA"
	CreateShortCut "$SMPROGRAMS\Cocos2D XNA\Uninstall.lnk" "$PROGRAMFILES\${APPNAME}\v${VERSION}\uninstall.exe" "" "$PROGRAMFILES\${APPNAME}\v${VERSION}\uninstall.exe" 0
	;CreateShortCut "$SMPROGRAMS\Cocos2D XNA\GettingStarted.lnk" "$PROGRAMFILES\${APPNAME}\v${VERSION}\GettingStarted.pdf" "" "$PROGRAMFILES\${APPNAME}\v${VERSION}\GettingStarted.pdf" 0
	CreateShortCut "$SMPROGRAMS\Cocos2D XNA\Templates.lnk" "$DOCUMENTS\Visual Studio 2010\Templates\ProjectTemplates\Visual C#\Cocos2D-XNA" "" "$DOCUMENTS\Visual Studio 2010\Templates\ProjectTemplates\Visual C#\Cocos2D-XNA" 0
	WriteINIStr "$SMPROGRAMS\Cocos2D XNA\Project Web Site.url" "InternetShortcut" "URL" "http://github.com/mono/CocosSharp"
	WriteINIStr "$SMPROGRAMS\Cocos2D XNA\Project Web Site.url" "InternetShortcut" "IconFile" "$PROGRAMFILES\${APPNAME}\v${VERSION}\Icon.ico"
	WriteINIStr "$SMPROGRAMS\Cocos2D XNA\Project Web Site.url" "InternetShortcut" "IconIndex" "0"

SectionEnd

LangString CoreComponentsDesc ${LANG_ENGLISH} "Install the Runtimes and the MSBuild extensions for Cocos2D-XNA"
;LangString OpenALDesc ${LANG_ENGLISH} "Install the OpenAL drivers"
LangString MonoDevelopDesc ${LANG_ENGLISH} "Install the project templates for MonoDevelop"
LangString VS2010Desc ${LANG_ENGLISH} "Install the project templates for Visual Studio 2010"
LangString VS2012Desc ${LANG_ENGLISH} "Install the project templates for Visual Studio 2012"
LangString MenuDesc ${LANG_ENGLISH} "Add a link to the Cocos2D XNA website to your start menu"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${CoreComponents} $(CoreComponentsDesc)
  !insertmacro MUI_DESCRIPTION_TEXT ${OpenAL} $(OpenALDesc)
  !insertmacro MUI_DESCRIPTION_TEXT ${MonoDevelop} $(MonoDevelopDesc)
  !insertmacro MUI_DESCRIPTION_TEXT ${VS2010} $(VS2010Desc)
  !insertmacro MUI_DESCRIPTION_TEXT ${VS2012} $(VS2012Desc)
  !insertmacro MUI_DESCRIPTION_TEXT ${Menu} $(MenuDesc)
!insertmacro MUI_FUNCTION_DESCRIPTION_END



Function .onInit

  IntOp $0 ${SF_SELECTED} | ${SF_RO}
  SectionSetFlags ${core_id} $0
FunctionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  DeleteRegKey HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows GL'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Linux'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME}'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.5.50709\AssemblyFoldersEx\${APPNAME} for Windows Store' 
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone ARM'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone x86'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for Android'
  DeleteRegKey HKLM 'SOFTWARE\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for OUYA'

  IfFileExists $WINDIR\SYSWOW64\*.* Is64bit Is32bit
  Is32bit:
    GOTO End32Bitvs64BitCheck
  Is64bit:
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows GL'
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows'
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Linux'
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.5.50709\AssemblyFoldersEx\${APPNAME} for Windows Store'
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone ARM'
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\${APPNAME} for Windows Phone x86'
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for Android'
    DeleteRegKey HKLM 'SOFTWARE\Wow6432Node\Microsoft\MonoAndroid\v2.3\AssemblyFoldersEx\${APPNAME} for OUYA'


  End32Bitvs64BitCheck:

  ReadRegStr $0 HKLM 'SOFTWARE\Wow6432Node\Xamarin\MonoDevelop' "Path"
  ${If} $0 == "" ; check on 32 bit machines just in case
  ReadRegStr $0 HKLM 'SOFTWARE\Xamarin\MonoDevelop' "Path"
  ${EndIf}

  ${If} $0 == ""
  ${Else}
  RMDir /r "$0\AddIns\MonoDevelop.Cocos2D XNA"
  ${EndIf}
  
  RMDir /r "$DOCUMENTS\Visual Studio 2010\Templates\ProjectTemplates\Visual C#\Cocos2D XNA"
  RMDir /r "$DOCUMENTS\Visual Studio 2012\Templates\ProjectTemplates\Visual C#\Cocos2D XNA"
  RMDir /r "$PROGRAMFILES32\MSBuild\${APPNAME}\v${VERSION}"
  RMDir /r "$SMPROGRAMS\Cocos2D XNA"

  Delete "$INSTDIR\Uninstall.exe"
  RMDir /r "$INSTDIR"

SectionEnd

