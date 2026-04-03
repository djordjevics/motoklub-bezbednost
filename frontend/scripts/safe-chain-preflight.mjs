/**
 * Runs before `npm run build`: ensures Aikido Safe Chain is installed, wired to npm, and matches npm registry latest.
 * @see https://github.com/AikidoSec/safe-chain
 */
import { execSync } from 'node:child_process'

function run(cmd, label) {
  try {
    return execSync(cmd, {
      encoding: 'utf8',
      stdio: ['ignore', 'pipe', 'pipe'],
      shell: true,
    }).trim()
  } catch (e) {
    const err = new Error(`${label} failed: ${cmd}`)
    err.cause = e
    throw err
  }
}

let localRaw
try {
  localRaw = run('safe-chain --version', 'safe-chain --version')
} catch {
  console.error(
    '[safe-chain-preflight] safe-chain is not on PATH. Install from https://github.com/AikidoSec/safe-chain (Windows: install-safe-chain.ps1), then restart the terminal.',
  )
  process.exit(1)
}

const semverMatch = localRaw.match(/(\d+\.\d+\.\d+(?:-[.\w]+)?)/)
const localVersion = semverMatch ? semverMatch[1] : localRaw

try {
  run('npm safe-chain-verify', 'npm safe-chain-verify')
} catch {
  console.error(
    '[safe-chain-preflight] npm safe-chain-verify failed. Safe Chain may not wrap npm (restart terminal after install; check shell integration).',
  )
  process.exit(1)
}

let latest
try {
  latest = run('npm view @aikidosec/safe-chain version', 'npm view')
} catch {
  console.warn(
    '[safe-chain-preflight] Could not read latest @aikidosec/safe-chain from registry; skipping version match.',
  )
  console.log(`[safe-chain-preflight] OK: safe-chain CLI reports ${localVersion}`)
  process.exit(0)
}

const normalize = (v) => v.replace(/^v/i, '').split('-')[0]
const localBase = normalize(localVersion)
const latestBase = normalize(latest)

if (localBase !== latestBase) {
  console.error(
    `[safe-chain-preflight] safe-chain ${localVersion} is not the same as npm latest @aikidosec/safe-chain (${latest}). Update: https://github.com/AikidoSec/safe-chain/releases`,
  )
  process.exit(1)
}

console.log(`[safe-chain-preflight] OK: safe-chain ${localVersion} (matches npm latest ${latest})`)
