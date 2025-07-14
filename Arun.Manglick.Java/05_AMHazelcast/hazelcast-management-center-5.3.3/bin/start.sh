#!/bin/sh
echo Warning: start.sh is deprecated. Please use "hz-mc start" instead.
cd "$(dirname "$0")" && cd ..

if [ "$1" = "--help" ] || [ $# -gt 3 ] ; then
    echo "usage: start.sh"
    echo "usage: start.sh [port]"
    echo "usage: start.sh [port] [context path]"
    echo "usage: start.sh [port] [context path] [classpath]"
    exit;
fi

if [ $# -eq 3 ] ; then
    # shellcheck disable=SC2154
    exec java $JAVA_OPTS "-Dhazelcast.mc.http.port=$1" "-Dhazelcast.mc.contextPath=$2" \
      "-Dloader.path=${3//:/,},./bin/user-lib/*" \
      -cp "hazelcast-management-center-5.3.3.jar" \
      org.springframework.boot.loader.PropertiesLauncher
elif [ $# -eq 2 ] ; then
    exec java $JAVA_OPTS "-Dhazelcast.mc.http.port=$1" "-Dhazelcast.mc.contextPath=$2" \
      "-Dloader.path=./bin/user-lib/*" \
      -cp "hazelcast-management-center-5.3.3.jar" \
      org.springframework.boot.loader.PropertiesLauncher
elif [ $# -eq 1 ] ; then
    exec java $JAVA_OPTS "-Dhazelcast.mc.http.port=$1" \
      "-Dloader.path=./bin/user-lib/*" \
      -cp "hazelcast-management-center-5.3.3.jar" \
      org.springframework.boot.loader.PropertiesLauncher
else
    exec java $JAVA_OPTS \
      "-Dloader.path=./bin/user-lib/*" \
      -cp "hazelcast-management-center-5.3.3.jar" \
      org.springframework.boot.loader.PropertiesLauncher
fi
