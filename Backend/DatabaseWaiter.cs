using System;
using System.Data;
using MySql.Data.MySqlClient;

// Datenbankverbindung vor Start der Anwendung sicher stellen um unerwartetes Verhalten zu unterbinden (wenn nämlich die Anwendung vor der Db geladen wird)

public static class DatabaseWaiter
{
    public static void WaitForDatabaseConnection(string connectionString)
    {
        Console.WriteLine("Warte auf die Datenbankverbindung...");

        bool connected = false;
        int retryCount = 0;
        int maxRetries = 10;
        TimeSpan retryInterval = TimeSpan.FromSeconds(10);

        while (!connected && retryCount < maxRetries)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        connected = true;
                        Console.WriteLine("Datenbankverbindung hergestellt.");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Verbindung zur Datenbank fehlgeschlagen. Warte {retryInterval.TotalSeconds} Sekunden...");
                retryCount++;
                System.Threading.Thread.Sleep(retryInterval);
            }
        }

        if (!connected)
        {
            Console.WriteLine("Maximale Anzahl von Verbindungsversuchen erreicht. Die Anwendung wird nicht gestartet.");
            Environment.Exit(1);
        }
    }
}