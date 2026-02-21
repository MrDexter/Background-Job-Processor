A high-performance .NET 10 asynchronous worker service designed to handle data-intensive background tasks.
Key Features
Job Polling Architecture: Monitors an Azure SQL Database for pending tasks using a "Pending Job" queue logic.
Data Transformation: Processes player data into standardized CSV formats.
Cloud Storage Integration: Automatically uploads processed files to Azure Blob Storage.
Azure Cost-Optimization: To minimize cloud resource consumption, the worker is architected as a manually-triggered service for live testing, demonstrating a "Pay-As-You-Go" infrastructure mindset.

Try it and See Documentation here: [worker.decspage.com](https://worker.decspage.com/)
