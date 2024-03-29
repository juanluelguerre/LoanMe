# The actual image is tiny (applies an init script to an existing image), so
# just rebuild on every restart in case the repo has updated. This way users
#  never have to worry about it.
echo "BUILDING postgres ..."

docker build -t marten-postgres .
docker rm -f marten-db
#  Use a unique-ish port in case other psql instances are running
docker run --name marten-db -e POSTGRES_PASSWORD=Password12! -p 5454:5432 -td marten-postgres

export marten_testing_database=Host=localhost\;Port=5454\;Database=marten_test\;Username=postgres\;Password=Password12!

echo "Postgres with plv8 has been started. In order to set the connection string environment variable, either run this script as '. ./startPostgres.sh' or run the following command:"
echo "SET marten_testing_database=Host=localhost\;Port=5454\;Database=marten_test\;Username=postgres\;Password=Password12!"
