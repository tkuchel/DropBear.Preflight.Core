﻿namespace DropBear.Preflight.Core;

/// <summary>
///     Represents the configuration options for the PreflightManager.
/// </summary>
public class PreflightManagerConfig
{
    /// <summary>
    ///     Gets or sets a value indicating whether the manager should stop executing tasks if a task fails.
    /// </summary>
    public bool StopOnFailure { get; set; } = false;

    /// <summary>
    ///     Gets or sets the timeout for each task.
    /// </summary>
    public TimeSpan TaskTimeout { get; set; } = TimeSpan.FromMinutes(1);


    /// <summary>
    ///     Gets or sets the number of times to retry a task if it fails.
    /// </summary>
    public int RetryCount { get; set; } = 0;
    
    /// <summary>
    ///   Gets or sets a value indicating whether logging is enabled.
    /// </summary>
    public bool EnableDebugLogging { get; set; } = false;
    
    /// <summary>
    ///  Gets or sets a value indicating whether error logging is enabled.
    /// </summary>
    public bool EnableErrorLogging { get; set; } = true;
}