# PowerShell-Skript: Extrahiere nur das Schema aus einer MySQL-Dump-Datei (data.sql)
# Ergebnis: 1_schema.sql (nur Tabellenstruktur, keine Daten)

$inputFile = "data.sql"
$outputFile = "docker-entrypoint-initdb.d\1_schema.sql"

# Lies alle Zeilen ein
$content = Get-Content $inputFile

# Filtere alle Zeilen heraus, die mit INSERT, LOCK, UNLOCK oder ALTER TABLE ... DISABLE/ENABLE KEYS beginnen
$schemaOnly = $content | Where-Object {
    ($_ -notmatch '^INSERT INTO') -and
    ($_ -notmatch '^LOCK TABLES') -and
    ($_ -notmatch '^UNLOCK TABLES') -and
    ($_ -notmatch '^/\*!40000 ALTER TABLE .* (DISABLE|ENABLE) KEYS \*/;')
}

# Schreibe das Ergebnis in die neue Datei
$schemaOnly | Set-Content $outputFile

Write-Host "Fertig: $outputFile wurde aktualisiert"
