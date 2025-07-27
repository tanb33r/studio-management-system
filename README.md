# Studio Booking Management System

A complete studio booking management system built with .NET 8 backend and Angular frontend, featuring JWT authentication, user registration, and login functionality.

## 🚀 Features

- **User Registration & Login**: Secure user authentication system
- **JWT Authentication**: Token-based authentication with refresh tokens  
- **Studio Booking Management**: Complete booking management system
- **Entity Framework Core**: Database management with SQL Server
- **CORS Support**: Configured for frontend-backend communication
- **Security**: Password hashing with BCrypt

## 🏗️ Project Structure

## 🛠️ Technologies Used

### Backend (.NET 8)
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- BCrypt.NET for password hashing

### Frontend (Angular)
- Angular 20+ (Latest)
- TypeScript
- SCSS for styling
- Reactive Forms
- HTTP Client
- Router with Guards

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (Latest LTS version)
- [Angular CLI](https://angular.io/cli) (Latest version)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB is sufficient)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd studio-booking-management
```

### 2. Backend Setup

Navigate to the backend directory:

```bash
cd StudioBookingManagementBackend/StudioBookingManagement.WebApi
```

**Install dependencies:**
```bash
dotnet restore
```

**Update Database Connection String:**

Update the connection string in `appsettings.json` to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudioBookingManagementDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**Create and Apply Database Migration:**

```bash
dotnet ef database update
```

**Run the Backend:**

```bash
dotnet run
```

The API will be available at `https://localhost:7247`

### 3. Frontend Setup

Navigate to the frontend directory:

```bash
cd ../../studio-booking-management-frontend
```

**Install dependencies:**
```bash
npm install
```

**Update API URL (if needed):**

Update the API URL in `src/app/services/auth.service.ts` if your backend runs on a different port:

```typescript
private apiUrl = 'https://localhost:7247/api/auth';
```

**Run the Frontend:**

```bash
ng serve
```

The application will be available at `http://localhost:4200`

## 🎯 Usage

1. **Access the Application**: Open your browser and navigate to `http://localhost:4200`

2. **Register a New Account**:
   - Click "Sign up" on the login page
   - Fill in your details (first name, last name, email, password)
   - Click "Create Account"

3. **Login**:
   - Enter your email and password
   - Click "Sign In"

4. **Dashboard**:
   - View your profile information
   - Explore available features
   - Use the logout button to sign out

## 🔧 Configuration

### Backend Configuration

#### JWT Settings (`appsettings.json`):
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "StudioBookingManagementApi",
    "Audience": "StudioBookingManagementApi",
    "AccessTokenExpirationMinutes": "60"
  }
}
```

#### CORS Configuration:
The backend is configured to allow requests from `http://localhost:4200`. Update the CORS policy in `Program.cs` if needed.

### Frontend Configuration

#### Environment Configuration:
Create environment files for different deployment environments in `src/environments/`.

## 📡 API Endpoints

### Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login user |
| POST | `/api/auth/refresh` | Refresh access token |
| POST | `/api/auth/revoke` | Revoke refresh token |
| GET | `/api/auth/profile` | Get user profile (authenticated) |

### Request/Response Examples

#### Register User
```bash
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}
```

#### Login User
```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "Password123!"
}
```

## 🔒 Security Features

- **Password Hashing**: Passwords are hashed using BCrypt
- **JWT Tokens**: Secure token-based authentication
- **HTTPS**: API configured for HTTPS
- **CORS**: Configured for secure cross-origin requests
- **Input Validation**: Comprehensive validation on both client and server
- **Route Guards**: Protected routes requiring authentication

## 🎨 UI Features

- **Responsive Design**: Works on desktop and mobile devices
- **Modern Interface**: Clean, professional design
- **Loading States**: Visual feedback during API calls
- **Form Validation**: Real-time validation with helpful error messages
- **Smooth Animations**: CSS animations for better user experience

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Issues**:
   - Ensure SQL Server is running
   - Verify connection string in `appsettings.json`
   - Run `dotnet ef database update` to create the database

2. **CORS Errors**:
   - Ensure the frontend URL is properly configured in the backend CORS policy
   - Check that both frontend and backend are running on the correct ports

3. **JWT Token Issues**:
   - Verify the JWT secret key is properly configured
   - Ensure the token hasn't expired

4. **Port Conflicts**:
   - Backend default: `https://localhost:7247`
   - Frontend default: `http://localhost:4200`
   - Change ports if conflicts occur

## 🚀 Deployment

### Backend Deployment
1. Publish the application: `dotnet publish -c Release`
2. Configure production connection string
3. Deploy to your preferred hosting service (Azure, AWS, etc.)

### Frontend Deployment
1. Build for production: `ng build --prod`
2. Deploy the `dist/` folder to your web server

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Commit your changes: `git commit -am 'Add new feature'`
4. Push to the branch: `git push origin feature/new-feature`
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with .NET 8 and Angular
- Uses Entity Framework Core for data access
- JWT authentication implementation
- Modern responsive design principles

---

For questions or support, please open an issue in the repository. 