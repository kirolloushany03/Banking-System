using Banking_System.Services;
using NCrontab;

namespace Banking_System
{
    public class CronScheduler : BackgroundService
    {
        private readonly IAccountService _accountService;
        private Timer? _timer;

        public CronScheduler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Define the cron expression for monthly execution (e.g., every first day of the month at midnight)
            var cronExpression = "0 0 1 * *"; // Cron expression for first day of each month at midnight

            // Parse the cron expression
            var schedule = CrontabSchedule.Parse(cronExpression);

            // Get the next occurrence of the cron schedule from the current time
            var nextRun = schedule.GetNextOccurrence(DateTime.Now);

            // Calculate the time interval to the next run
            var timeUntilNextRun = nextRun - DateTime.Now;

            // Set up a timer to execute the job
            _timer = new Timer(async _ => await _accountService.CalculateMonthlyInterest(), null, timeUntilNextRun, TimeSpan.FromDays(30)); // Repeat every 30 days after the first execution

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Dispose the timer when the service is stopped
            _timer?.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
