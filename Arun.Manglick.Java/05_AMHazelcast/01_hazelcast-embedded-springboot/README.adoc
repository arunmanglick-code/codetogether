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
    - hazelcast.yaml - Hazelcast configuration (hazelcast.yaml) is placed in the src/main/resources/ directory.

 - Open Command Prompt
    - Build Package: mvn package
    - This output .jar file in target folder at rool level
    - Run this application at two ports
        - java -Dserver.port=8081  -jar target\hazelcast-embedded-springboot-0.1.jar
        - java -Dserver.port=8082  -jar target\hazelcast-embedded-springboot-0.1.jar

 - Check Hazelcast Cluster:
    - After both application instances are initialized, you should see that the Hazelcast cluster is formed:
        Members {size:2, ver:2} [
            Member [192.168.1.64]:5701 - 520aec3f-58a6-4fcb-a3c7-498dcf37d8ff
            Member [192.168.1.64]:5702 - 5c03e467-d457-4847-b49a-745a335db557 this
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