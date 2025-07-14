#!/bin/sh

cd "$(dirname "$0")" && cd ..

#Management Center Docker container passes JAVA_OPTS_DEFAULT
if [ -n "${JAVA_OPTS}" ]; then
    export JAVA_OPTS="${JAVA_OPTS_DEFAULT} ${JAVA_OPTS}"
else
    export JAVA_OPTS="${JAVA_OPTS_DEFAULT}"
fi

java $JAVA_OPTS -Dloader.main=com.hazelcast.webmonitor.cli.MCConfCommandLine -cp hazelcast-management-center-5.3.3.jar org.springframework.boot.loader.PropertiesLauncher "$@"
