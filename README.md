Background Worker reference project. Worker runs in the background checking SQL Database for pending jobs before processing them. Currently can only dump a players list from the database into a CSV format and upload to Azure Blob storage  
NOTE: For this to be hosted on Azure for testing it has has been converted to a Manually triggered worker. Triggered on a job creation with a 25 second timeout

Try it out: [worker.decspage.com](https://worker.decspage.com/)

Documentation:

## GET /jobs
Lists all pending jobs

**Example:**
- /jobs/

## GET /jobs/id
List specific job by Database ID

**Params:**
- ID

**Example:**
- /jobs/7

## GET /jobs/id/download
Download file associated with the job

**Params:** 
- ID

**Example:**
- /jobs/7/download

## GET /players/dump
Creates a Job to Dump the Players list into a CSV format

**Example:**
- /players/dump
