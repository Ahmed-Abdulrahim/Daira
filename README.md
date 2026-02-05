<!-- Project Badges -->
<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8.0" />
  <img src="https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C# 12" />
  <img src="https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
  <img src="https://img.shields.io/badge/Entity%20Framework-8.0-512BD4?style=for-the-badge&logo=nuget&logoColor=white" alt="Entity Framework Core" />
  <img src="https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" alt="JWT Auth" />
  <img src="https://img.shields.io/badge/SignalR-Real--Time-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="SignalR" />
  <img src="https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
</p>

<!-- Project Title -->
<h1 align="center">🌐 Daira</h1>

<p align="center">
  <strong>A Modern Social Media Platform Backend Built with Clean Architecture</strong>
</p>

<p align="center">
  Daira is a production-ready, enterprise-grade social media platform backend API built with ASP.NET Core 8.0, following Clean Architecture principles and SOLID design patterns. The platform provides comprehensive social networking features including real-time messaging, post management, user interactions, and notification systems, all designed for scalability, maintainability, and extensibility.
</p>

---

## 📑 Table of Contents

- [Overview](#-overview)
- [Key Highlights](#-key-highlights)
- [Features](#-features)
  - [Authentication & Authorization](#authentication--authorization)
  - [User Management](#user-management)
  - [Posts & Feed](#posts--feed)
  - [Social Interactions](#social-interactions)
  - [Real-Time Messaging](#real-time-messaging)
  - [Notifications](#notifications)
  - [Administration](#administration)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Technologies Used](#-technologies-used)
- [Prerequisites](#-prerequisites)
- [Installation & Setup](#-installation--setup)
- [Configuration](#-configuration)
- [API Endpoints](#-api-endpoints)
- [Real-Time Hub Methods](#-real-time-hub-methods)
- [Usage Examples](#-usage-examples)
- [Contributing](#-contributing)

---

## 🔭 Overview

### What is Daira?

Daira (Arabic for "Circle") is a comprehensive social media platform backend designed to power modern social networking applications. It provides all the essential building blocks for creating engaging social experiences, from user authentication to real-time messaging.

### Business Domain

The platform operates in the **social networking domain**, enabling users to:
- Connect with friends through follow and friendship systems
- Share content through posts with likes and comments
- Communicate in real-time through direct and group messaging
- Stay informed through an intelligent notification system

### Target Users

- **Frontend Developers**: Building web or mobile social media applications
- **Startups**: Looking for a robust, scalable backend foundation
- **Enterprises**: Needing internal social networking solutions
- **Educational Projects**: Learning Clean Architecture with real-world examples

### Problem Solved

Daira addresses the complexity of building social media backends by providing:
- **Pre-built social features**: No need to reinvent common patterns
- **Scalable architecture**: Designed for growth from day one
- **Real-time capabilities**: Native WebSocket support for instant updates
- **Security-first approach**: JWT authentication with refresh tokens
- **Clean codebase**: Easy to extend and maintain

---

## ✨ Key Highlights

- 🔐 **Enterprise-Grade Security**: JWT authentication with access/refresh token rotation, email confirmation, and password reset flows
- 🏗️ **Clean Architecture**: Strict separation of concerns with Domain, Application, Infrastructure, and Presentation layers
- ⚡ **Real-Time Communication**: SignalR-powered messaging with typing indicators, read receipts, and presence detection
- 📊 **Specification Pattern**: Flexible and reusable query specifications for complex data filtering
- 🔄 **Unit of Work Pattern**: Transactional consistency across repository operations
- 📧 **Email Integration**: MailKit-based email service for confirmations and notifications
- ✅ **Input Validation**: FluentValidation for robust request validation
- 🗺️ **Object Mapping**: AutoMapper for clean DTO transformations
- 📝 **API Documentation**: Swagger/OpenAPI with JWT authorization support
- 🌐 **CORS Support**: Configurable cross-origin resource sharing

---

## 🚀 Features

### Authentication & Authorization

| Feature | Description |
|---------|-------------|
| User Registration | Complete registration with email confirmation requirement |
| Login/Logout | Secure authentication with JWT token generation |
| Refresh Tokens | Silent token renewal with 7-day refresh token validity |
| Email Confirmation | Token-based email verification flow |
| Password Reset | Secure forgot/reset password with email tokens |
| Resend Confirmation | Ability to resend confirmation emails |

### User Management

| Feature | Description |
|---------|-------------|
| Account Management | User profile and account settings |
| Role-Based Access | Flexible role assignment and management |
| User Roles Query | Retrieve roles for specific users |
| Role CRUD | Create, read, update, and delete roles |

### Posts & Feed

| Feature | Description |
|---------|-------------|
| Create Posts | Rich content post creation |
| Update/Delete Posts | Full post lifecycle management |
| Personal Feed | Get feed from followed users with pagination |
| User Posts | Retrieve all posts for a specific user |
| Like/Unlike | Toggle post likes |
| Post Likes List | Retrieve all users who liked a post |

### Social Interactions

| Feature | Description |
|---------|-------------|
| Follow System | Follow/unfollow users |
| Followers List | Get all followers of current user |
| Following List | Get all users the current user follows |
| Friend Requests | Send/accept/decline friend requests |
| Friends List | Retrieve accepted friendships |
| Unfriend | Remove existing friendships |
| Comments | Add, update, delete comments on posts |

### Real-Time Messaging

| Feature | Description |
|---------|-------------|
| Conversations | Create and manage direct/group conversations |
| Send Messages | Real-time message delivery via SignalR |
| Message History | Retrieve conversation message history |
| Typing Indicators | Real-time typing status broadcast |
| Read Receipts | Mark messages/conversations as read |
| User Presence | Online/offline status detection |
| Conversation Groups | Manage participants in group chats |

### Notifications

| Feature | Description |
|---------|-------------|
| Push Notifications | Real-time notification delivery |
| Notification Feed | Paginated notification retrieval |
| Unread Count | Get count of unread notifications |
| Mark as Read | Mark individual or all notifications read |
| Create Notifications | System-triggered notification creation |
| Delete Notifications | Remove unwanted notifications |

### Administration

| Feature | Description |
|---------|-------------|
| Role Management | Complete CRUD for application roles |
| User Role Assignment | Assign/remove roles from users |
| Users in Role | Query users by role membership |

---

## 🏛️ Architecture

Daira follows **Clean Architecture** (also known as Onion Architecture), ensuring:
- Independence from frameworks and databases
- Testability at every layer
- Clear separation of concerns
- Dependency inversion principle compliance

```
┌─────────────────────────────────────────────────────────────────┐
│                        PRESENTATION                              │
│                      (Daira.Api)                                │
│   ┌─────────────┐ ┌─────────────┐ ┌─────────────┐               │
│   │ Controllers │ │    Hubs     │ │ Middlewares │               │
│   └──────┬──────┘ └──────┬──────┘ └──────┬──────┘               │
└──────────┼───────────────┼───────────────┼──────────────────────┘
           │               │               │
           ▼               ▼               ▼
┌─────────────────────────────────────────────────────────────────┐
│                       INFRASTRUCTURE                             │
│                   (Daira.Infrastructure)                        │
│   ┌─────────────┐ ┌─────────────┐ ┌─────────────┐               │
│   │  Services   │ │Repositories │ │ Persistence │               │
│   └──────┬──────┘ └──────┬──────┘ └──────┬──────┘               │
│   ┌─────────────┐ ┌─────────────┐ ┌─────────────┐               │
│   │Specifications│ │ UnitOfWork │ │  Settings   │               │
│   └─────────────┘ └─────────────┘ └─────────────┘               │
└──────────┼───────────────┼───────────────┼──────────────────────┘
           │               │               │
           ▼               ▼               ▼
┌─────────────────────────────────────────────────────────────────┐
│                        APPLICATION                               │
│                    (Daira.Application)                          │
│   ┌─────────────┐ ┌─────────────┐ ┌─────────────┐               │
│   │    DTOs     │ │ Interfaces  │ │  Response   │               │
│   └─────────────┘ └─────────────┘ └─────────────┘               │
│   ┌─────────────┐ ┌─────────────┐ ┌─────────────┐               │
│   │   Mapping   │ │ Validation  │ │   Shared    │               │
│   └─────────────┘ └─────────────┘ └─────────────┘               │
└──────────┼───────────────────────────────┼──────────────────────┘
           │                               │
           ▼                               ▼
┌─────────────────────────────────────────────────────────────────┐
│                          DOMAIN                                  │
│                         (Domain)                                │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │                     Entities                            │   │
│   │  User, Post, Comment, Like, Follower, Friendship,       │   │
│   │  Conversation, Message, Notification, etc.              │   │
│   └─────────────────────────────────────────────────────────┘   │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │                    Exceptions                           │   │
│   └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

| Layer | Project | Responsibility |
|-------|---------|----------------|
| **Domain** | `Domain` | Core business entities, exceptions, and domain logic. Zero external dependencies. |
| **Application** | `Daira.Application` | DTOs, service interfaces, validation rules, mapping profiles. Defines contracts. |
| **Infrastructure** | `Daira.Infrastructure` | Service implementations, repositories, database context, external integrations. |
| **Presentation** | `Daira.Api` | REST controllers, SignalR hubs, middleware, API configuration. |

---

## 📁 Project Structure

```
Daira/
├── 📂 Domain/                           # Core Domain Layer
│   ├── 📂 Models/                       # Domain Entities
│   │   ├── 📂 AuthModel/                # Identity-related models
│   │   │   ├── AppUser.cs               # Application user entity
│   │   │   ├── AppRole.cs               # Application role entity
│   │   │   └── RefreshToken.cs          # JWT refresh token entity
│   │   ├── BaseEntity.cs                # Base entity with common properties
│   │   ├── Post.cs                      # Post entity
│   │   ├── Comment.cs                   # Comment entity
│   │   ├── Like.cs                      # Like entity
│   │   ├── Follower.cs                  # Follower relationship entity
│   │   ├── Friendship.cs                # Friendship entity
│   │   ├── Conversation.cs              # Chat conversation entity
│   │   ├── ConversationParticipant.cs   # Conversation membership
│   │   ├── Message.cs                   # Chat message entity
│   │   └── Notification.cs              # Notification entity
│   ├── 📂 Exceptions/                   # Domain exceptions
│   ├── GlobalUsing.cs                   # Global namespace imports
│   └── Domain.csproj
│
├── 📂 Daira.Application/                # Application Layer
│   ├── 📂 DTOs/                         # Data Transfer Objects
│   │   ├── 📂 AuthDto/                  # Authentication DTOs
│   │   ├── 📂 PostModule/               # Post-related DTOs
│   │   ├── 📂 CommentModule/            # Comment DTOs
│   │   ├── 📂 ConversationModule/       # Conversation DTOs
│   │   ├── 📂 MessageModule/            # Message DTOs
│   │   ├── 📂 FollowerModule/           # Follower DTOs
│   │   ├── 📂 FriendshipModule/         # Friendship DTOs
│   │   ├── 📂 NotifcationModule/        # Notification DTOs
│   │   ├── 📂 LikeModule/               # Like DTOs
│   │   └── 📂 RolesDto/                 # Role management DTOs
│   ├── 📂 Interfaces/                   # Service Interfaces
│   │   ├── 📂 Auth/                     # Auth service interfaces
│   │   ├── 📂 PostModule/               # Post service interface
│   │   ├── 📂 CommentModule/            # Comment service interface
│   │   ├── 📂 ConversationModule/       # Conversation service interface
│   │   ├── 📂 MessageModule/            # Message service interface
│   │   ├── 📂 FollowerModule/           # Follower service interface
│   │   ├── 📂 FriendshipModule/         # Friendship service interface
│   │   ├── 📂 NotificationModule/       # Notification service interface
│   │   ├── IRepository.cs               # Generic repository interface
│   │   ├── IUnitOfWork.cs               # Unit of work interface
│   │   ├── ISpecefication.cs            # Specification pattern interface
│   │   ├── IUserRepository.cs           # User-specific repository
│   │   ├── IChatHubClient.cs            # SignalR hub client interface
│   │   └── IConnectionService.cs        # Connection tracking interface
│   ├── 📂 Response/                     # Strongly-typed response classes
│   ├── 📂 Mapping/                      # AutoMapper profiles
│   ├── 📂 Validation/                   # FluentValidation validators
│   ├── 📂 Shared/                       # Shared utilities
│   ├── DependencyInjection.cs           # Application DI registration
│   └── Daira.Application.csproj
│
├── 📂 Daira.Infrastructure/             # Infrastructure Layer
│   ├── 📂 Persistence/                  # Database access
│   │   ├── 📂 Configurations/           # EF entity configurations
│   │   ├── 📂 Migrations/               # EF migrations
│   │   └── ApplicationDbContext.cs      # EF DbContext
│   ├── 📂 Repositories/                 # Repository implementations
│   ├── 📂 Services/                     # Service implementations
│   │   ├── 📂 AuthService/              # Authentication services
│   │   ├── 📂 PostService/              # Post service
│   │   ├── 📂 CommentService/           # Comment service
│   │   ├── 📂 ConversationService/      # Conversation service
│   │   ├── 📂 MessageService/           # Message service
│   │   ├── 📂 FollowService/            # Follow service
│   │   ├── 📂 FriendShipService/        # Friendship service
│   │   ├── 📂 NotificationService/      # Notification service
│   │   └── ConnectionService.cs         # SignalR connection tracking
│   ├── 📂 UnitOfWork/                   # Unit of Work implementation
│   ├── 📂 Specefication/                # Specification implementations
│   ├── 📂 Settings/                     # Configuration POCOs
│   ├── DependencyInjection.cs           # Infrastructure DI registration
│   └── Daira.Infrastructure.csproj
│
├── 📂 Daira.Api/                        # Presentation Layer
│   ├── 📂 Controllers/                  # API Controllers
│   │   ├── AuthController.cs            # Authentication endpoints
│   │   ├── AccountController.cs         # Account management
│   │   ├── PostController.cs            # Post CRUD & interactions
│   │   ├── CommentController.cs         # Comment management
│   │   ├── FollowController.cs          # Follow/unfollow
│   │   ├── FriendshipController.cs      # Friend requests
│   │   ├── ConversationController.cs    # Chat conversations
│   │   ├── MessageController.cs         # Message operations
│   │   ├── NotificationController.cs    # Notification management
│   │   └── RoleController.cs            # Role administration
│   ├── 📂 Hub/                          # SignalR Hubs
│   │   └── ChatHub.cs                   # Real-time messaging hub
│   ├── 📂 Middlewares/                  # Custom middleware
│   ├── 📂 Extensions/                   # Extension methods
│   ├── 📂 Errors/                       # Error handling
│   ├── Program.cs                       # Application entry point
│   ├── appsettings.json                 # Application configuration
│   └── Daira.Api.csproj
│
├── Daira.sln                            # Solution file
├── .gitignore
└── README.md
```

---

## 🛠️ Technologies Used

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 8.0 | Core framework and runtime |
| ASP.NET Core | 8.0 | Web API framework |
| Entity Framework Core | 8.0.23 | ORM for database operations |
| SQL Server | 2022 | Primary relational database |
| ASP.NET Core Identity | 8.0.23 | User authentication and authorization |
| JWT Bearer | 8.0.23 | Token-based authentication |
| SignalR | 8.0 | Real-time WebSocket communication |
| AutoMapper | 13.0.1 | Object-to-object mapping |
| FluentValidation | 12.1.1 | Request validation |
| MailKit | 4.14.1 | Email sending service |
| Swashbuckle | 6.6.2 | Swagger/OpenAPI documentation |

---

## 📋 Prerequisites

Before running Daira, ensure you have the following installed:

| Requirement | Version | Download |
|-------------|---------|----------|
| .NET SDK | 8.0 or later | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) |
| SQL Server | 2019+ or LocalDB | [Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) |
| Visual Studio | 2022 (recommended) | [Download](https://visualstudio.microsoft.com/) |
| VS Code | Latest (alternative) | [Download](https://code.visualstudio.com/) |
| Git | Latest | [Download](https://git-scm.com/) |

---

## 🚀 Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/Ahmed-Abdulrahim/Daira.git
cd Daira
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure the Database

Update the connection string in `Daira.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "conn1": "Data Source=YOUR_SERVER;Initial Catalog=Daira;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Apply Database Migrations

```bash
cd Daira.Api
dotnet ef database update --project ../Daira.Infrastructure
```

Or let the application apply migrations automatically on startup (configured by default).

### 5. Run the Application

```bash
dotnet run --project Daira.Api
```

### 6. Access the API

- **Swagger UI**: https://localhost:7171/swagger
- **API Base URL**: https://localhost:7171/api
- **SignalR Hub**: https://localhost:7171/chatHub

---

## ⚙️ Configuration

### appsettings.json Structure

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "conn1": "Data Source=.;Initial Catalog=Daira;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLongForHS256Algorithm!",
    "Issuer": "AuthFeature.Api",
    "Audience": "AuthFeature.Client",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "SenderEmail": "noreply@daira.com",
    "SenderName": "Daira",
    "UseSsl": true,
    "BaseUrl": "https://localhost:7171"
  }
}
```

### Configuration Details

| Section | Key | Description |
|---------|-----|-------------|
| **ConnectionStrings** | `conn1` | SQL Server connection string |
| **JwtSettings** | `SecretKey` | HMAC-SHA256 secret (min 32 chars) |
| **JwtSettings** | `Issuer` | Token issuer identifier |
| **JwtSettings** | `Audience` | Token audience identifier |
| **JwtSettings** | `AccessTokenExpirationMinutes` | Access token TTL |
| **JwtSettings** | `RefreshTokenExpirationDays` | Refresh token TTL |
| **EmailSettings** | `SmtpHost` | SMTP server hostname |
| **EmailSettings** | `SmtpPort` | SMTP port (587 for TLS) |
| **EmailSettings** | `SmtpUser` | SMTP authentication username |
| **EmailSettings** | `SmtpPassword` | SMTP authentication password |
| **EmailSettings** | `SenderEmail` | From email address |
| **EmailSettings** | `SenderName` | From display name |
| **EmailSettings** | `BaseUrl` | Application base URL for email links |

### Environment Variables

For production, override sensitive settings using environment variables:

```bash
ConnectionStrings__conn1="your-production-connection-string"
JwtSettings__SecretKey="your-production-secret-key"
EmailSettings__SmtpPassword="your-smtp-password"
```

---

## 📡 API Endpoints

### Authentication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/Auth/register` | Register a new user | ❌ |
| `POST` | `/api/Auth/Login` | Authenticate and get tokens | ❌ |
| `GET` | `/api/Auth/confirm-email` | Confirm email with token | ❌ |
| `POST` | `/api/Auth/refresh-token` | Refresh access token | ❌ |
| `POST` | `/api/Auth/Logout` | Invalidate refresh token | ✅ |
| `POST` | `/api/Auth/Forget-password` | Request password reset | ❌ |
| `POST` | `/api/Auth/reset-password` | Reset password with token | ❌ |
| `POST` | `/api/Auth/confirm-Email` | Resend confirmation email | ❌ |

### Posts

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/Post/create-post` | Create a new post | ✅ |
| `GET` | `/api/Post/get-post/{id}` | Get post by ID | ✅ |
| `PUT` | `/api/Post/update-post/{id}` | Update a post | ✅ |
| `DELETE` | `/api/Post/delete-post/{id}` | Delete a post | ✅ |
| `GET` | `/api/Post/get-posts` | Get current user's posts | ✅ |
| `GET` | `/api/Post/feed` | Get personalized feed | ✅ |
| `POST` | `/api/Post/like-post/{id}` | Like a post | ✅ |
| `DELETE` | `/api/Post/Unlike-post/{id}` | Unlike a post | ✅ |
| `GET` | `/api/Post/post-likes/{id}` | Get post's likes | ✅ |

### Comments

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/Comment/get-comments/{postId}` | Get comments by post | ✅ |
| `POST` | `/api/Comment/add-comment/{id}` | Add comment to post | ✅ |
| `PUT` | `/api/Comment/update-commnet/{id}` | Update a comment | ✅ |
| `DELETE` | `/api/Comment/delete-comment/{id}` | Delete a comment | ✅ |

### Follow System

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/Follow/follow-user/{id}` | Follow a user | ✅ |
| `POST` | `/api/Follow/unfollow-user/{id}` | Unfollow a user | ✅ |
| `GET` | `/api/Follow/get-followers` | Get user's followers | ✅ |
| `GET` | `/api/Follow/get-following` | Get user's following | ✅ |

### Friendships

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/Friendship/GetAll-friendRequest` | Get pending requests | ✅ |
| `GET` | `/api/Friendship/GetAll-friends` | Get all friends | ✅ |
| `POST` | `/api/Friendship/request-friendship/{addresseeId}` | Send friend request | ✅ |
| `PUT` | `/api/Friendship/accept-friendship/{id}` | Accept request | ✅ |
| `PUT` | `/api/Friendship/decline-friendship/{id}` | Decline request | ✅ |
| `POST` | `/api/Friendship/unFriend/{id}` | Remove friend | ✅ |

### Conversations

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/Conversation/create-conversation` | Create conversation | ✅ |
| `GET` | `/api/Conversation/get-conversation/{id}` | Get conversation | ✅ |
| `GET` | `/api/Conversation/get-User-conversation` | Get user's conversations | ✅ |
| `POST` | `/api/Conversation/add-participant/{userId}-to-conversation{id}` | Add participant | ✅ |
| `DELETE` | `/api/Conversation/remove-participant/{userId}-to-conversation{id}` | Remove participant | ✅ |

### Messages

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/Message/get-message/{id}` | Get message by ID | ✅ |
| `GET` | `/api/Message/get-all-messages/{conversationId}` | Get conversation messages | ✅ |
| `POST` | `/api/Message/send-Message` | Send a message | ✅ |
| `PUT` | `/api/Message/read-message/{id}` | Mark message as read | ✅ |
| `PUT` | `/api/Message/read-conversation/{id}` | Mark conversation as read | ✅ |
| `DELETE` | `/api/Message/delete-message/{id}` | Delete a message | ✅ |

### Notifications

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/Notification` | Get paginated notifications | ✅ |
| `GET` | `/api/Notification/unread-count` | Get unread count | ✅ |
| `POST` | `/api/Notification` | Create notification | ✅ |
| `PUT` | `/api/Notification/read/{id}` | Mark as read | ✅ |
| `PUT` | `/api/Notification/read-all` | Mark all as read | ✅ |
| `DELETE` | `/api/Notification/{id}` | Delete notification | ✅ |

### Roles (Admin)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/api/Role` | Get all roles | ✅ |
| `GET` | `/api/Role/getById/{id}` | Get role by ID | ✅ |
| `GET` | `/api/Role/getByName/{Name}` | Get role by name | ✅ |
| `POST` | `/api/Role/CreateRole` | Create a role | ✅ |
| `PUT` | `/api/Role/updateRole` | Update a role | ✅ |
| `DELETE` | `/api/Role/deleteRole/{Name}` | Delete a role | ✅ |
| `GET` | `/api/Role/GetUserRole/{userId}` | Get user's roles | ✅ |
| `GET` | `/api/Role/UsersInRole/{Name}` | Get users in role | ✅ |
| `POST` | `/api/Role/AssignRole/{Name}/User/{userId}` | Assign role to user | ✅ |
| `DELETE` | `/api/Role/user/{userId}/remove/{Name}` | Remove role from user | ✅ |

---

## 📡 Real-Time Hub Methods

Connect to the SignalR hub at `/chatHub` with a valid JWT token.

### Client → Server (Invocable Methods)

| Method | Parameters | Description |
|--------|------------|-------------|
| `JoinConversation` | `conversationId: Guid` | Join a conversation group |
| `LeaveConversation` | `conversationId: Guid` | Leave a conversation group |
| `SendMessage` | `conversationId: Guid, content: string` | Send a message |
| `StartTyping` | `conversationId: Guid` | Broadcast typing start |
| `StopTyping` | `conversationId: Guid` | Broadcast typing stop |
| `MarkAsRead` | `conversationId: Guid` | Mark conversation as read |
| `MarkNotificationAsRead` | `notificationId: Guid` | Mark notification as read |
| `MarkAllNotificationsAsRead` | - | Mark all notifications read |

### Server → Client (Receivable Events)

| Event | Parameters | Description |
|-------|------------|-------------|
| `ReceiveMessage` | `MessageResponse` | New message received |
| `UserTyping` | `conversationId, userId, displayName` | User started typing |
| `UserStoppedTyping` | `conversationId, userId` | User stopped typing |
| `MessagesRead` | `conversationId, userId` | Messages marked as read |
| `UserOnline` | `userId` | User came online |
| `UserOffline` | `userId` | User went offline |
| `AddedToConversation` | `ConversationResponse` | Added to a conversation |
| `UserJoinedConversation` | `conversationId, ParticipantResponse` | User joined group |
| `UserLeftConversation` | `conversationId, userId` | User left group |
| `NotificationRead` | `notificationId` | Notification marked read |
| `AllNotificationsRead` | - | All notifications read |

---

## 📝 Usage Examples

### User Registration

```bash
curl -X POST https://localhost:7171/api/Auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePassword123!",
    "confirmPassword": "SecurePassword123!",
    "firstName": "John",
    "lastName": "Doe"
  }'
```

### User Login

```bash
curl -X POST https://localhost:7171/api/Auth/Login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePassword123!"
  }'
```

**Response:**
```json
{
  "succeeded": true,
  "message": "Login successful",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "d4f8c1b2-9876-4a3d-8b7c-6e5f4a3b2c1d",
    "expiresAt": "2024-01-15T14:30:00Z"
  }
}
```

### Accessing Protected Endpoints

```bash
curl -X POST https://localhost:7171/api/Post/create-post \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "content": "Hello, Daira community! This is my first post."
  }'
```

### Refresh Access Token

```bash
curl -X POST https://localhost:7171/api/Auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "d4f8c1b2-9876-4a3d-8b7c-6e5f4a3b2c1d"
  }'
```

### Get Personalized Feed

```bash
curl -X GET "https://localhost:7171/api/Post/feed?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### SignalR Connection (JavaScript)

```javascript
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7171/chatHub", {
    accessTokenFactory: () => "your-jwt-token"
  })
  .withAutomaticReconnect()
  .build();

// Event handlers
connection.on("ReceiveMessage", (message) => {
  console.log("New message:", message);
});

connection.on("UserTyping", (conversationId, userId, displayName) => {
  console.log(`${displayName} is typing...`);
});

// Connect
await connection.start();

// Send message
await connection.invoke("SendMessage", conversationId, "Hello!");

// Typing indicators
await connection.invoke("StartTyping", conversationId);
await connection.invoke("StopTyping", conversationId);
```

---

## 🤝 Contributing

We welcome contributions to Daira! Please follow these steps:

### 1. Fork the Repository

Click the "Fork" button at the top right of this page.

### 2. Create a Feature Branch

```bash
git checkout -b feature/AmazingFeature
```

### 3. Commit Your Changes

```bash
git commit -m 'Add some AmazingFeature'
```

Follow these commit conventions:
- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `refactor:` Code refactoring
- `test:` Adding tests
- `chore:` Maintenance tasks

### 4. Push to Your Branch

```bash
git push origin feature/AmazingFeature
```

### 5. Open a Pull Request

Create a PR with a clear description of changes.

### Coding Standards

- Follow C# naming conventions
- Use async/await for all I/O operations
- Add XML documentation for public APIs
- Write unit tests for new features
- Keep methods small and focused (SRP)
- Use dependency injection
- Follow the existing project structure

---
>
