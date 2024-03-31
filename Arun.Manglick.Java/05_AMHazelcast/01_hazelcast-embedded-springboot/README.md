See the link:https://docs.hazelcast.com/tutorials/hazelcast-embedded-springboot[tutorial].

Code Ref: https://github.com/hazelcast-guides/hazelcast-embedded-springboot
Documentation: https://docs.hazelcast.com/tutorials/hazelcast-embedded-springboot

How to:
 - Download code from the above given repo
    - HazelcastApplication - This is the main method
    - HazelController
        - Auto-wire the HazelcastInstance bean in the CommandController
        - Defines ConcurrentMap
        - Add and Get Method to Cache (ConcurrentMap)
    - HazelResponse
    - hazelcast.yaml - Hazelcast configuration (hazelcast.yaml) is placed in the src/main/resources/ directory
      - Define Cluster Name:
        - hazelcast:
          cluster-name: am-hazelcast-cluster

 - Open Command Prompt
    - Build Package: mvn package
    - This output .jar file in target folder at rool level
    - Run this application at two ports
        - java -Dserver.port=8081  -jar target\hazelcast-embedded-springboot-0.1.jar
        - java -Dserver.port=8082  -jar target\hazelcast-embedded-springboot-0.1.jar

 - Check Hazelcast Cluster:
    - After both application instances are initialized, you should see that the Hazelcast cluster is formed:
          Members {size:2, ver:2} [
              Member [192.168.1.72]:5701 - f36eca4a-935f-488e-9839-fa232bfd7a64
              Member [192.168.1.72]:5702 - 89004f20-e2bc-4f45-9dda-04c71f73f2c4 this
          ]

  - Testing:
       - Go to Postman
            - Add Value in Hazelcast
                    curl --location 'localhost:8081/addValue' \
                    --header 'Content-Type: application/x-www-form-urlencoded' \
                    --data-urlencode 'key=key1' \
                    --data-urlencode 'value=amhazelcast'
            - Get Value from Hazelcast (Port: 8081)
                curl --location 'localhost:8081/getValue?key=key1'
                Output: {
                            "value": "amhazelcast"
                        }
            - Get Value from Hazelcast (Port: 8082)
                curl --location 'localhost:8082/getValue?key=key1'
                Output: {
                            "value": "amhazelcast"
                        }

  - Hazelcast Management Center
    - Download (V5.3.3) https://hazelcast.com/get-started/download/
    - Unzip
    - Command Prompt: hazelcast-management-center-5.3.3\bin\start.bat
      - Output: Hazelcast Management Center successfully started at http://localhost:8080/
    - Browse: http://localhost:8080/
      - It'll take you to: http://localhost:8080/cluster-connections
      - Press Add Cluster
        - ClusterName: Same as defined in hazelcast.yaml (which is 'am-hazelcast-cluster')
        - Member Addresses: Seperate addresses with commas: 127.0.0.1:5701, 127.0.0.1:5702, ...
          - For this example: 192.168.1.72:5701, 192.168.1.72:5702
      - Check Cluster-> Members (You'll find two members here)
      - Check Storage-> Maps (You'll find items added in Hazelcast ConcurrentMap)
      - 
