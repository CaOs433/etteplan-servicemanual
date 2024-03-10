# Service Manual

## 📝 Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Tests](#tests)
- [Built Using](#built_using)

## 🧐 About <a name = "about"></a>

A service manual CRUD API with .NET, PostgreSQL and Docker.

## 🏁 Getting Started <a name = "getting_started"></a>

To run the app, you need to have Docker installed and running.

You need to also add a .env (environment variables file named `.env`) into the root with following values:

```bash
# Database configuration:
DB_HOST=db  # This must be the same as in `compose.yaml`
DB_PORT=5432
DB_DATABASE=factory
DB_USER=postgres
DB_PASSWORD=REPLACE_WITH_YOUR_OWN_PASSWORD  # Just put some password, it will be automatically created

DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
```

Now you can start the app by running `docker compose up --build` in the root folder.

You can also use the `start.sh` script for a quick setup. It asks for the `.env` values
and creates/updates the file if it's not already created with the necessary values.
Finally it runs `docker compose up --build`.

The app will be available at <http://localhost:8080>.

Now you can try `curl http://localhost:8080/api/FactoryDevices/` to get:

```json
[
  {
    "id": 1,
    "name": "Device 0",
    "year": 2004,
    "type": "Type 19"
  },
  {
    "id": 2,
    "name": "Device 1",
    "year": 1987,
    "type": "Type 2"
  },
  {
    "id": 3,
    "name": "Device 2",
    "year": 1982,
    "type": "Type 11"
  },
  ...
]
```

See `Requests.http` -file for more example requests
(You can use a Visual Studio Code extension for making the requests directly from the file.
See REST Client: <https://marketplace.visualstudio.com/items?itemName=humao.rest-client>).

## 🔧 Running the tests <a name = "tests"></a>

Work in progress...

## ⛏️ Built Using <a name = "built_using"></a>

- [PostgreSQL](https://www.postgresql.org/) - Database
- [.NET](https://dotnet.microsoft.com/en-us/) - Project Framework
- [Docker](https://www.docker.com/) - Environment Container
