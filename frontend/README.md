# Motoklub Bezbednost Frontend

React + TypeScript frontend application for managing motorcycle club members, motorcycles, equipment, and training records.

## Prerequisites

- Node.js 18+ and npm/yarn
- AWS Cognito User Pool configured

## Setup

1. Install dependencies:
```bash
npm install
```

2. Create `.env` file with your AWS Cognito configuration:
```
VITE_AWS_COGNITO_USER_POOL_ID=your-user-pool-id
VITE_AWS_COGNITO_CLIENT_ID=your-client-id
VITE_AWS_REGION=us-east-1
VITE_API_BASE_URL=http://localhost:5000/api
```

## Running the Application

```bash
npm run dev
```

The app will be available at `http://localhost:3000`

## Building for Production

```bash
npm run build
```

The built files will be in the `dist` directory, ready to be deployed to S3.

