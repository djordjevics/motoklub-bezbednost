/**
 * Runs before `npm run build`: ensures Aikido Safe Chain is installed, wired to npm, and matches npm registry latest.
 * @see https://github.com/AikidoSec/safe-chain
 */
import { execFileSync } from 'node:child_process'
import fs from 'node:fs'
import os from 'node:os'
import path from 'node:path'

const safeChainRoot = path.join(os.homedir(), '.safe-chain')
const safeChainExe = path.join(
  safeChainRoot,
  'bin',
  process.platform === 'win32' ? 'safe-chain.exe' : 'safe-chain',
)
const npmShim = path.join(
  safeChainRoot,
  'shims',
  process.platform === 'win32' ? 'npm.cmd' : 'npm',
)

function runFile(file, args, label, options = {}) {
  try {
    return execFileSync(file, args, {
      encoding: 'utf8',
      stdio: ['ignore', 'pipe', 'pipe'],
      ...options,
    }).trim()
  } catch (e) {
    const err = new Error(`${label} failed: ${file} ${args.join(' ')}`)
    err.cause = e
    throw err
  }
}

/**
 * Small env for `safe-chain.exe npm …` on Windows. Inheriting the full `npm run` environment can
 * break the nested pkg CLI (`Cannot find module '…\\frontend\\npm'`); pass only PATH + basics.
 */
function envForSafeChainNpm() {
  const home = os.homedir()
  const chainBin = path.join(home, '.safe-chain', 'bin')
  const fromParent = String(process.env.Path || process.env.PATH || '')
    .split(path.delimiter)
    .filter(Boolean)
    .filter((p) => !p.includes(path.join('.safe-chain', 'shims')))
  const pathParts = [chainBin, 'C:\\Program Files\\nodejs', ...fromParent]
  const seen = new Set()
  const merged = []
  for (const p of pathParts) {
    if (!seen.has(p.toLowerCase())) {
      seen.add(p.toLowerCase())
      merged.push(p)
    }
  }
  const pathEnv = merged.join(path.delimiter)
  const pick = (k) => process.env[k]
  return {
    SYSTEMROOT: pick('SYSTEMROOT'),
    WINDIR: pick('WINDIR'),
    TEMP: pick('TEMP'),
    TMP: pick('TMP'),
    USERPROFILE: pick('USERPROFILE'),
    HOMEDRIVE: pick('HOMEDRIVE'),
    HOMEPATH: pick('HOMEPATH'),
    LOCALAPPDATA: pick('LOCALAPPDATA'),
    APPDATA: pick('APPDATA'),
    USERNAME: pick('USERNAME'),
    Path: pathEnv,
    PATH: pathEnv,
    COMSPEC: pick('ComSpec') || pick('COMSPEC') || 'C:\\Windows\\System32\\cmd.exe',
  }
}

/** Minimal env so `safe-chain.exe` (pkg) is not confused by npm lifecycle / workspace variables. */
function minimalSafeChainEnv() {
  const keys = [
    'PATH',
    'Path',
    'PATHEXT',
    'SYSTEMROOT',
    'WINDIR',
    'TEMP',
    'TMP',
    'USERPROFILE',
    'USERNAME',
    'HOMEDRIVE',
    'HOMEPATH',
    'LOCALAPPDATA',
    'APPDATA',
    'PROGRAMFILES',
    'PROGRAMFILES(X86)',
    'COMSPEC',
  ]
  const e = {}
  for (const k of keys) {
    const v = process.env[k]
    if (v !== undefined) {
      e[k] = v
    }
  }
  return e
}

/**
 * Run npm through safe-chain. On Windows, calling `npm.cmd` from a script that is itself started by
 * `npm run` can break argument passing; invoke `safe-chain.exe npm …` directly instead.
 */
function runNpm(args, label) {
  if (process.platform === 'win32') {
    return runFile(safeChainExe, ['npm', ...args], label, { env: envForSafeChainNpm() })
  }
  return runFile(npmShim, args, label)
}

/**
 * Read CLI version from the safe-chain binary. On Windows, when npm is driven from PowerShell
 * shell integration, spawning `safe-chain.exe` directly from this script can confuse the nested
 * pkg bootstrap; delegate to `cmd /c` with a **single** remainder argument (no nested quotes).
 */
function safeChainVersion() {
  const opts = { env: minimalSafeChainEnv() }
  if (process.platform !== 'win32') {
    return runFile(safeChainExe, ['--version'], 'safe-chain --version', opts)
  }
  const comspec = opts.env.ComSpec || process.env.ComSpec || 'cmd.exe'
  const remainder = safeChainExe.includes(' ')
    ? `"${safeChainExe}" --version`
    : `${safeChainExe} --version`
  return runFile(comspec, ['/c', remainder], 'safe-chain --version', opts)
}

if (!fs.existsSync(safeChainExe)) {
  console.error(
    '[safe-chain-preflight] safe-chain is not installed under ~/.safe-chain. Install from https://github.com/AikidoSec/safe-chain (Windows: install-safe-chain.ps1 with -ci for npm shims), then restart the terminal.',
  )
  process.exit(1)
}

if (!fs.existsSync(npmShim)) {
  console.error(
    '[safe-chain-preflight] safe-chain npm shim missing. Re-run the installer with -ci: https://github.com/AikidoSec/safe-chain#windows-powershell',
  )
  process.exit(1)
}

let localRaw
try {
  localRaw = safeChainVersion()
} catch {
  console.error(
    '[safe-chain-preflight] safe-chain --version failed. Install from https://github.com/AikidoSec/safe-chain (Windows: install-safe-chain.ps1), then restart the terminal.',
  )
  process.exit(1)
}

const semverMatch = localRaw.match(/(\d+\.\d+\.\d+(?:-[.\w]+)?)/)
const localVersion = semverMatch ? semverMatch[1] : localRaw

try {
  runNpm(['safe-chain-verify'], 'npm safe-chain-verify')
} catch {
  console.error(
    '[safe-chain-preflight] npm safe-chain-verify failed. Re-run the safe-chain Windows installer with -ci so shims exist under ~/.safe-chain/shims.',
  )
  process.exit(1)
}

let latest
try {
  latest = runNpm(['view', '@aikidosec/safe-chain', 'version'], 'npm view')
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
