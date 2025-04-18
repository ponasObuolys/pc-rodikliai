@echo off
cd /d "%~dp0"
powershell -Command "Start-Process 'bin\Debug\net8.0-windows\PC-Rodikliai.exe' -Verb RunAs" 