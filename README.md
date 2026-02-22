*An Updated version of this project has been merged with the Community API Project and can be found under DecsPage and Tested at [api.decspage.com](https://api.decspage.com/)*

A high-performance .NET 10 asynchronous worker service designed to handle data-intensive background tasks.
Key Features
Job Polling Architecture: Monitors an Azure SQL Database for pending tasks using a "Pending Job" queue logic.
Data Transformation: Processes player data into standardized CSV formats.
Cloud Storage Integration: Automatically uploads processed files to Azure Blob Storage.
