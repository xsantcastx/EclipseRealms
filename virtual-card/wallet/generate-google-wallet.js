/*
 * Generate branded "Add to Google Wallet" passes for the Creation Design & Events
 * digital business cards (Angela Triana + Carlos Triana).
 *
 * It (1) creates/updates one Generic pass CLASS, (2) creates/updates one pass
 * OBJECT per person, and (3) writes signed "Save to Google Wallet" links into
 * ../google-links.js so the "Add to Google Wallet" button on each card lights up.
 *
 * You only run this ONCE (re-run to update the pass design). See ./README.md.
 *
 * Required before running:
 *   - A Google Wallet API Issuer account  -> set GOOGLE_WALLET_ISSUER_ID
 *   - A service-account key JSON with the "Wallet Object Issuer" role, saved as
 *     wallet/service-account.json  (or point GOOGLE_APPLICATION_CREDENTIALS at it)
 *
 * Usage:
 *   cd virtual-card/wallet
 *   npm install
 *   GOOGLE_WALLET_ISSUER_ID=3388000000000000000 npm run generate
 */

const fs = require('fs');
const path = require('path');
const jwt = require('jsonwebtoken');
const { GoogleAuth } = require('google-auth-library');

// ---- Config -------------------------------------------------------------
const ISSUER_ID = process.env.GOOGLE_WALLET_ISSUER_ID;
const KEY_FILE =
  process.env.GOOGLE_APPLICATION_CREDENTIALS ||
  path.join(__dirname, 'service-account.json');

// Where the live cards are hosted (used for the logo + the QR target on the pass).
// Defaults to the GitHub Pages URL; override with CARD_BASE_URL if you use a custom domain.
const CARD_BASE_URL = (
  process.env.CARD_BASE_URL || 'https://xsantcastx.github.io/EclipseRealms/virtual-card'
).replace(/\/$/, '');
const LOGO_URL = process.env.LOGO_URL || CARD_BASE_URL + '/logo.jpg';

if (!ISSUER_ID) {
  console.error('ERROR: set GOOGLE_WALLET_ISSUER_ID (your Google Wallet issuer id).');
  process.exit(1);
}
if (!fs.existsSync(KEY_FILE)) {
  console.error('ERROR: service account key not found at ' + KEY_FILE);
  console.error('Save it there, or set GOOGLE_APPLICATION_CREDENTIALS.');
  process.exit(1);
}

const credentials = JSON.parse(fs.readFileSync(KEY_FILE, 'utf8'));
const CLASS_ID = `${ISSUER_ID}.creadevents_card`;
const WALLET_API = 'https://walletobjects.googleapis.com/walletobjects/v1';

const auth = new GoogleAuth({
  credentials,
  scopes: 'https://www.googleapis.com/auth/wallet_object.issuer',
});

const PEOPLE = {
  angela: {
    suffix: 'angela_triana',
    name: 'Angela Triana',
    title: 'Creative Director',
    phone: '786-356-2219',
    email: 'angela@creadevents.com',
    cardUrl: CARD_BASE_URL + '/',
  },
  carlos: {
    suffix: 'carlos_triana',
    name: 'Carlos Triana',
    title: 'President',
    phone: '786-356-2958',
    email: 'carlos@creadevents.com',
    cardUrl: CARD_BASE_URL + '/carlos/',
  },
};

// ---- Helpers ------------------------------------------------------------
async function api(url, method, data) {
  const client = await auth.getClient();
  return client.request({ url, method, data });
}

async function ensureClass() {
  const genericClass = { id: CLASS_ID };
  try {
    await api(`${WALLET_API}/genericClass/${CLASS_ID}`, 'GET');
    console.log('Class exists:', CLASS_ID);
  } catch (e) {
    if (e.response && e.response.status === 404) {
      await api(`${WALLET_API}/genericClass`, 'POST', genericClass);
      console.log('Created class:', CLASS_ID);
    } else {
      throw e;
    }
  }
}

function buildObject(p) {
  const objectId = `${ISSUER_ID}.${p.suffix}`;
  const digits = p.phone.replace(/\D/g, '');
  return {
    id: objectId,
    classId: CLASS_ID,
    state: 'ACTIVE',
    genericType: 'GENERIC_TYPE_UNSPECIFIED',
    hexBackgroundColor: '#7a154c',
    logo: { sourceUri: { uri: LOGO_URL } },
    cardTitle: { defaultValue: { language: 'en', value: 'Creation Design & Events' } },
    subheader: { defaultValue: { language: 'en', value: p.title } },
    header: { defaultValue: { language: 'en', value: p.name } },
    textModulesData: [
      { id: 'tagline', header: 'Luxury Floral Company', body: 'Miami, Florida · U.S.A.' },
      { id: 'phone', header: 'Phone', body: p.phone },
      { id: 'email', header: 'Email', body: p.email },
    ],
    linksModuleData: {
      uris: [
        { uri: 'tel:+1' + digits, description: 'Call' },
        { uri: 'mailto:' + p.email, description: 'Email' },
        { uri: 'https://www.creadevents.com', description: 'Website' },
        { uri: 'https://instagram.com/creadevents', description: 'Instagram' },
        { uri: p.cardUrl, description: 'Open my card' },
      ],
    },
    barcode: { type: 'QR_CODE', value: p.cardUrl, alternateText: 'creadevents.com' },
  };
}

async function ensureObject(obj) {
  try {
    await api(`${WALLET_API}/genericObject/${obj.id}`, 'GET');
    await api(`${WALLET_API}/genericObject/${obj.id}`, 'PUT', obj);
    console.log('Updated object:', obj.id);
  } catch (e) {
    if (e.response && e.response.status === 404) {
      await api(`${WALLET_API}/genericObject`, 'POST', obj);
      console.log('Created object:', obj.id);
    } else {
      throw e;
    }
  }
}

function saveLink(obj) {
  const claims = {
    iss: credentials.client_email,
    aud: 'google',
    typ: 'savetowallet',
    payload: { genericObjects: [{ id: obj.id, classId: CLASS_ID }] },
  };
  const token = jwt.sign(claims, credentials.private_key, { algorithm: 'RS256' });
  return `https://pay.google.com/gp/v/save/${token}`;
}

// ---- Main ---------------------------------------------------------------
(async () => {
  await ensureClass();
  const links = {};
  for (const key of Object.keys(PEOPLE)) {
    const obj = buildObject(PEOPLE[key]);
    await ensureObject(obj);
    links[key] = saveLink(obj);
    console.log(`\n${PEOPLE[key].name} — Add to Google Wallet:\n${links[key]}\n`);
  }
  const outFile = path.join(__dirname, '..', 'google-links.js');
  fs.writeFileSync(
    outFile,
    '// Generated by wallet/generate-google-wallet.js — do not edit by hand.\n' +
      'window.GOOGLE_WALLET_LINKS = ' +
      JSON.stringify(links, null, 2) +
      ';\n'
  );
  console.log('Wrote ' + outFile + '\nCommit google-links.js and the buttons go live.');
})().catch((e) => {
  console.error('FAILED:', e.response ? JSON.stringify(e.response.data, null, 2) : e.message);
  process.exit(1);
});
