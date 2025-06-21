# Quiz Application

A comprehensive web-based quiz management system built with ASP.NET Core MVC that allows users to create, manage, and take quizzes with different difficulty levels.

## 🚀 Features

### User Management

- User registration and authentication
- Role-based access control
- User profile management

### Quiz Management

- Create and manage quizzes
- Add questions to quizzes
- Set different difficulty levels for questions
- Organize questions by categories

### Question Management

- Create multiple-choice questions
- Assign difficulty levels (Easy, Medium, Hard)
- Support for various question types
- Question editing and deletion

### Dashboard & Analytics

- Interactive dashboard with statistics
- Quiz performance tracking
- User activity monitoring

### User Interface

- Modern, responsive design using Bootstrap
- Dark/Light theme toggle
- Mobile-friendly interface
- Interactive charts and visualizations

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Charts**: ApexCharts, Chart.js, ECharts
- **Icons**: Bootstrap Icons, Boxicons, Remixicon
- **Data Tables**: Simple DataTables
- **Rich Text Editor**: Quill.js
- **Excel Export**: ClosedXML

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server (Express or higher)
- Visual Studio 2022 or JetBrains Rider (recommended)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd QuizApplication
```

### 2. Database Setup

1. Create a new database named `Quiz` in SQL Server
2. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "ConnectionString": "Data Source=YOUR_SERVER\\SQLEXPRESS;Initial Catalog=Quiz;Integrated Security=true;"
  }
}
```

### 3. Build and Run

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

### 4. Docker (Optional)

```bash
# Build Docker image
docker build -t quiz-application .

# Run container
docker run -p 8080:80 quiz-application
```

## 📁 Project Structure

```
QuizApplication/
├── Controllers/          # MVC Controllers
│   ├── AuthController.cs
│   ├── DashboardService.cs
│   ├── HomeController.cs
│   ├── QuestionController.cs
│   ├── QuestionLevelController.cs
│   ├── QuizController.cs
│   ├── QuizWiseQuestionController.cs
│   └── UserController.cs
├── Models/              # Data Models
│   ├── DashboardViewModel.cs
│   ├── ErrorViewModel.cs
│   ├── LoginViewModel.cs
│   ├── QuestionLevelsModel.cs
│   ├── QuestionModel.cs
│   ├── QuizModel.cs
│   ├── QuizWiseQuestionModel.cs
│   └── UserModel.cs
├── Views/               # Razor Views
│   ├── Auth/           # Authentication views
│   ├── Home/           # Home and dashboard views
│   ├── Question/       # Question management views
│   ├── QuestionLevel/  # Question level views
│   ├── Quiz/           # Quiz management views
│   ├── QuizWiseQuestion/ # Quiz-question relationship views
│   ├── User/           # User management views
│   └── Shared/         # Shared layouts and components
├── DbConfiguration/     # Database configuration and CRUD operations
├── wwwroot/            # Static files (CSS, JS, images)
└── Program.cs          # Application entry point
```

## 🔧 Configuration

### Environment Variables

- `ConnectionStrings:ConnectionString`: Database connection string
- `Logging:LogLevel`: Logging configuration

### Session Configuration

- Session timeout: 30 minutes
- HTTP-only cookies enabled
- Essential cookies for authentication

## 🎯 Usage

1. **Registration/Login**: Users must register or login to access the system
2. **Dashboard**: View statistics and recent activities
3. **Quiz Management**: Create and manage quizzes
4. **Question Management**: Add questions with different difficulty levels
5. **User Management**: Manage user accounts and permissions

## 🔒 Security Features

- Session-based authentication
- HTTP-only cookies
- HTTPS redirection in production
- Input validation and sanitization
- Role-based access control

## 📊 Database Schema

The application uses the following main entities:

- **Users**: User accounts and authentication
- **Quizzes**: Quiz definitions and metadata
- **Questions**: Question content and options
- **QuestionLevels**: Difficulty level definitions
- **QuizWiseQuestions**: Many-to-many relationship between quizzes and questions

## 🚀 Deployment

### Local Development

```bash
dotnet run --environment Development
```

### Production

```bash
dotnet publish -c Release
dotnet run --environment Production
```

### Docker Deployment

```bash
docker build -t quiz-app .
docker run -d -p 80:80 quiz-app
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions, please open an issue in the repository or contact the development team.

## 🔄 Version History

- **v1.0.0**: Initial release with basic quiz management functionality
- **v1.1.0**: Added dashboard and analytics features
- **v1.2.0**: Enhanced UI with modern design and theme support

---

**Note**: Make sure to update the connection string in `appsettings.json` with your SQL Server details before running the application.
