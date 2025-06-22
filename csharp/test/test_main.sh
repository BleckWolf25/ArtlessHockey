#!/bin/bash
set -e

dotnet build ../src

dotnet run --project ../src

echo "Test completed."
