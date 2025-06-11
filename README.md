In configuration file there is mainly defined ConnectionString as a DefaultConnection.
DefaultConnection needs to have:
- Data Source (with sever localization with port)
-  User ID and password
-   Trust Server Certificate set to the True

Solution is devided into three Projects:
1. API - That's where all endpoints are descibed with all the implementation
2. Models - All classes from databases with MasterContext as a handler of all operations on Tables
3. DTO - All request and response bodies that is used in this application.
