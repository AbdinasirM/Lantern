# Lantern — TODO

I'm building Lantern as a self-hosted chat app for Ollama. The server keeps the chat history; the phone connects to it and keeps a local cache. I want to be able to start on one device and pair another without setting up email/password accounts.

I'm keeping this list in the repo so I can pick up where I left off after work.

**Current task:** Secure the device-pairing flow. Basic code redemption works in Postman; next, add rate limiting, enforce failed-attempt limits, prevent simultaneous redemption, and handle six-digit code collisions.

## Ground rules

- Aim for 1–2 hours per weekday session. It's fine if a task takes two evenings.
- Work on one small thing, test it, then commit it.
- If I can't explain why something works, take a step back and understand it first.
- Check things off after I've actually tested them.
- Keep tokens, secrets, and the local SQLite database out of Git.

## What I've finished

- [x] Set up the solution: Server, Database, Shared, and Mobile projects.
- [x] Connect the server to SQLite with EF Core.
- [x] Set up migrations and apply them automatically in Development.
- [x] Add the database models: users, devices, conversations, messages, pairing codes.
- [x] Set up the relationships between those models.
- [x] Add the Ollama model-list and availability endpoints.
- [x] Build user registration and create a device at the same time.
- [x] Generate random device tokens; only store their SHA-256 hashes.
- [x] Add a custom authentication handler for device tokens.
- [x] Protect `GET /api/users/me` and read the user/device IDs from claims.
- [x] Test registration and authentication in Postman.
- [x] Confirm 401 for missing/invalid tokens and 200 for a valid token.
- [x] Build the basic pairing-code generator and HMAC hash function.
- [x] Add authenticated `POST /api/pairing/code` and verify that it stores a hashed, expiring code in SQLite.
- [x] Implement basic `POST /api/pairing/redeem`, create a second device, and return its own token.
- [x] Test successful pairing in Postman and verify that the device was created and the code marked used in SQLite.
- [x] Confirm a used pairing code is rejected on a second, sequential redemption attempt.

## Next up: device pairing

The first device is already registered. A second device can now join the same user with a short-lived code. The basic flow works, but its security protections are not finished.

- [x] **Session 1 (~1h):** Make `PairingController`, inject the pairing service, and add a protected endpoint to request a code. Get the user ID from the authenticated token. Test it in Postman.
- [ ] **Session 2 (~1–2h):** Accept a submitted code and check whether it's valid, expired, already used, or locked after too many attempts.
  - [x] Validate six-digit format and look up the matching code hash.
  - [x] Reject codes that are expired, already used, or have reached the failed-attempt limit.
  - [ ] Record failed redemption attempts so the limit is actually enforced.
  - [ ] Test invalid, expired, and locked codes.
- [ ] **Session 3 (~1–2h):** On a valid code, create the second device and issue its own random token. The code must only work once, even if two requests arrive together.
  - [x] Create the new device and generate/hash its token.
  - [x] Mark the code as used and save both changes with EF Core.
  - [x] Verify a used code cannot be redeemed again sequentially.
  - [ ] Prevent concurrent requests from redeeming the same code twice.
- [ ] **Session 4 (~1–2h):** Test the full pairing flow in Postman. Add request limits, handle six-digit code collisions, and check invalid/reused codes.
  - [x] Test successful pairing in Postman.
  - [x] Verify the new device and `IsUsed = 1` in SQLite.
  - [x] Confirm a reused code is rejected.
  - [ ] Add rate limiting to the unauthenticated redemption endpoint.
  - [ ] Handle generated six-digit code collisions (including the unique hash index).
  - [ ] Test invalid, expired, and repeatedly guessed codes.
- [ ] **Session 5 (~1–2h):** Add a way to list my devices and revoke one. Test that a revoked token stops working.

## After that: chat API

I want the server to be usable through Postman before building the mobile screens.

- [ ] **Session 6 (~1–2h):** Create a conversation for the authenticated user and list their conversations.
- [ ] **Session 7 (~1–2h):** Get, rename, and delete conversations. Make sure one user can't edit another user's chat.
- [ ] **Session 8 (~1–2h):** Save messages and return the history in the right order.
- [ ] **Session 9 (~1–2h):** Check the selected Ollama model and handle a missing/unavailable model.
- [ ] **Session 10 (~1–2h):** Send a prompt to Ollama and save the assistant's reply in SQLite.
- [ ] **Session 11 (~1h):** Test the full flow: register → authenticate → create chat → send message → reopen history.

## Mobile app (Android first)

The server comes first so I don't end up debugging the API and mobile UI at the same time.

- [ ] **Session 12 (~1–2h):** Create the server URL screen and check that the Android emulator can reach the API.
- [ ] **Session 13 (~1–2h):** Register the first device from the app and save its token with MAUI SecureStorage.
- [ ] **Session 14 (~1h):** Restore the saved token when the app opens. Skip setup if it still works; handle revoked tokens.
- [ ] **Session 15 (~1–2h):** Show conversations and add a new-chat screen with model selection.
- [ ] **Session 16 (~1–2h):** Build the chat screen, load messages, and send a message to Ollama.
- [ ] **Session 17 (~1–2h):** Add the two pairing screens: show a code on the old device, enter it on the new one.
- [ ] **Session 18 (~1–2h, maybe split):** Cache recent conversations/messages locally and show them when offline.

## Before calling it an MVP

- [ ] **Session 19 (~1–2h):** Check API authorization, pairing-code limits, error handling, and HTTPS for non-local use.
- [ ] **Session 20 (~1–2h, maybe split):** Test a clean setup with two devices. Update the README with actual setup instructions and fix the rough edges I find.

## Ideas for later (not part of the first version)

- [ ] Stream responses as Ollama generates them.
- [ ] Edit or regenerate messages.
- [ ] Search and organize conversations.
- [ ] Support attachments or images.
- [ ] Improve offline sync.
- [ ] Figure out account recovery if all paired devices are lost.
- [ ] Package the server and publish an Android build.

## Decisions / things to remember

- Chat history belongs on the server. The mobile database is a cache, not the source of truth.
- Every device gets a different token. The server stores a hash, and the app keeps the token securely.
- A pairing code is temporary (currently five minutes), limited to a few attempts, and should be usable only once.
- Only an already authenticated device should be able to create pairing codes for its user.
- Don't accept a user ID from the request body when I can get it from authentication claims.
- Keep the scope small until I can send a message from Android and see the reply.
- **Pairing security still to finish:** Failed attempts are checked but not yet recorded; rate limiting, code collisions, and simultaneous-redemption protection are outstanding.

## Work log

I'll track real focused time from now on. For the first day, I only have an approximate time window.

| Date | Time | What I worked on |
| --- | --- | --- |
| 2026-11-15 | ~7h elapsed (estimate; breaks not tracked) | SQLite/EF Core models and migrations, registration, device-token authentication, protected route, Postman tests. |
| 2026-11-16 | ~1h | Added the authenticated pairing-code endpoint, tested it in Postman, and verified the code was saved in SQLite. |
| 2026-11-17 | **1h 30m** | Built pairing-code validation and device redemption. Added response DTOs, generated a new device token, marked codes used, and tested successful pairing and rejection of reused codes in Postman and SQLite. |

### End of each session

- [ ] Build and test what I changed.
- [ ] Write down any bug or unfinished step.
- [ ] Update the work log with actual time spent.
- [ ] Move the current task to the next small step.
- [ ] Review staged files for secrets and commit working changes.