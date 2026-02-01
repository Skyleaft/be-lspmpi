@echo off
set IMAGE_NAME=be-lspmpi
set TAG=latest
set REGISTRY=11.1.1.4:5000

echo ===============================
echo Build Docker Image
echo ===============================
docker build -t %IMAGE_NAME%:%TAG% .
if errorlevel 1 goto error

echo ===============================
echo Tag Image for Registry
echo ===============================
docker tag %IMAGE_NAME%:%TAG% %REGISTRY%/%IMAGE_NAME%:%TAG%
if errorlevel 1 goto error

echo ===============================
echo Login to Registry
echo ===============================
docker login %REGISTRY%
if errorlevel 1 goto error

echo ===============================
echo Push Image to Registry
echo ===============================
docker push %REGISTRY%/%IMAGE_NAME%:%TAG%
if errorlevel 1 goto error

echo ===============================
echo DONE ✅ Image pushed successfully
echo ===============================
pause
exit /b 0

:error
echo ❌ ERROR occurred, process stopped
pause
exit /b 1