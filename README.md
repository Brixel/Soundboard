# Soundboard
Upload a mp3 file and send it over mqtt to be played on a raspberry pi.

Projet is more of a proof of concept, showing you can actually send large files (up to 260MB) over MQTT.

## Setup
### Client
Written in Blazor Webassembly. Runs in .NET Core 3.1. File uploads via multipart/form-data to the API
### Server
.NET Core 3.1 API that relays the bytes to an MQTT topic. MQTT Can be used with client credentials.
### MQTT
MQTT Broker that has a specific topic where the data is posted to.
