!include "LogicLib.nsh"

Function GetDotNET8Version
  ClearErrors
  ReadRegDWORD $0 HKLM "SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedhost" "Version"
  IfErrors 0 found
  Push "0"
  Return
found:
  Push "1"
FunctionEnd

Var DOTNET_RETURN_CODE

Section "Microsoft .NET 8 Desktop Runtime"
  SectionIn RO
  
  ; search for /SKIPDOTNET on commandline and skip if found
  Push $CMDLINE
  Push "/SKIPDOTNET"
  Call StrStr
  Pop $0
  StrCmp $0 "" lbl_notSkipped
  Goto lbl_Done
  
  lbl_notSkipped:
  Call GetDotNET8Version
  Pop $0
  StrCmp $0 "1" lbl_Done
  
  lbl_DotNetVersionNotFound:
  MessageBox MB_ICONEXCLAMATION|MB_YESNO|MB_DEFBUTTON2 "Microsoft .NET 8 Desktop Runtime (x64) is required to run EVEMon, and does not appear to be installed.$\n$\nWould you like to automatically download and install it now?" /SD IDNO IDYES lbl_Confirmed IDNO lbl_Cancelled

  lbl_Cancelled:                  
  Abort "Microsoft .NET 8 Desktop Runtime is required."

  lbl_Confirmed:
  nsisdl::download \
         /TIMEOUT=120000 "https://download.visualstudio.microsoft.com/download/pr/9b4eb9ab-873b-48aa-b935-4dbbdf06ce59/ea59c19b0cece9d671801c6e1dbbe373/windowsdesktop-runtime-8.0.3-win-x64.exe" "$PLUGINSDIR\dotnet8.exe"
  Pop $0
  StrCmp "$0" "success" lbl_continue
  Abort "Microsoft .NET 8 Desktop Runtime download failed"

  lbl_continue:
  Banner::show /NOUNLOAD "Installing .NET 8 Desktop Runtime..."
  nsExec::ExecToStack '"$PLUGINSDIR\dotnet8.exe" /install /quiet /norestart'
  Pop $DOTNET_RETURN_CODE
  Banner::destroy

  StrCmp "$DOTNET_RETURN_CODE" "" lbl_NoError
  StrCmp "$DOTNET_RETURN_CODE" "0" lbl_NoError
  StrCmp "$DOTNET_RETURN_CODE" "1641" lbl_NoError
  StrCmp "$DOTNET_RETURN_CODE" "3010" lbl_NoError
  StrCmp "$DOTNET_RETURN_CODE" "error" lbl_Error
  StrCmp "$DOTNET_RETURN_CODE" "timeout" lbl_TimeOut
  Goto lbl_Error

  lbl_TimeOut:
  Abort "The .NET 8 Runtime download timed out."

  lbl_Error:
  Abort "The .NET 8 Runtime install failed (error code $DOTNET_RETURN_CODE)."
  
  lbl_NoError:
  lbl_Done:
SectionEnd
