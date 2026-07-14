/// <reference types="vitest/config" />
import fs from 'node:fs'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

const certPath = '.certs/localhost.pem'
const keyPath = '.certs/localhost.key'
const devCertExists = fs.existsSync(certPath) && fs.existsSync(keyPath)

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    port: 5173,
    // The API's refresh-token cookie is Secure + SameSite=Strict. Chrome's schemeful-same-site
    // rules treat http and https as different "sites" even on the same host/port, so an http dev
    // server would never get that cookie sent back to it — the frontend must also run on https
    // for the auth flow to work locally, matching how both sides are https in any real
    // deployment. Reuses the ASP.NET Core dev cert (already trusted via `dotnet dev-certs https
    // --trust`) instead of a plugin-generated self-signed one, so there's no separate browser
    // trust prompt to click through: run `dotnet dev-certs https -ep web/.certs/localhost.pem
    // --format Pem --no-password` from the repo root to (re)generate it.
    //
    // Guarded by existsSync rather than the `serve`/`build` command, because Vitest also
    // resolves this config with command === 'serve' — a plain command check would make `npm
    // test` fail too. `.certs/` is gitignored (a local dev artifact), so CI (which only ever
    // runs build/test, never the dev server) simply never has it and skips this block.
    ...(devCertExists && {
      https: {
        cert: fs.readFileSync(certPath),
        key: fs.readFileSync(keyPath),
      },
    }),
  },
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: './src/test/setup.ts',
  },
})
