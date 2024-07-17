// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

System.Timers.Timer _timer;
 PerformanceCounter _ramCounter;

PerformanceCounter _committedBytesCounter;
// Initialize the PerformanceCounter for available memory
_ramCounter = new PerformanceCounter("Memory", "Available MBytes");

// Initialize the PerformanceCounter for committed bytes
_committedBytesCounter = new PerformanceCounter("Memory", "Committed Bytes");

// Set up a timer to trigger every 30 seconds (30000 milliseconds)
_timer = new System.Timers.Timer(30000);
_timer.Elapsed += OnTimedEvent;
_timer.AutoReset = true;
_timer.Enabled = true;

Console.WriteLine("Press [Enter] to exit the program...");
Console.ReadLine();

void OnTimedEvent(Object source, ElapsedEventArgs e)
{
    // Get the total and available memory
    var availableMemory = _ramCounter.NextValue();
    var totalMemory = GetTotalMemory();
    var usedMemory = totalMemory - availableMemory;

    // Create a log entry
    var logEntry = $"{DateTime.Now}: Total Memory: {totalMemory} MB, Used Memory: {usedMemory} MB, Available Memory: {availableMemory} MB";

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
    // Get the total committed bytes and convert to MB
    var totalBytes = _committedBytesCounter.NextValue();
    var totalMB = totalBytes / (1024 * 1024);
    return totalMB;
}