@echo off
set serviceName="Demo.Services.HelloWcfHost"
set folder=%cd%
set action="%1"
if %action% == "install" goto install
if %action% == "uninstall" goto uninstall
if %action% == "start" goto start
if %action% == "stop" goto stop
goto exit

:install
@echo on
cls
sc stop %serviceName%
sc delete %serviceName%
sc create %serviceName% binPath= "%folder%\bin\Debug\Demo.Services.HelloWcfHost.exe"
@echo off
goto exit

:uninstall
@echo on
sc stop %serviceName%
sc delete %serviceName%
@echo off
goto exit

:stop
@echo on
sc stop %serviceName%
@echo off
goto exit

:start
@echo on
sc start %serviceName%
@echo off
goto exit

:exit
