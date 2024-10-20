#!/bin/bash
echo "WARNING: Restart JackalWebHost2 after 2 seconds..."
for ((count=2; count > 0; count--))
do
echo "$count...";
sleep 1;
done

pkill -f dotnet
echo "killed dotnet";
sleep 1;

echo "Waiting for kill: 2 seconds..."
for ((count=2; count > 0; count--))
do
echo "$count...";
sleep 1;
done

cd JackalWebHost2
nohup dotnet run -c=Release &
sleep 1;
echo "started JackalWebHost2.csproj";
cd ..

echo "Finish, OK."
sleep 1;
exit 0