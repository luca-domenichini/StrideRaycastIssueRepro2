using Stride.Engine;
using System;
using System.Diagnostics;
using System.Threading;

using var game = new Game();
game.Run();

Console.WriteLine("Game finished! Here are the threads running:");

// print out the list of threads running
foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
{
    Console.WriteLine($"Thread ID: {thread.Id}, State: {thread.ThreadState}");
}

game.Dispose(); // try to set a breakpoint here and look at Visual Studio Threads window, before and after disposing.

Thread.Sleep(1000); // wait for a second to give CLR some time to clean up the threads.. (we should not need that)

Console.WriteLine("Game disposed! Here are the remaining threads:");

// after the game is disposed, print out the list of threads running
foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
{
    Console.WriteLine($"Thread ID: {thread.Id}, State: {thread.ThreadState}");
}

Console.WriteLine("Game disposed. Press any key to exit."); // set a breakpoint here and look at Visual Studio Threads window. There are still threads running from BepuUtilities and Stride ThreadPool
Console.ReadKey();
