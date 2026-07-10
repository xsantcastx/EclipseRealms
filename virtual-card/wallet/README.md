# Google Wallet passes (self-owned, free)

This builds a **branded "Add to Google Wallet" pass** for Angela and Carlos — plum
background, the lotus logo, name / title / "Luxury Floral Company", phone + email
fields, and a **QR code that opens the full lotus web card**. No third-party app,
no fee.

> Note on looks: Google Wallet renders passes in **its own layout** (you can't put the
> literal HTML card inside Wallet). This makes it as on-brand as Google allows, and the
> QR/link opens the real card.

## One-time Google setup (~10 min, free)

1. **Google Cloud project** — go to <https://console.cloud.google.com>, create a project
   (or reuse one).
2. **Enable the API** — in the console, enable **Google Wallet API**
   (APIs & Services → Library → search "Google Wallet API" → Enable).
3. **Issuer account** — go to the **Google Wallet API Console**
   <https://pay.google.com/business/console/> and sign up as an issuer. Copy your
   **Issuer ID** (a long number).
4. **Service account + key**
   - In Cloud Console → **IAM & Admin → Service Accounts → Create service account**.
   - Create a **JSON key** for it and download it.
   - Save that file here as **`wallet/service-account.json`** (it's git-ignored).
   - In the **Google Wallet API Console → Users**, add the service account's email and
     give it the **Developer / Wallet Object Issuer** access.

## Generate the passes

```bash
cd virtual-card/wallet
npm install
# use YOUR issuer id from step 3:
GOOGLE_WALLET_ISSUER_ID=3388000000000000000 npm run generate
```

Optional overrides:
- `CARD_BASE_URL` — if you host the cards somewhere other than the default GitHub Pages
  URL (e.g. `https://card.creadevents.com`). The logo + QR on the pass use this.

What it does:
- Creates the pass **class** + a pass **object** for each person.
- Writes signed **"Save to Google Wallet"** links into **`../google-links.js`**.

## Turn the buttons on

The generator writes `virtual-card/google-links.js`. Commit it:

```bash
git add virtual-card/google-links.js
git commit -m "Enable Google Wallet buttons"
git push
```

Now the **"Add to Google Wallet"** button appears on Angela's and Carlos's cards.
Android users tap it → the branded pass is saved to their Google Wallet. 🎉
(iPhone users keep using **Add to Contacts** — Apple Wallet needs the separate paid route.)

## Updating a pass later

Change the pass design/fields in `generate-google-wallet.js`, re-run `npm run generate`,
and re-commit `google-links.js`. Existing saved passes update automatically.

## Security

- **Never commit `service-account.json`** (already git-ignored). It's a private key.
- If it ever leaks, delete/rotate the key in Cloud Console.
