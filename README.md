# RaftLab Assignment

A modular .NET Core solution that integrates with the public ReqRes API to fetch user data. The project is structured for reusability, testing, and clean separation of concerns.

---

## 📦 Project Structure

RaftLab-Assignment.sln
│
├── RaftLab_Assignment/ # ASP.NET Core Web API (Optional UI/API layer)
│ └── Controllers/ # API endpoints (UsersController)
│
├── RaftLab_Assignment.Core/ # Class Library with business logic
│ ├── Models/ # POCOs like User, ReqResAPISettings
│ ├── Interfaces/ # IReqResInterface
│ ├── Services/ # ReqResClient - API integration logic
│ └── Utils/ # Helper methods like ReadFromJsonDataFieldAsync
│
├── RaftLab_Assignment.ConsoleApp/ # Console App using Core library
│
└── RaftLab_Assignment.Tests/ # xUnit Test Project for Core logic and Controllers


---

## 🔧 Configuration

Update your `appsettings.json` with the following section to configure the ReqRes API client:

```json
"ReqResApi": {
  "BaseUrl": "https://reqres.in/",
  "ApiKey": "<API_KEY>",
  "TimeoutSeconds": 30,
  "MaxRetryAttempts": 3
}```

🛠️ Build Instructions
Clone the Repository
```bat
git clone https://github.com/yourname/RaftLab-Assignment.git
cd RaftLab-Assignment 
```
Restore and Build Solution

```bat
dotnet restore
dotnet build
```

Run the Console App

```bat
cd RaftLab_Assignment.ConsoleApp
dotnet run
```

This fetches and prints users from https://reqres.in.

Run the API (optional)
```bat
cd RaftLab_Assignment
dotnet run
```

Navigate to: https://localhost:{port}/users in your browser or Postman.

🧪 Run Tests

```bat
cd RaftLab_Assignment.Tests
dotnet test
```

Test output will show pass/fail status for all unit tests.
