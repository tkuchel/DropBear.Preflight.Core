﻿namespace DropBear.Preflight.Core;

/// <summary>
/// Provides data for the OverallProgress event.
/// </summary>
public class ProgressEventArgs : EventArgs
{
    /// <summary>
    /// Gets the current progress as a value between 0 and 1.
    /// </summary>
    public double Progress { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class.
    /// </summary>
    /// <param name="progress">The current progress as a value between 0 and 1.</param>
    public ProgressEventArgs(double progress)
    {
        Progress = progress;
    }
}
