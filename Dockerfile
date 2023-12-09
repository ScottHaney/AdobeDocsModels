FROM ubuntu:14.04.4

RUN apt-get update
RUN apt-get install -y python3 python3-pip

WORKDIR /docs

# Install pip requirements
COPY after-effects-scripting-guide-master /docs
RUN python3 -m pip install -r requirements.txt; exit 0
