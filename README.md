# batch-processing-service

This repository contains the code for a .Net Core REST service that allows the user to start different jobs.

## How to run:

In order to run this API, you're gonna need Docker and Docker Compose installed in your environment. Then its just a matter of running `docker compose build` and then `docker compose up` at the root folder of the repository.

After this, the service will be listening at http://localhost:5001.

You'll also be able to access the swagger, the job dashboard and the log server. You can read more about this below.

## The API

The service has 2 endpoints.

#### /Migration/ScheduleMigration

This enpoint starts the jobs. You can see the request model if you browse http://localhost:5001/swagger/index.html in your browser

Specifically there are two types of jobs:

**0 - Bulk jobs:** They process all the provided data in sequence. For this, the process should not stop if one fails. There is no rollback.

**1 - Batch jobs:** They process all the provided data in sequence. For this, the process stops, if one fails. There is no rollback

The endpoint accepts the data to process in CSV format. You can use the samples in the Samples folder of this repo to test it.

The response is a simple UUID that identifies the job scheduled as a result of this request.


#### /Migration/GetMigrationStatus/{jobId}

Through this endpoint you can asyncronously check the status of the data being processed by a job identified with a specific jobId.

The response shows the total number of items being processed, the number of items that have already been processed and the number of items that gave an error.

## Logs

Along with the service, there is a logging server listening to the logger output of the API. If you ran the service using docker-compose, you should be able to access it by going to http://localhost:5002 in your browser.

Once in there, you can check the logs of each job execution by simply writing the jobId in the search bar.

## Job Dashboard

There is also a dashboard dedicated to monitoring of all job executions. 

You can see it at http://localhost:5001/dashboard

There you can see the history of all executed jobs, along with several charts displaying different types of insights regarding the jobs executed.

## To Improve

This is a work in progress, so there are many things that can be improved. For any requests you can contact me.