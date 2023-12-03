FROM nvidia/cuda:11.7.1-devel-ubuntu22.04

RUN apt update \
    && apt install -y dotnet-sdk-6.0 nano wget aria2 \
    && apt clean \
    && rm -rf /var/lib/apt/lists/*

