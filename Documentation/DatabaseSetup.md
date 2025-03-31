# Database Setup Guide

This guide will help you set up the required database tables for the FusionWebGL AI package.

## MySQL Tables

### Users Table

```sql
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_login TIMESTAMP NULL,
    metadata JSON,
    is_active BOOLEAN DEFAULT TRUE
);
```

### NPCs Table

```sql
CREATE TABLE npcs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    type VARCHAR(50) NOT NULL,
    faction VARCHAR(50) NOT NULL,
    personality TEXT,
    background TEXT,
    nameplate_color VARCHAR(7),
    is_interactive BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL ON UPDATE CURRENT_TIMESTAMP
);
```

### Dialogue Sessions Table

```sql
CREATE TABLE dialogue_sessions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    session_id VARCHAR(36) NOT NULL UNIQUE,
    user_id INT NOT NULL,
    npc_id INT NOT NULL,
    started_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ended_at TIMESTAMP NULL,
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (npc_id) REFERENCES npcs(id)
);
```

### Dialogue Messages Table

```sql
CREATE TABLE dialogue_messages (
    id INT AUTO_INCREMENT PRIMARY KEY,
    session_id VARCHAR(36) NOT NULL,
    message TEXT NOT NULL,
    is_player BOOLEAN NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (session_id) REFERENCES dialogue_sessions(session_id)
);
```

## API Documentation (Swagger)

### Authentication Endpoints

#### POST /api/auth/register

```json
{
  "username": "string",
  "email": "string",
  "password": "string"
}
```

#### POST /api/auth/login

```json
{
  "username": "string",
  "password": "string"
}
```

### NPC Endpoints

#### GET /api/npcs

Returns a list of all NPCs

#### GET /api/npcs/{id}

Returns details of a specific NPC

#### POST /api/npcs

Create a new NPC

```json
{
  "name": "string",
  "type": "string",
  "faction": "string",
  "personality": "string",
  "background": "string",
  "nameplateColor": "string",
  "isInteractive": boolean
}
```

### Dialogue Endpoints

#### POST /api/chat/send

Send a message to an NPC

```json
{
  "message": "string",
  "sessionId": "string",
  "npcType": "string",
  "playerContext": "string"
}
```

#### GET /api/chat/sessions/{sessionId}

Get dialogue history for a session

## Environment Variables

Create a `.env` file in your server directory:

```env
DB_HOST=localhost
DB_USER=your_username
DB_PASSWORD=your_password
DB_NAME=fusionwebgl_ai
JWT_SECRET=your_jwt_secret
API_PORT=3001
```

## Server Setup

1. Install Node.js and npm
2. Clone the server repository
3. Install dependencies:

   ```bash
   npm install
   ```

4. Set up environment variables
5. Run database migrations:

   ```bash
   npm run migrate
   ```

6. Start the server:

   ```bash
   npm start
   ```

## Unity Configuration

1. Update the API base URL in your Unity project:
   - Open `Runtime/AI/ChatGPTService.cs`
   - Update `API_BASE_URL` to match your server
   - Open `Runtime/Auth/LoginManager.cs`
   - Update `API_BASE_URL` to match your server

2. Configure your Fusion App ID:
   - Open Fusion Hub
   - Enter your App ID
   - Save settings

## Security Considerations

1. Always use HTTPS in production
2. Implement rate limiting
3. Use secure password hashing
4. Implement JWT token expiration
5. Validate all user inputs
6. Use prepared statements for database queries
7. Implement CORS policies
8. Use environment variables for sensitive data
