import requests
from pyfacebook import GraphAPI

app_id = None
app_secret =  None
access_token = None
group_id = None
page_id = None


msg="dies ist ein kleiner Test :)" ###Hier später die Datenbank einfügen, aktuell noch nicht möglich da Db fehlt

####Prüfen, auf welcher eigenen ("me") Seite wir uns befinden - idealerweise der Facebook-Page auf der auch gepostet werden soll
url = f'https://graph.facebook.com/v18.0/me?fields=id,name&access_token={access_token}'

# Senden der Anfrage
response = requests.get(url)

# Überprüfen Sie, ob die Anfrage erfolgreich war
if response.status_code == 200:
    # Ergebnis
    print("Erfolgreich: ",response.json())
else:
    # Wenn Anfrage fehlschlägt
    print('Fehler bei der Anfrage:', response.status_code)
    print('Antwort:', response.text)

### Post auf der Page
resp = requests.post(
        f"https://graph.facebook.com/me/feed?message={msg}&access_token={access_token}",
        timeout=10,
    )
print(f"Posted on FB {resp.content.decode('utf-8')}")
