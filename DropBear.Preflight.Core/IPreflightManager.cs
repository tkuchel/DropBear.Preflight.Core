namespace DropBear.Preflight.Core
{
    /// <summary>
    /// Defines methods and events for managing the execution of preflight tasks.
    /// </summary>
    public interface IPreflightManager
    {
        /// <summary>
        /// Occurs when a task has completed.
        /// </summary>
        event EventHandler<TaskCompletionEventArgs> TaskCompleted;

        /// <summary>
        /// Occurs when the overall progress has changed.
        /// </summary>
        event EventHandler<ProgressEventArgs> OverallProgress;

        /// <summary>
        /// Starts the execution of tasks.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Adds a task to the list of tasks to be executed.
        /// </summary>
        void AddTask(PreflightTask task);
    }
}