<template>
  <PublicNav />
  <Navbar />
 <div class="container">          
            <h2>Datenschutzerklärung</h2>
            <p>
<h3> 1. Verantwortlicher </h3>
<ul>
<li>Der Verantwortliche im Sinne der Datenschutz-Grundverordnung (DSGVO) ist:
</li>
</ul>
<div style="font-size: 18px;padding: 5px;width: 300px; border:2px solid #000">
ALRECO GmbH
<br>Musterstraße 1
<br>12345 Musterstadt
<br>Deutschland
<br>E-Mail: <a href="mailto:datenschutz@alreco.de">datenschutz@alreco.de</a>
</div>
<br>
<h3> 2. Erhebung und Verarbeitung von personenbezogenen Daten </h3>
<h4>2.1. Registrierung und Verifizierung</h4>
 <ul>
<li> Bei der Registrierung auf unserer Plattform werden folgende personenbezogene Daten erhoben: Name, E-Mail-Adresse, Geburtsdatum und Angaben zur Identitätsprüfung (z.B. Ausweisdokumente). </li>
<li> Diese Daten werden zur Einrichtung Ihres Benutzerkontos, zur Verifizierung Ihrer Identität und zur Sicherstellung der Plattformintegrität verwendet. </li>
  </ul>
<h4>2.2. Nutzung der Plattform</h4>
<ul>
<li> Bei der Nutzung unserer Plattform erheben wir Daten zu Ihrem Profil, den von Ihnen eingestellten Angeboten und Gesuchen sowie zu den von Ihnen verfassten Bewertungen und Feedbacks. </li>
<li> Diese Daten werden verarbeitet, um die Funktionalität der Plattform zu gewährleisten und eine positive Nutzererfahrung zu bieten. </li>
</ul>
<h4>2.3. Spiegelung in der Facebook-Gruppe</h4>
<ul>
<li> Alle auf ALRECO eingestellten Angebote und Gesuche werden automatisch auch in der [Facebook-Gruppe „Urlaub gegen Hand“](https://www.facebook.com/groups/urlaub.gegen.hand) gespiegelt.</li>
<li> Für die Spiegelung verwenden wir die Daten aus Ihren Angebots- und Gesuchsanzeigen. </li>
</ul>
    <h3> 3. Rechtsgrundlage der Verarbeitung </h3>
<ul> <li>Die Verarbeitung Ihrer personenbezogenen Daten erfolgt auf Grundlage von Art. 6 Abs. 1 lit. b DSGVO, soweit dies für die Erfüllung des Nutzungsvertrags mit Ihnen erforderlich ist. Weiterhin erfolgt die Verarbeitung auf Grundlage Ihrer Einwilligung gemäß Art. 6 Abs. 1 lit. a DSGVO, insbesondere für die Verifizierung Ihrer Identität.</li></ul>
<h3> 4. Dauer der Speicherung </h3>
<ul><li>Ihre personenbezogenen Daten werden nur so lange gespeichert, wie dies für die Durchführung der Dienste und die Erfüllung unserer vertraglichen Pflichten erforderlich ist. Darüber hinaus speichern wir Ihre Daten nur im Rahmen gesetzlicher Aufbewahrungspflichten.</li></ul>
<h3> 5. Datensicherheit </h3>
<ul><li>Wir haben umfangreiche technische und organisatorische Maßnahmen getroffen, um Ihre personenbezogenen Daten vor Verlust, Manipulation und unberechtigtem Zugriff zu schützen. Unsere Sicherheitsmaßnahmen werden regelmäßig überprüft und entsprechend dem technologischen Fortschritt angepasst.</li></ul>
<h3> 6. Weitergabe von Daten an Dritte </h3>
<ul><li>Eine Weitergabe Ihrer personenbezogenen Daten an Dritte erfolgt nur, wenn dies für die Erfüllung unserer vertraglichen Pflichten erforderlich ist, Sie ausdrücklich eingewilligt haben oder wir aufgrund gesetzlicher Bestimmungen dazu verpflichtet sind.</li></ul>
<h3> 7. Ihre Rechte </h3>
<h4>Sie haben das Recht:</h4>
<ul>
<li> Auskunft über Ihre von uns verarbeiteten personenbezogenen Daten zu verlangen (Art. 15 DSGVO) </li>
<li> Berichtigung unrichtiger oder Vervollständigung Ihrer bei uns gespeicherten personenbezogenen Daten zu verlangen (Art. 16 DSGVO) </li>
<li> Löschung Ihrer bei uns gespeicherten personenbezogenen Daten zu verlangen (Art. 17 DSGVO) </li>
<li> Einschränkung der Verarbeitung Ihrer personenbezogenen Daten zu verlangen (Art. 18 DSGVO) </li>
<li> Widerspruch gegen die Verarbeitung Ihrer personenbezogenen Daten einzulegen (Art. 21 DSGVO) </li>
<li> Ihre personenbezogenen Daten in einem strukturierten, gängigen und maschinenlesbaren Format zu erhalten oder die Übermittlung an einen anderen Verantwortlichen zu verlangen (Art. 20 DSGVO). </li></ul>
<h3> 8. Widerruf Ihrer Einwilligung </h3>
<ul><li>Sofern Sie eine Einwilligung zur Verarbeitung Ihrer personenbezogenen Daten erteilt haben, können Sie diese jederzeit mit Wirkung für die Zukunft widerrufen. Der Widerruf ist per E-Mail an [datenschutz@alreco.de](mailto:datenschutz@alreco.de) zu richten. </li></ul>
<h3> 9. Änderungen dieser Datenschutzerklärung </h3>
<ul><li>Wir behalten uns das Recht vor, diese Datenschutzerklärung jederzeit zu ändern. Änderungen werden auf der Website veröffentlicht und treten mit ihrer Veröffentlichung in Kraft.</li></ul>
<h4>Durch die Nutzung von ALRECO erklären Sie sich mit dieser Datenschutzerklärung einverstanden. Bei Fragen oder Anliegen wenden Sie sich bitte an unseren Datenschutzbeauftragten unter [datenschutz@alreco.de](mailto:datenschutz@alreco.de).</h4>
            </p>
    </div>
</template>

<script>
import Navbar from '@/components/navbar/Navbar.vue';
import axiosInstance from '@/interceptor/interceptor';
import PublicNav from '@/components/navbar/PublicNav.vue';
import isActiveMembership from '@/services/CheckActiveMembership';
import getLoggedUserId from '@/services/LoggedInUserId';
import Swal from "sweetalert2";
import CheckUserRole from "@/services/CheckUserRole";
import router from "@/router";
import toast from '@/components/toaster/toast';
import debounce from 'lodash/debounce';

export default {
  components: {
    Navbar,
    PublicNav
  },
};
</script>