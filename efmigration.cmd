@echo off

cls
echo.

echo RUNINNG Migration for Catalog...
cd src\LoanMe.Catalog.Api

dotnet ef migrations add Init 

if %ERRORLEVEL% == 0 (
    echo RUNINNG Database Update...
    dotnet ef database update
) else (
    echo Error found: No 'database update' started !
)

echo.

REM echo RUNINNG Migration for Customers...
REM cd ..\..
REM cd src\LoanMe.Customers.Api

REM dotnet ef migrations add Init 

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

REM dotnet ef migrations add Init 

REM if %ERRORLEVEL% == 0 (
REM     echo RUNINNG Database Update...
REM     dotnet ef database update
REM ) else (
REM     echo Error found: No 'database update' started !
REM )
REM echo.



cd ..\..