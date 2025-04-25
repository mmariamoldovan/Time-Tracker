# TimeTracker API

TimeTracker is a client-server application for tracking work time.

## How to Run the Application

Execute the `deploy.bat` script to build and start both the server and client.

## API Endpoints

Base URL: `http://localhost:5033/api/TimeTracker`

### Sessions

- `POST /sessions/start` - Start a new work session
- `PUT /sessions/stop-active` - Stop the current active session
- `GET /sessions/active` - Get the active session
- `GET /sessions` - Get all sessions

### Reports

- `GET /reports/daily?date=2023-06-15` - Daily report
- `GET /reports/weekly?startDate=2023-06-12` - Weekly report
- `GET /reports/monthly?year=2023&month=6` - Monthly report

## Testing with Postman

1. Import the `TimeTracker.postman_collection.json` collection
2. Test the API with the predefined requests