# LibraryAPI

This is a web application to simulate a library, implemented in ASP.NET Core 8 using EF Core, JWT, AutoMapper, Swagger and other technologies.

## Requirements

### Description 
Development of a web application to simulate a library (creation, modification, deleting, receiving), is performed on .Net Core, using EF Core. To develop the client side of the application, you should use Angular or React. The application must be deployed and run using Docker. The instructions for launching the project should be provided in the form README.md the file. 
### Functionality of the Web API
Working with books: 
1. Getting a list of all books; 
2. Getting a specific book by its Id; 
3. Getting a book by its ISBN; 
4. Adding a new book; 
5. Changing information about an existing book; 
6. Deleting a book; 
7. Handing out books to the user; 
8. The ability to add an image to a book and store it; 
9. *Sending a notification about the expiration date of the book (will be a plus). 
### Working with authors: 
1. Getting a list of all the authors; 
2. Getting the author by his Id; 
3. Adding a new author; 
4. Changing information about an existing author; 
5. Deleting the author; 
6. Getting all books by author. 
### Information about the book 
1. ISBN; 
2. Name; 
3. Genre; 
4. Description;
5. The author; 
6. The time when the book was taken; 
7. The time when the book should be returned. 
### Information about the author 
1. Name; 
2. Last name; 
3. Date of birth; 
4. Country. 
### Development of the client part of the application 
1. A registration/authentication page should be implemented;  
2. Implement a book list display page. If all books with the given author and title were taken from the library, display that the book is out of stock; 
3. Implement a page displaying information about a specific book. 
If the book is available, the user can take it.; 
4. For the admin to implement the add/edit book page; 
5. For the admin, there should be "Edit" and "Delete" buttons on the page with information about the book. Clicking on the edit button redirects to the edit/add book page. Clicking on the delete button opens a modal window confirming the deletion of the book.; 
6. Each user should have a page where they can view books they have borrowed from the library.; 
7. Implement pagination, book title search , genre/author filtering. 
### Web API Requirements 
1. Implementation of policy-based authorization using refresh and jwt 
access tokens; 
2. Implementation of the repository pattern; 
3. Development of middleware for global exception handling;
4. Implementation of pagination; 
5. Ensuring that unit tests cover all use cases/services of one model and two methods of its repository (one for receiving and one for saving data). The model is chosen at the discretion of the candidate (for all models must have written use cases/services). It is allowed
to use InMemoryDatabase; 
6. *Implementation of image caching (will be a plus). 
### Technologies required for use 
1. .Net 5.0+; 
2. Entity Framework Core; 
3. MS SQL / PostgreSQL or any other; 
4. AutoMapper / Mapster or any other; 
5. FluentValidation; 
6. Authentication via jwt access & refresh token (ex.: IdentityServer4); 
7. Swagger; 
8. EF Fluent API; 
9. Angular/React; 
10. xUnit/nUnit; 
11.  Docker. 
## Architecture 
The application must be developed on a layered or pure architecture.

## How to launch a project

**Clone the repository:**

   ```bash
   git clone https://github.com/Rudzeniapol/LibraryWebAPI.git
   cd LibraryAPI
   ```
**Assemble and launch the containers:**

```bash
docker-compose up --build
```
The app will be available:

API: http://localhost:8080
Swagger UI: http://localhost:8080/swagger
Configuring the database
In the docker-compose file.The yml image of the MS SQL Server is used. The connection string for the API is defined via the ConnectionStrings__DefaultConnection environment variable.
If you need to change the database settings, edit the variables in docker-compose.yml.

## Assembly and testing
**For a local project build:**

```bash
dotnet build
```
**To run the tests:**

```bash
dotnet test
```
## Additional information
API is built on .NET 8.
To manage the API, the Swagger UI is used, available at http://localhost:8080/swagger .
The following technologies are used: EF Core, AutoMapper, JWT authentication, FluentValidation (if implemented), etc.
Contacts
If you have any questions, please create an issue in the repository or contact the developer.