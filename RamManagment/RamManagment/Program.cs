// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Management;
System.Timers.Timer _timer;
 PerformanceCounter _ramCounter;

// Initialize the PerformanceCounter for available memory
_ramCounter = new PerformanceCounter("Memory", "Available MBytes");

// Set up a timer to trigger every 30 seconds (30000 milliseconds)
_timer = new System.Timers.Timer(30000);
_timer.Elapsed += OnTimedEvent;
_timer.AutoReset = true;
_timer.Enabled = true;

Console.WriteLine("Press [Enter] to exit the program...");
Console.ReadLine();

void OnTimedEvent(Object source, ElapsedEventArgs e)
{
    // Get the available memory
    var availableMemory = _ramCounter.NextValue();

    // Get the total physical memory
    var totalMemory = GetTotalMemory();
    var usedMemory = totalMemory - availableMemory;

    // Create a log entry
    var logEntry = $"{DateTime.Now}: Total Physical Memory: {totalMemory} MB, Used Memory: {usedMemory} MB, Available Memory: {availableMemory} MB";

    // Write the log entry to a file
    var logFilePath = "RamUsageLog.txt";
    using (StreamWriter sw = new StreamWriter(logFilePath, true))
    {
        sw.WriteLine(logEntry);
    }

    Console.WriteLine(logEntry);
}

 float GetTotalMemory()
{
    float totalMemory = 0;
    try
    {
        // Query WMI for total physical memory
        var query = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
        using (var searcher = query.Get())
        {
            foreach (ManagementObject queryObj in searcher)
            {
                // Convert total physical memory from bytes to MB
                totalMemory = Convert.ToSingle(queryObj["TotalPhysicalMemory"]) / (1024 * 1024);
                break; // Assuming there's only one result, we break after the first iteration
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving total physical memory: {ex.Message}");
    }
    return totalMemory;
}