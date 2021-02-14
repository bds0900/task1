# Task1

## Structure
### DataSource
- This is a library that support CRUD opperation

### Task1
- this program two controller, AuthController and StatusController
- [x] Section1
- [x] Section2
- [ ] Section3

### Task1Test
Integration test for Task1
- [x] Signin
- [x] Login
- [x] Create
- [ ] Read
- [ ] Update
- [ ] Delete


## Database
- use in-memory database for login and signin, so it doesn't require database setting (Section 1)

- use MongoDb for CRUD operation (Section 2)


### Below is docker setting for MongoDb
- port: **27019**
- username: **mongoadmin**
- password: **secret**
```
docker run -d `
--name mymongo `
-p 27019:27019 `
-e MONGO_INITDB_ROOT_USERNAME=mongoadmin `
-e MONGO_INITDB_ROOT_PASSWORD=secret `
-v c:\Users\Doosan\mongo:/data/db `
mongo
```

## Working Enviornment
This program runs on Windows. I haven't tested it on Linux, but since I used .net core, it should run on Linux as well. 

## Command
### Running project
1. open powershell
2. go to **Task1** folder
3. type **dotnet run** to run the project

### Test
1. open powershell
2. go to **Task1Test** folder
3. type **dotnet test** to test the project
