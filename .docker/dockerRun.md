As a user/tester, 

I want to test/ run the application through Docker Desktop:

There are four containers in the Docker desktop:
1. Frontend     -To run frontend
2. Backend      -To run Backend
3. Migration    -To successfully run the migrations
4. Webserver
5. db_1         -To ensure that the DB is in healthy state

Steps to run Application on Docker Desktop:
Step 1: Open Docker Desktop 
Step 2: Click on "Contaniers" from the left menu to remove all Containers. Click "Select All" and then "Delete All" to delete all the Containers
Step 3: Navigate to "Images" from the left menu to remove all Images. Click "Select All" on the top left and then "Delete All" to delete all the Images
Step 4: Navigate to "Volumes" from the left menu to remove all the existing Volumes. Click on "Select All" on the top left and then "Delete All" to delete all the Images
Step 5: Clone branch from GitHub using command "git clone -b branch_name git_url"
Step 6: Navigate to Project directory, and open "Command Prompt" and write command "docker-compose up --build" to build images, containers and volumes in your Docker Desktop

If in case, the error persists while creating containers use "CTRL+C" to stop the docker to make images, containers and volumes. Once done, try running "docker-compose up --build" command again. 