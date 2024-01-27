## Setup and Start Docker
Docker builden
docker build -t ugh .

Docker starten
docker run -d -p 8000:80 ugh

ggf. Logs prüfen
docker logs [containername]

lokaler Aufruf
http://localhost:8000/

Inkl "allem":
docker-compose up -d --build

Herunterfahren der Container:
docker-compose down

## Troubleshooting
Windows User:  
If you get an error `ERROR: error during connect: in the default daemon configuration on Windows, the docker client must be run with elevated privileges to connect: ... The system cannot find the file specified.`  

make sure Docker Desktop is running.  
Proof this by checking your status in Docker Desktop's left bottom corner, should display
> Engine running
