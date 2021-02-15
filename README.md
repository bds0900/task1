# Task1

## Structure
This solution has 3 projects in it.
### 1. DataSource
This is a library that support CRUD opperation

### 2. Task1
This project has two controllers, AuthController and StatusController
- [x] Section1 (AuthController)
- [x] Section2 (StatusController)
- [ ] Section3

### 3. Task1Test
Integration test for Task1
- [x] Signin
- [x] Login
- [x] Create
- [ ] Read
- [ ] Update
- [ ] Delete


## Database
SQl server
- use in-memory database for login and signin, so it doesn't require database setting (Section 1)

MongoDb
- use MongoDb for CRUD operation (Section 2)


## Setting for MongoDb
- port: **27019**
- username: **mongoadmin**
- password: **secret**

#### For docker setting
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
This program runs on Windows. I haven't tested it on Linux, but since I used .Net Core, it should run on Linux as well. 

## Command
### Run project
1. open powershell
2. go to **Task1** folder
3. type **dotnet run** to run the project

### Test
1. open powershell
2. go to **Task1Test** folder
3. type **dotnet test** to test the project

<br/>

# API Reference
https://speer-task1.herokuapp.com/api/auth  
https://speer-task1.herokuapp.com/api/status
<br/>


## **Section1**

## **Post** method
### **api/auth/signin**
>https://speer-task1.herokuapp.com/api/auth/signin

|  Namee |Type   |  Required |  Description |   
|---|---|---|---|
|  username | string  |  Required | Unique id or email |  
|  password | string  |  Required |   |
|  confirmpassword | string  |  Required |   |
```
body
{
    "username":"bds0900",
    "password":"zxcasdqwe",
    "confirmpassword":"zxcasdqwe"
}
```

<br />
<br />

### **api/auth/login**
>https://speer-task1.herokuapp.com/api/auth/login

|  Namee |Type   |  Required |  Description |   
|---|---|---|---|
|  username | string  |  Required | Unique id or email |  
|  password | string  |  Required |   |
```
body
{
    "username":"bds0900",
    "password":"zxcasdqwe"
}
```
<br />
<br />

### **api/auth/logout**
>https://speer-task1.herokuapp.com/api/auth/logout

```
Doesn't need body
```
<br />
<br />

## **Section2**

## **Post** method
### **api/status/**
>https://speer-task1.herokuapp.com/api/status/


|  Namee |Type   |  Required |  Description |   
|---|---|---|---|
|  text | string  |  Required |  the tweet content |  

```
body
{
    "text":"hi there"
}
```
<br />
<br />

## **Get** method
### **api/status/{id}**
>https://speer-task1.herokuapp.com/api/status/{id}

|  Namee |Type   |  Required |  Description |   
|---|---|---|---|
|  id | string  |  Required |  id of tweet, not username |  
<br />
<br />

## **Patch** method
### **api/status/{id}**
>https://speer-task1.herokuapp.com/api/status/{id}

|  Namee |Type   |  Required |  Description |   
|---|---|---|---|
|  id | string  |  Required |  id of tweet, not username |  
|  text | string  |  Required |  what to updatee |  

```
body
{
    "text":"update content"
}
```
<br />
<br />

## **Delete** method
### **api/status/{id}**
>https://speer-task1.herokuapp.com/api/status/{id}

|  Namee |Type   |  Required |  Description |   
|---|---|---|---|
|  id | string  |  Required |  id of tweet, not username |  


