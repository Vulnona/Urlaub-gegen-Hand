// Node.js script to extract MySQL credentials and resolve the real secret file path
// Usage: node get-mysql-creds.js <path-to-compose.yaml>
const fs = require('fs');
const path = require('path');
const yaml = require('js-yaml');

if (process.argv.length < 3) {
  console.error('Usage: node get-mysql-creds.js <path-to-compose.yaml>');
  process.exit(1);
}

const composePath = process.argv[2];
let doc;
try {
  doc = yaml.load(fs.readFileSync(composePath, 'utf8'));
} catch (e) {
  console.error('Could not read or parse compose.yaml:', e.message);
  process.exit(1);
}

const dbService = doc.services && (doc.services.db || doc.services[Object.keys(doc.services).find(k => k.toLowerCase() === 'db')]);
if (!dbService || !dbService.environment) {
  console.error('No db service or environment found in compose.yaml');
  process.exit(1);
}

const env = dbService.environment;
function getEnv(key) {
  if (typeof env === 'object' && !Array.isArray(env)) {
    return env[key] || env[key.toUpperCase()] || env[key.toLowerCase()];
  } else if (Array.isArray(env)) {
    const found = env.find(e => e.startsWith(key + '=') || e.startsWith(key.toUpperCase() + '='));
    return found ? found.split('=')[1] : undefined;
  }
  return undefined;
}

const user = getEnv('MYSQL_USER');
const db = getEnv('MYSQL_DATABASE');
const pwFile = getEnv('MYSQL_PASSWORD_FILE');

if (!user || !db || !pwFile) {
  console.error('MYSQL_USER, MYSQL_DATABASE or MYSQL_PASSWORD_FILE missing in db service');
  process.exit(1);
}

// Find the real secret file path from the secrets section
let secretName = path.basename(pwFile);
if (secretName.startsWith('db__')) secretName = secretName.replace('db__', '');
const secrets = doc.secrets || {};
let secretFile = null;
for (const [key, val] of Object.entries(secrets)) {
  if (pwFile.endsWith(key) && val.file) {
    secretFile = val.file;
    break;
  }
}
if (!secretFile) {
  // fallback: try to guess
  secretFile = `.docker/db/secrets/.${secretName.replace('_', '-')}.txt`;
}
console.log(JSON.stringify({ user, db, pwFile: secretFile }));
