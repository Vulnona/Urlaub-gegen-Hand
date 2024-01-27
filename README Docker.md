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