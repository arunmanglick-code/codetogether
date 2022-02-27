Here this POC talks about - 

Setting up Monitoring system
     - How Telegraf InfluxDB & Grafana work together to push your system health metrics and show over Grafana
     = To do this there are two approaches:
        - No Docker:
            - Install Each Individually and tie them to work together-  Telegraf InfluxDB & Grafana
                -	Telegraf Installation: https://docs.influxdata.com/telegraf/v1.19/introduction/installation/ 
                -	InfluxDb Installation: https://docs.influxdata.com/influxdb/v2.1/install/
                -	Grafana Download: https://grafana.com/grafana/download?platform=windows  
        - Docker:
            - Here achieve same results, but fast, by using the docker images of each 
            - Then use docker compose - For defining and running multi-container Docker applications
        
    - Details of this POC   
        - Setup Telegraf, InfluxDB and Grafana using Docker and Docker Compose
        - Configure Telegraf using telegraf.conf to push data to InfluxDB
        - Configure InfluxDB as DataSource for Grafana
        - Configure Grafana to create Dashboard showing metrics

# Details about - Telegraf InfluxDB and Grafana

Telegraf: 
    - Is an agent that is in charge of collecting, processing, aggregating and sending metrics to Influxdb
    - Configurations : 
        - https://github.com/influxdata/telegraf/blob/master/etc/telegraf.conf
        - https://docs.influxdata.com/telegraf/v1.21/administration/configuration/      
            
Influxdb:
    - Open-source Time Series database
	- Metrics sent from Telegraf are stored in InfluxDB
	- Written in the Go programming language 

Grafana:
    - Open-source Data Visualization and Monitoring Suite
    - Allows Queryitng 
    - Here you create Panels, Dashboard using data/metrics

docker compose: https://docs.docker.com/compose/
    - Compose is a tool for defining and running multi-container Docker applications. 
    - With Compose, you use a YAML file to configure your application’s services. 
    - Then, with a single command, you create and start all the services from your configuration
    - Three Steps:
        - Define app
        - Define your app’s in a Dockerfile so it can be reproduced anywhere.
        - Define the services that make up your app in docker-compose.yml so they can be run together in an isolated environment.
        - Run 'docker compose up' and the Docker compose command starts and runs your entire app. 
        - You can alternatively run 'docker-compose up' using the docker-compose binary.

Docker Commands Used:
    - docker -v (check version)
    - docker ps (list dockers)
    - docker image ls (to list local images)
    - docker-compose up (To build and run your app with Compose)
    - Browse: http://192.168.1.55:3000/login
        - Username: admin
        - Pwd: admin
        - Set New Username/Pwd  
            - Username: nessdemo
            - Pwd: nessdemo   
    - docker-compose down (To stop the running app)


Connect to Grafana:
 - After this command is completed: docker-compose up 
 - Browse: http://192.168.1.55:3000/login
    - Username: admin
    - Pwd: admin
    - Set New Username/Pwd  
        - Username: nessdemo
        - Pwd: nessdemo   
 - Add DataSource as InfluxDB
    - URL: http://http://192.168.1.55:8086
    - Database Name: influx
    - Username: admin
    - Pwd: admin
    



