/// <reference types="vitest/config" />
import fs from 'node:fs'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

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
    https: {
      cert: fs.readFileSync('.certs/localhost.pem'),
      key: fs.readFileSync('.certs/localhost.key'),
    },
  },
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: './src/test/setup.ts',
  },
})
