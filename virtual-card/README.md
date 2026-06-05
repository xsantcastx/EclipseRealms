# Angela Triana — Digital Business Card

A free, shareable digital business card for **Angela Triana, Creative Director — Creation Design & Events**.
Anyone who opens the link can tap **Add to Contacts** to save Angela directly into their
iPhone or Android phone, call/email with one tap, follow on social, and scan a QR code to
pass the card along.

## What's in this folder

| File | Purpose |
|------|---------|
| `index.html` | The card itself (open this in any browser). |
| `angela-triana.vcf` | The contact file (vCard) the "Add to Contacts" button downloads. |
| `logo.jpg` | Creation Design & Events logo shown on the card. |
| `photo.jpg` | *(optional)* A photo of Angela. Drop a square JPG here and it appears automatically. |

> If `photo.jpg` is missing, the card gracefully shows an "AT" monogram instead — nothing breaks.

## How to see it right now

Just open `virtual-card/index.html` in any web browser (double-click it). Everything works
locally except the QR code, which needs the card to be hosted at a real web address (below).

## Publish it for free (so colleagues can open the link)

The easiest free option is **GitHub Pages**:

1. In the repo on GitHub: **Settings → Pages**.
2. Under **Build and deployment**, set **Source = Deploy from a branch**.
3. Pick the branch (e.g. `main` after this is merged) and folder **/ (root)**, then **Save**.
4. After a minute the card is live at:
   `https://<your-github-username>.github.io/<repo>/virtual-card/`

Other one-click free hosts that also work: **Netlify Drop** (drag this folder onto
netlify.com/drop), **Vercel**, or **Cloudflare Pages**.

Once it's live, the QR code and Share button automatically use that real address.

### Tip: use her own domain
Since she owns **creadevents.com**, a nice touch is to host the card at something like
`https://card.creadevents.com` or `https://www.creadevents.com/card`. Point that to the same
files and the card looks fully branded.

## Sharing with colleagues

Once live, share the link any way you like:
- **Text / WhatsApp / email** the URL.
- Let people **scan the QR code** shown on the card.
- Add the link to her email signature and Instagram/Facebook bio.

When someone taps **Add to Contacts**, the `.vcf` saves Angela into their phone's Contacts —
this is the universal way contacts get added on both **iPhone** and **Android**.

## About Apple Wallet / Google Wallet

The screenshots showed cards that "Add to Apple/Google Wallet." A real Wallet **pass**
(`.pkpass` for Apple, a Google Wallet object for Android) must be **cryptographically signed**
and requires:

- **Apple:** a paid Apple Developer account ($99/yr) + a Pass Type ID certificate.
- **Google:** a Google Cloud project + Wallet API issuer account + service-account key.

Those credentials can't be generated here, so this card uses the **vCard "Add to Contacts"**
approach instead — which is what most "digital business cards" actually do to save you into a
phone, works on every device, and needs no accounts or fees.

If Angela later wants true Wallet passes, the path is:
1. Get the Apple/Google credentials above.
2. Generate a signed pass (e.g. with the `passkit-generator` Node library for Apple, and the
   Google Wallet REST API for Android).
3. Add "Add to Apple Wallet" / "Add to Google Wallet" buttons that serve those passes.

Open an issue / ask and this can be scaffolded when the credentials are ready.

## Editing the details

All of Angela's info lives in two places — edit both if something changes:
- **`index.html`** — the visible text, links, phone, emails.
- **`angela-triana.vcf`** — the saved-contact data.

Note: the **Facebook** link (`facebook.com/creadevents`) is a best guess. Update it in both
files if her real Facebook page URL is different.
