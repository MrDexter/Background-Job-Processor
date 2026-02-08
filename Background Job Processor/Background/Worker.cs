using System.Diagnostics;
using BackgroundJobs.Services;

namespace BackgroundJobs.Background;

public class BackgroundService
{
    
}

public class BackgroundWorker : BackgroundService
{
    private readonly IJobService JobService;
    private readonly IProcessorService ProcessorService;
    private readonly ILogger Logger;

    protected async Task ExecuteAsync(CancellationToken stopToken)
    {
        Logger.LogInformation("Background Worker Started");

        while (!stopToken.IsCancellationRequested)
        {
            try
            {
                var job = JobService.GetWaitingJobAsync(stopToken);
                if (job is null)
                {
                    // No job found, wait
                    Logger.LogInformation("No Job Found Waiting...");
                    await Task.Delay(TimeSpan.FromSeconds(5), stopToken);
                    continue;
                }
                Logger.LogInformation("Job Found Starting Processing");
                var processor = ProcessorService.GetJobProcessorAsync("PlayerDump"); // job.Type not working?

                if (processor is null)
                {
                    // No Propcess found
                    JobService.UpdateJobStatusAsync(job.Id, "Failed", "No Processor Found");
                    continue;
                }

                await JobService.UpdateJobStatusAsync(job.Id, "Processing", "Processing");
                // Perform Process
            //    ProcessorService.processor();
                await JobService.UpdateJobStatusAsync(job.Id, "Complete", "Complete");
            }
            catch (OperationCanceledException) // Canceled
            {
                
            }
            catch (Exception exception)// Job Failed
            {
                
            }
        }
    }
}