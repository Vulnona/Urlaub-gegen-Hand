As a user/tester,

I want to test/ run the application through Docker Desktop:

There are four containers in the Docker desktop:

Frontend -To run frontend
Backend -To run Backend
Migration -To successfully run the migrations
Webserver
db_1 -To ensure that the DB is in healthy state
Steps to run Application on Docker Desktop: Step 1: Open Docker Desktop Step 2: Click on "Contaniers" from the left menu to remove all Containers. Click "Select All" and then "Delete All" to delete all the Containers Step 3: Navigate to "Images" from the left menu to remove all Images. Click "Select All" on the top left and then "Delete All" to delete all the Images Step 4: Navigate to "Volumes" from the left menu to remove all the existing Volumes. Click on "Select All" on the top left and then "Delete All" to delete all the Images Step 5: Clone branch from GitHub using command "git clone -b branch_name git_url" Step 6: Navigate to Project directory, and open "Command Prompt" and write command "docker-compose up --build" to build images, containers and volumes in your Docker Desktop

If in case, the error persists while creating containers use "CTRL+C" to stop the docker to make images, containers and volumes. Once done, try running "docker-compose up --build" command again.

After the docker is up, Check clicking on the URLs formed in Docker Desktop, directly click on the URL and the application would start working:

Frontend: http://localhost:3000
Backend: http://localhost:8080/swagger/index.html
Als Benutzer/Tester

Ich möchte die Anwendung über Docker Desktop testen/ausführen:

Im Docker-Desktop gibt es vier Container:

Frontend – Zum Ausführen des Frontends
Backend – Zum Ausführen des Backends
Migration – Um die Migrationen erfolgreich durchzuführen
Webserver
db_1 – Um sicherzustellen
Dass sich die Datenbank in einem fehlerfreien Zustand befindet

Schritte zum Ausführen der Anwendung auf Docker Desktop: Schritt 1: Öffnen Sie Docker Desktop Schritt 2: Klicken Sie in der Baumstruktur auf "Containers", entfernen Sie alle Container (klicken Sie auf "Select All“ und dann auf "Delete All", um alle Container zu löschen). Schritt 3: Navigieren Sie in der Baumstruktur zu "Images", entfernen Sie alle Bilder (klicken Sie oben links auf "Select All“ und dann auf "Delete All", um alle Bilder zu löschen). Schritt 4: Navigieren Sie in der Baumstruktur zu "Volumes", entfernen Sie alle vorhandenen Volumes (klicken Sie oben links auf "Select All" und dann auf "Delete All", um alle Bilder zu löschen). Schritt 5: Klonen Sie den Zweig von GitHub mit dem Befehl "git clone -b branch_name git_url“ Schritt 6: Navigieren Sie zum Projektverzeichnis, öffnen Sie die "Command Prompt“ und schreiben Sie den Befehl "docker-compose up --build“, um Images, Container und Volumes in Ihrem Docker-Desktop zu erstellen

Falls der Fehler weiterhin besteht, verwenden Sie "CTRL+C“, um den Docker anzuhalten, um Bilder, Container und Volumes zu erstellen. Wenn Sie fertig sind, versuchen Sie erneut, den Befehl "docker-compose up --build“ auszuführen.

Nachdem der Docker hochgefahren ist, Klicken Sie auf die in Docker Desktop erstellten URLs, klicken Sie direkt auf die URL und die Anwendung beginnt zu funktionieren:

Frontend: http://localhost:3000
Backend: http://localhost:8080/swagger/index.html
