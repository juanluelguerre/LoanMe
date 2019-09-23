@echo off

REM
REM PRE-REQUISITES:
REM With a .NET Core 3.0 SDK, run "dotnet tool install -g dotnet-ef and run dotnet ef"
REM 

cls
echo.

echo RUNINNG Migration for Catalog...
cd src\LoanMe.Catalog.Api

dotnet ef migrations add Init -o Infrastructure\Migrations

if %ERRORLEVEL% == 0 (
    echo RUNINNG Database Update...
    cd src
    
) else (
    echo Error found: No 'database update' started !
)

echo.

REM echo RUNINNG Migration for Customers...
REM cd ..\..
REM cd src\LoanMe.Customers.Api

REM dotnet ef migrations add Init -o Infrastructure\Migrations

REM if %ERRORLEVEL% == 0 (
REM     echo RUNINNG Database Update...
REM     dotnet ef database update
REM ) else (
REM     echo Error found: No 'database update' started !
REM )
REM cd ..\..
REM echo.

REM echo RUNINNG Migration for Loans...
REM cd ..\..
REM cd src\LoanMe.Finance.Api

REM dotnet ef migrations add Init -o Infrastructure\Migrations

REM if %ERRORLEVEL% == 0 (
REM     echo RUNINNG Database Update...
REM     dotnet ef database update
REM ) else (
REM     echo Error found: No 'database update' started !
REM )
REM echo.



cd ..\..