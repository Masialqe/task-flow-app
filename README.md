<!-- ABOUT THE PROJECT -->
## About The Project

TFA (Task Flow Assistant) is a learning project designed to help small teams or solo developers manage their task workflow efficiently. 
Unlike traditional task management tools like Kanban, TFA is built for individuals who prefer a structured workflow and like to focus on one task at a time. Instead of overwhelming users with multiple tasks at once, TFA ensures that:

You only see tasks that are assigned to you.

You have the option to focus on just one task at a time, reducing distractions and increasing productivity.

The workflow promotes order and discipline, making it ideal for those who like to work through tasks sequentially.

<!-- STRUCTURE  -->
## Project Structure

 - TFA.Client - React frontend application. (not implemented yet)

 - TFA.App - C# backend (WebAPI .NET 9). (in progress)

 - PostgreSQL - Database for persistent storage.

 - Redis - Used for caching to improve performance. (not implemented yet)

<!-- HOW TO RUN -->
## Installation

Installation

To install and run the application, follow these steps:

Prerequisites

Docker installed on your system.

.env file with required environment variables.

Steps

Clone the repository:

git clone https://github.com/your-repository/tfa.git
cd tfa

Create a .env file in the root directory and populate it with necessary values (see Environment Variables section below).

Run the application using Docker Compose:

docker-compose up --build
