

import json
import os
import mysql.connector
import socket
import time
import sys
from dotenv import load_dotenv
import base64
from selenium.common.exceptions import NoSuchElementException, TimeoutException
from selenium.webdriver.chrome.service import Service as ChromeService
from selenium.webdriver.chrome.webdriver import WebDriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.chrome.options import Options

# Load the appsettings.json
with open('appsettings.json') as config_file:
    config = json.load(config_file)

facebook_config = config.get("Facebook", {})
EMAIL = facebook_config.get("EMAIL")
PASS = facebook_config.get("PASS")
MAIN_GROUP_ID = facebook_config.get("MAIN_GROUP_ID")

if not EMAIL or not PASS or not MAIN_GROUP_ID:
    print("Configuration variables EMAIL, PASS, and MAIN_GROUP_ID are required.")
    sys.exit(1)

db_config = config.get("ConnectionStrings", {}).get("DefaultConnection")
if not db_config:
    print("Database configuration is required.")
    sys.exit(1)

# Parse the database connection string
db_params = dict(x.split('=') for x in db_config.split(';') if x)

try:
    connection = mysql.connector.connect(
        database=db_params.get("Database"),
        user=db_params.get("User"),
        password=db_params.get("Password"),
        host=db_params.get("Server")
    )
    print("Connected to MySQL database successfully!")
    cursor = connection.cursor()
    cursor.execute("SELECT id, title, description, imageData, location, accomodation, accomodationsuitable, skills FROM offers WHERE fb_status='pending' LIMIT 1")
    offers = cursor.fetchall()
except mysql.connector.Error as err:
    print(f"Error connecting to MySQL database: {err}")
    sys.exit(1)

try:
    socket.create_connection(("localhost", 9515), timeout=5)
except (socket.timeout, socket.error):
    print("Selenium standalone server not running. Please start the server and try again.")
    sys.exit(1)

options = Options()
options.add_argument("--disable-notifications")
options.add_argument("--ignore-ssl-errors=yes")
options.add_argument("--ignore-certificate-errors")

driver = WebDriver(
    service=ChromeService(executable_path="/home/azureuser/migration-fix/chromedriver.exe"), ##Ändern Sie den Pfad gemäß dem angegebenen Ordner
    options=options
)

print("Logging in to Facebook")
driver.get("https://mbasic.facebook.com/login.php")

try:
    username = WebDriverWait(driver, 20).until(
        EC.element_to_be_clickable((By.NAME, 'email'))
    )
    password = WebDriverWait(driver, 20).until(
        EC.element_to_be_clickable((By.NAME, 'pass'))
    )
    username.send_keys(EMAIL)
    password.send_keys(PASS)
    driver.find_element(By.NAME, "login").click()
    print("Login completed")

    print("Navigating to the Groups page")
    main_group_link = f"https://mbasic.facebook.com/groups/{MAIN_GROUP_ID}/"
    driver.get(main_group_link)
    print("Group page accessed")
    print(f"Page title: {driver.title}")

    for offer in offers:
        try:
            offer_id, title, description, imageData, location, accommodation, accommodationSuitable, skills = offer
            if imageData:
                base64_image = base64.b64encode(imageData).decode('utf-8')
                print(f"\tBase64 representation: {base64_image[:50]}...")
                image_data = base64.b64decode(base64_image)
                file_path = r"C:\users\Downloads\fb_upload.jpg" ## edit path to store images (Pfad zum Speichern von Bildern bearbeiten)
                with open(file_path, "wb") as file:
                    file.write(image_data)
                print(f"Image saved to {file_path}")

            print("Clicking on the post field")
            post_field = WebDriverWait(driver, 20).until(
                EC.element_to_be_clickable((By.NAME, 'xc_message'))
            )
            post_field.click()
            time.sleep(2)

            print("Typing in the post field")
            post_content = (
                f"Offer Title: {title}\n"
                f"Description: {description}\n"
                f"Location: {location}\n"
                f"Amenities: {accommodation}\n"
                f"Accommodation Suitability: {accommodationSuitable}\n"
                f"skills: {skills}\n"
            )
            post_field.send_keys(post_content)

            if imageData:
                print("Adding photo")
                add_photo = WebDriverWait(driver, 20).until(
                    EC.element_to_be_clickable((By.NAME, 'view_photo'))
                )
                add_photo.click()
                time.sleep(2)

                print("Uploading photo")
                choose_file = WebDriverWait(driver, 20).until(
                    EC.presence_of_element_located((By.NAME, 'file1'))
                )
                choose_file.send_keys(file_path)
                time.sleep(2)

                print("Submitting photo")
                upload_photo_button = WebDriverWait(driver, 20).until(
                    EC.element_to_be_clickable((By.NAME, 'add_photo_done'))
                )
                upload_photo_button.click()
                time.sleep(2)

            print("Submitting post")
            submit_post_button = WebDriverWait(driver, 20).until(
                EC.element_to_be_clickable((By.NAME, 'view_post'))
            )
            submit_post_button.click()
            print("Post submitted successfully")

            if imageData:
                os.remove(file_path)
                print(f"Image {file_path} deleted")

            try:
                cursor.execute("UPDATE offers SET fb_status='posted' WHERE id=%s", (offer_id,))
                connection.commit()
                print(f"fb_status updated to 'posted' for offer ID: {offer_id}")
            except mysql.connector.Error as err:
                print(f"Error updating fb_status for offer ID {offer_id}: {err}")

            time.sleep(5)
        except NoSuchElementException as e:
            print(f"No such element found: {e}")
        except TimeoutException as e:
            print(f"Timeout occurred: {e}") 
        except Exception as e:
            print(f"An error occurred while posting: {e}")
finally:
    print("Stopping the driver")
    time.sleep(5) 
    driver.quit()
    connection.close()


