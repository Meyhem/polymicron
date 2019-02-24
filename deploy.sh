#!/bin/sh

systemctl is-active --quiet polymicronblog && { echo 'meyhemblog.service is active, stop first'; exit 1; }

dotnet --version || { echo 'Error: No dotnet installed' ; exit 1; }

echo 'Building'
dotnet publish --configuration Release || { echo 'Error: dotnet publish failed' ; exit 1; }

echo 'Copying production files to /var/asp/blog'
cp -R PolyMicron/bin/Release/netcoreapp2.1/publish/. /var/asp/polymicronblog

echo 'Changing owner to www-data'
chown -R www-data:www-data /var/asp/polymicronblog || { echo 'Cannot change owner to www-data'; exit 1; }

echo 'Deploying polymicronblog.service file'
cp polymicronblog.service /etc/systemd/system/

echo 'Cleaning'
dotnet clean
