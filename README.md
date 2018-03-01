# ToWer.DbWrapper

ToWer.DbWrapper is library which allows you to call stored procedures in a database without manually read the result of the data reader.
You just have to create a class, representing the result of your stored procedure with a constructor. Now you can call the ReadSingle<T> or ReadList<T> function of the DbWrapper, where T is your new class.