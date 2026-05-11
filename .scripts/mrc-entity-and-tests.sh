while getopts n: flag
do
    case "${flag}" in
        n) name=${OPTARG};;
    esac
done
echo "Creating Entity: $name";

dotnet new mrc-entity -n $name -o src/Domain --project src/Domain/Domain.csproj
dotnet new mrc-entity-test -n $name -o tests/Domain.UnitTests --project src/Domain/Domain.csproj