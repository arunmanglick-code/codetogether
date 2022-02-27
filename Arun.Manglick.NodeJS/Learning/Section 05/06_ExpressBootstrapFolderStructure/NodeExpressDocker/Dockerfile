FROM node:slim

# Create App Directory
RUN MKDIR -p /usr/src/app
WORKDIR /usr/src/app

# Install Node.JS app dependencies
COPY package.json /usr/src/app
RUN CD /usr/src/app
RUN npm Install
RUN node --version

# Bundle App Source
COPY . /usr/src/app

EXPOSE 300
CMD ["npm", "start"]
