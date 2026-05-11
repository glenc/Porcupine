while getopts n:f:t:r: flag
do
    case "${flag}" in
        n) name=${OPTARG};;
        f) featureName=${OPTARG};;
        t) usecaseType=${OPTARG};;
        r) returnType=${OPTARG};;
    esac
done
echo "Creating Use Case: $name";

dotnet new mrc-usecase -n $name -fn $featureName -ut $usecaseType -rt $returnType -o src/Application --project src/Application/Application.csproj
dotnet new mrc-usecase-test -n $name -fn $featureName -ut $usecaseType -rt $returnType -o tests/Application.FunctionalTests --project src/Application/Application.csproj