@ECHO OFF

rem build init
set project=MediaPortal
call BuildInit.bat %1

rem build
echo.
echo Writing GIT revision assemblies...
rem %DeployVersionGIT% /git="%GIT_ROOT%" /path="%MediaPortal%" >> %log%
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%CommonMPTV%" >> %log%

echo.
echo Building Libbluray Java...
call %ant_home%\bin\ant -f %LibblurayJAR% -Dsrc_awt=:java-j2se

echo.
echo Building MediaPortal...
set xml=Build_Report_%BUILD_TYPE%_MediaPortal.xml
set html=Build_Report_%BUILD_TYPE%_MediaPortal.html
set logger=/l:XmlFileLogger,"BuildReport\MSBuild.ExtensionPack.Loggers.dll";logfile=%xml%

"%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBUILD.exe" %logger% /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86 "%MediaPortal%\MediaPortal.sln" >> %log%
BuildReport\msxsl %xml% _BuildReport_Files\BuildReport.xslt -o %html%

echo.
echo Reverting assemblies...
rem %DeployVersionGIT% /git="%GIT_ROOT%" /path="%MediaPortal%" /revert >> %log%
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%CommonMPTV%" /revert >> %log%

echo.
echo Reading the git revision...
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%CommonMPTV%" /GetVersion >> %log%
rem SET /p version=<version.txt >> %log%
SET version=%errorlevel%
DEL version.txt >> %log%

echo.
echo Make MediaPortal 2GB LARGEADDRESSAWARE...
call MSBUILD_MP_LargeAddressAware.bat %BUILD_TYPE%

echo.
echo Building Installer...
"%progpath%\NSIS\makensis.exe" /DBUILD_TYPE=%BUILD_TYPE% /DVER_BUILD=%version% "%MediaPortal%\Setup\setup.nsi" >> %log%
