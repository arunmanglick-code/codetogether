@ECHO OFF
echo Warning: start.bat is deprecated. Please use mc-start.cmd instead.
cd %~dp0 && cd ..

set argC=0
for %%x in (%*) do Set /A argC+=1
set help=false

if %argC% gtr 3 set help=true
if %argC% equ 1 (
    if "%1" == "--help" (
        set help=true
    )
)

if %help% == true (
    echo usage: start.bat
    echo usage: start.bat [port]
    echo usage: start.bat [port] [path]
    echo usage: start.bat [port] [path] [classpath]
    exit /b
)

if %argC% == 3 (
    SET MC_CLASSPATH=%3
    SET MC_CLASSPATH=%MC_CLASSPATH:;=,%
    java %JAVA_OPTS% -Dhazelcast.mc.http.port=%1 -Dhazelcast.mc.contextPath=%2 -Dloader.path=%MC_CLASSPATH% -cp "hazelcast-management-center-5.3.3.jar" org.springframework.boot.loader.PropertiesLauncher
)

if %argC% == 2 (
    java %JAVA_OPTS% -Dhazelcast.mc.http.port=%1 -Dhazelcast.mc.contextPath=%2 -cp "hazelcast-management-center-5.3.3.jar" org.springframework.boot.loader.PropertiesLauncher
)

if %argC% == 1 (
    java %JAVA_OPTS% -Dhazelcast.mc.http.port=%1 -cp "hazelcast-management-center-5.3.3.jar" org.springframework.boot.loader.PropertiesLauncher
)

if %argC% == 0 (
    java %JAVA_OPTS% -cp "hazelcast-management-center-5.3.3.jar" org.springframework.boot.loader.PropertiesLauncher
)
