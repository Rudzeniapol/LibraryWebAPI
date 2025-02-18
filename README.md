# LibraryAPI

Это веб-приложение для имитации библиотеки, реализованное на ASP.NET Core 8 с использованием EF Core, JWT, AutoMapper, Swagger и прочих технологий.

## Требования

- Docker
- Docker Compose

## Как запустить проект

1. **Клонируйте репозиторий:**

   ```bash
   git clone https://github.com/Rudzeniapol/LibraryWebAPI.git
   cd LibraryAPI
   ```
Соберите и запустите контейнеры:

```bash
docker-compose up --build
```
Приложение будет доступно:

API: http://localhost:8080
Swagger UI: http://localhost:8080/swagger
Настройка базы данных
В файле docker-compose.yml используется образ MS SQL Server. Строка подключения для API определяется через переменную окружения ConnectionStrings__DefaultConnection.
Если необходимо изменить настройки базы данных, отредактируйте переменные в docker-compose.yml.

Сборка и тестирование
Для локальной сборки проекта:

```bash
dotnet build
```
Для запуска тестов:

```bash
dotnet test
```
Дополнительная информация
API построено на .NET 8.
Для управления API используется Swagger UI, доступный по адресу http://localhost:8080/swagger.
Используются технологии: EF Core, AutoMapper, JWT-аутентификация, FluentValidation (если реализовано) и т.д.
Контакты
Если у вас возникнут вопросы, пожалуйста, создайте issue в репозитории или свяжитесь с разработчиком.

---

## Итог

1. **Dockerfile** – размещается в папке с вашим проектом (где находится csproj).  
2. **docker-compose.yml** – размещается в корневой папке (рядом с .sln, если он есть).  
3. **README.md** – размещается в корневой папке, чтобы дать инструкции по сборке и развертыванию.

---

Теперь, чтобы развернуть проект, выполните в терминале:

```bash
docker-compose up --build
```
Если всё настроено правильно, приложение запустится, и вы сможете получить доступ к API через http://localhost:8080.