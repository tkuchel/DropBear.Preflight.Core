namespace DropBear.Preflight.Core;

public class ProgressEventArgs : EventArgs
{
    // The current progress as a value between 0 and 1
    public double Progress { get; }

    public ProgressEventArgs(double progress)
    {
        Progress = progress;
    }
}
