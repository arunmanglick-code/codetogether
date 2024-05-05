Note: What is Logback:

A Guide To Logback | https://www.baeldung.com/logback
Configuring Logback with Spring Boot | https://www.codingame.com/playgrounds/4497/configuring-logback-with-spring-boot
About Appenders | https://logback.qos.ch/manual/appenders.html#TimeBasedRollingPolicy

Notations:https://www.codingame.com/playgrounds/4497/configuring-logback-with-spring-boot
%d - outputs the time which the log message occurred in formats that SimpleDateFormat allows.
%thread - outputs the name of the thread that the log message occurred in.
$-5level - outputs the logging level of the log message.
%logger{36} - outputs the package + class name the log message occurred in. The number inside the brackets represents the maximum length of the package + class name. If the output is longer than the specified length it will take a substring of the first character of each individual package starting from the root package until the output is below the maximum length. The class name will never be reduced. A nice diagram of this can be found in the Conversion word docs.
%M - outputs the name of the method that the log message occurred in (apparently this is quite slow to use and not recommended unless your not worried about performance or the method name is particularly important to you).
%msg - outputs the actual log message.
%n - line break
%magenta() - sets the colour of the output contained in the brackets to magenta (other colours are available).
highlight() - sets the colour of the output contained in the brackets to the depending on the logging level (for example ERROR = red).


Appenders: https://logback.qos.ch/manual/appenders.html#TimeBasedRollingPolicy
fileNamePattern: The mandatory fileNamePattern property defines the name of the rolled-over (archived) log files. Its value should consist of the name of the file, plus a suitably placed %d conversion specifier. The %d conversion specifier may contain a date-and-time pattern as specified by the java.text.SimpleDateFormat class. If the date-and-time pattern is omitted, then the default pattern yyyy-MM-dd is assumed
maxFileSize: (Controls Active Folder/Files)Each time the current log file reaches maxFileSize before the current time period ends, it will be archived with an increasing index, starting at 0.
maxHistory : (Controls Archived Folder/Files) The optional maxHistory property controls the maximum number of archive files to keep, asynchronously deleting older files.
totalSizeCap : (Controls Archived Folder/Files)The optional totalSizeCap property controls the total size of all archive files. Oldest archives are deleted asynchronously when the total size cap is exceeded. The totalSizeCap property requires maxHistory property to be set as well.