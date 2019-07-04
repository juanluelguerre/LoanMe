@echo off

cls
echo.

echo RUNINNG Migration for Customers...
cd src\MyBudget.Customers.Api

dotnet ef migrations add Init 

if %ERRORLEVEL% == 0 (
    echo RUNINNG Database Update...
    dotnet ef database update
) else (
    echo Error found: No 'database update' started !
)
echo.

echo RUNINNG Migration for Loans...

cd ..
cd src\MyBudget.Loan.Api

dotnet ef migrations add Init 

if %ERRORLEVEL% == 0 (
    echo RUNINNG Database Update...
    dotnet ef database update
) else (
    echo Error found: No 'database update' started !
)
echo.



cd ..\..