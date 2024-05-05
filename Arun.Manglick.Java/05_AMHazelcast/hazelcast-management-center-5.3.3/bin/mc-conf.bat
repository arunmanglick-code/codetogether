@ECHO OFF

SET SCRIPT_DIR=%~dp0

if "x%JAVA_HOME%" == "x" (
    set RUN_JAVA=java
) else (
    set "RUN_JAVA=%JAVA_HOME%\bin\java"
)

"%RUN_JAVA%" %JAVA_OPTS% -Dloader.main=com.hazelcast.webmonitor.cli.MCConfCommandLine -cp %SCRIPT_DIR%\..\hazelcast-management-center-5.3.3.jar org.springframework.boot.loader.PropertiesLauncher %*
