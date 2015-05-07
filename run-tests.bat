@echo off
setlocal



rem ---- Input params ----
rem %1 = Platform (Win32, AnyCPU)
rem %2 = Configuration (Release, Debug)

rem AnyCPU runs in Release/Debug folder (no .x64 or .AnyCPU)

set "platform=%~1"
set "config=%~2"
if "%platform%" == "" set "platform=AnyCPU"
if "%config%" == "" set "config=Release"

set "validPlatform=false"
if "%platform%" == "AnyCPU" set validPlatform="true"
if "%platform%" == "Win32" set validPlatform="true"

if "%validPlatform%" == "false" GOTO InvalidPlatform

set "nunit=.\target\dependency\bin\vc100\Release\nunit\nunit-console.exe"
if "%platform%" == "Win32" set "nunit=.\target\dependency\bin\vc100\Release\nunit\nunit-console-x86.exe"

rem Get folder of this batch file.
set "thisfolder=%~dp0"

if "%platform%" == "AnyCPU" set "platform="
if "%platform%" == "Win32" set "platform=.%platform%"

echo Running unit tests in %config%%platform%
set "errors=0"

set "dllToTestPath=%thisfolder%AutomateCmdSequenceLibTests\bin\%config%\"

rem ---TEST DLLS TO RUN-------------------
set testfiles=^
 AutomateCmdSequenceLibTests.dll

for %%A in (%testfiles%) do call :test_block %%A

exit /b %errors%

:InvalidPlatform

echo "Invalid platform specified: %platform%"
set "errors=1"

exit /b %errors%

rem ---TEST BLOCK-------------------
:test_block
set "testerrors=0"
set "testfile=%1"
echo testing: %testfile%
set "TESTS_LOG=test-%config%%platform%-%testfile%.log"
if exist %TESTS_LOG% del %TESTS_LOG%
echo Running test: %dllToTestPath%%testfile%
%nunit% %dllToTestPath%%testfile% >> %TESTS_LOG% 2>&1 || set "testerrors=1"
if %testerrors% NEQ 0 ( 
	echo ERROR: One or more unit tests failed!  See %TESTS_LOG% for details.
	set "errors=1"
	if exist TestResult_%testfile%.xml del TestResult_%testfile%.xml
	rename TestResult.xml TestResult_%testfile%.xml
	)


