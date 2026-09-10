@echo off
REM Minimal build: compiles the DLL and drops it into GameData\GroundHeightProbeMod\.
REM Installing means copying that folder into the GameData of KSP -- this script never does it.
setlocal
cd /d "%~dp0"

if not defined KSPDIR (
    echo ERROR: KSPDIR is not set. Point it at your KSP install folder.
    exit /b 1
)

dotnet build GroundHeightProbeMod.csproj -p:KSP_DATA_DIR="%KSPDIR%\KSP_x64_Data"
if errorlevel 1 (
    echo ERROR: build failed
    exit /b 1
)

copy /y "Output\bin\GroundHeightProbeMod.dll" "GameData\GroundHeightProbeMod\" >nul
if errorlevel 1 (
    echo ERROR: could not copy the DLL into GameData
    exit /b 1
)

echo.
echo Built: GameData\GroundHeightProbeMod\GroundHeightProbeMod.dll
echo Copy GameData\GroundHeightProbeMod into the GameData of KSP to install it.
