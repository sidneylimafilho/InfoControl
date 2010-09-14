@echo off
set TabName="VivinaDataControls"
set DllName="%2"
set ControlsFolder="%userprofile%\My Documents\Visual Studio 2005\Controls\%TabName%"
mkdir %ControlsFolder%
cd bin/release/
copy *.dll %ControlsFolder%
"%VS80COMNTOOLS%\..\IDE\devenv.exe" /command Tools.InstallCommunityControls
#set TabName=
#set DllName=
#set ControlsFolder=
