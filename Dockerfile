# For more information, please refer to https://aka.ms/vscode-docker-python
FROM ubuntu:14.04.4

RUN apt-get update
RUN apt-get install -y python3 python3-pip

WORKDIR /docs

# Install pip requirements
COPY after-effects-scripting-guide-master /docs
RUN python3 -m pip install -r requirements.txt; exit 0

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-python-configure-containers
#RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
#USER appuser
