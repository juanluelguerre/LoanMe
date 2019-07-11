echo "RUNINNG Migration for Customers..."
cd ./src/LoanMe.Customers.Api

if dotnet ef migrations add Init; then
   echo "Error found: No 'database update' started !"
 else 
    dotnet ef database update
fi

echo "RUNINNG Migration for Loans..."

cd ../..
cd ./src/LoanMe.Finance.Api

if dotnet ef migrations add Init ; then
        dotnet ef database update
else 
    echo "Error found: No 'database update' started !"
fi

cd ../..