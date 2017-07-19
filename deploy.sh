#!/bin/bash
mkdir -p deployed_code
cd ./Diporto
dotnet publish Diporto.csproj --output $TRAVIS_BUILD_DIR/deployed_code --configuration Release
cd ..
chmod 600 deploy_key
mv deploy_key ~/.ssh/id_rsa
rsync -r --delete-after --quiet $TRAVIS_BUILD_DIR/deployed_code diporto@diporto.undertide.co:/home/diporto
ssh diporto@diporto.undertide.co cp /home/diporto/appsettings.Production.json /home/diporto/deployed_code/
ssh diporto@diporto.undertide.co sudo systemctl restart diporto
