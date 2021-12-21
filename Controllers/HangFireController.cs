using Hangfire;
using HangFireSample.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HangFireSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireController : ControllerBase
    {

        MailHelper _mailHelper = new MailHelper();

        [HttpPost]
        [Route("welcome")]
        public IActionResult Welcome(string mailAddress)
        {
            //Fire-and-Forget Jobs
            //Fire - and - forget jobs are executed only once and almost immediately after creation.
            var mailSubject = $"Welcome {mailAddress}";
            var mailBody = $"<p>Thanks for join us!</p>";
            var jobId = BackgroundJob.Enqueue(()=> _mailHelper.SendMail(mailAddress, mailSubject, mailBody));

            return Ok($"JobId : {jobId} Completed. Welcome Mail Sent");
        }

        [HttpPost]
        [Route("welcome-delayed")]
        public IActionResult WelcomeDelayed(string mailAddress)
        {
            //Delayed Jobs
            //Delayed jobs are executed only once too, but not immediately, after a certain time interval.
            var mailSubject = $"Welcome {mailAddress}";
            var mailBody = $"<p>Thanks for join us!</p></br><p>This mail is delayed mail.!</p>";
            var jobId = BackgroundJob.Schedule(() => _mailHelper.SendMail(mailAddress, mailSubject, mailBody), TimeSpan.FromSeconds(20));

            return Ok($"JobId : {jobId} Completed. Welcome Mail Sent");
        }

        [HttpPost]
        [Route("welcome-recurring")]
        public IActionResult WelcomeRecurring(string mailAddress)
        {
            //Recurring Jobs
            //Recurring jobs fire many times on the specified CRON schedule.
            var mailSubject = $"Welcome {mailAddress}";
            var mailBody = $"<p>Thanks for join us!</p></br><p>This mail is recurring every minute</p>";
            RecurringJob.AddOrUpdate(() => _mailHelper.SendMail(mailAddress, mailSubject, mailBody), Cron.Minutely);

            return Ok($"Recurring job started, mails will send in every minute");
        }

        [HttpPost]
        [Route("welcome-continuation")]
        public IActionResult WelcomeContinuation(string mailAddress)
        {
            //Continuations Jobs
            //Continuations are executed when its parent job has been finished.

            //First job
            var jobId = BackgroundJob.Enqueue(() => _mailHelper.NotifySystemAdmin());

            var mailSubject = $"Welcome {mailAddress}";
            var mailBody = $"<p>Thanks for join us!</p></br><p>This mail is continuations hangfire job example.<br>Job started after NotifySystemAdmin process</p>";
            BackgroundJob.ContinueJobWith(jobId, () => _mailHelper.SendMail(mailAddress, mailSubject, mailBody));

            return Ok($"Recurring job started, mails will send in every minute");
        }
    }
}
