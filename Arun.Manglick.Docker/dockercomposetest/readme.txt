Ref: https://docs.docker.com/compose/gettingstarted/

Details about Dockerfile: 
  - Build an image starting with the Python 3.7 image.
  - Set the working directory to /code.
  - Set environment variables used by the flask command.
  - Install gcc and other dependencies
  - Copy requirements.txt and install the Python dependencies.
  - Add metadata to the image to describe that the container is listening on port 5000
  - Copy the current directory . in the project to the workdir . in the image.
  - Set the default command for the container to flask run

Details about Compose File: (docker-compose.yml)
  - This Compose file defines two services: web and redis
  - The web service uses an image that’s built from the Dockerfile in the current directory. 
    It then binds the container and the host machine to the exposed port, 8000. 
    This example service uses the default port for the Flask web server, 5000.

  - The redis service uses a public Redis image pulled from the Docker Hub registry

