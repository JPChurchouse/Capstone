# Instructions


## Creating a new program

1. Open the solution file _Splitlane_Tracker_Programs.sln_.
![](Screenshot%202023-06-16%20224240.png)

2. Create a new project inside the solution.
![](Screenshot%202023-06-16%20225315.png)

3. Ensure whatever your project is, is able to use ***.net 7***. If not, go back and try a different flavour of project.
![](Screenshot%202023-06-16%20225350.png)

4. Include a reference to the library.
![](Screenshot%202023-06-16%20230255.png)
![](Screenshot%202023-06-16%20230314.png)

5. To use the library functions, include `using XKarts;` at the top of the file.
 ![](Screenshot%202023-06-16%20225430.png)
 
 
## Using XKarts.Logging
+----------------------------------------------+---------------------------------------------------+
| Action                                       | Command                                           |
+==============================================+===================================================+
| Import Logging                               | `XKarts.Logging;`                                 |
+----------------------------------------------+---------------------------------------------------+
| Create new Logger                            | `Logger my_log = new Logger();`                   |
+----------------------------------------------+---------------------------------------------------+
| Log string as debug                          | `my_log.log(my_string)`                           |
|                                              | `my_log.log(my_string, Logger.Type.debug);`       |
+----------------------------------------------+---------------------------------------------------+
| Log string as information                    | `my_log.log(my_string, Logger.Type.information);` |
+----------------------------------------------+---------------------------------------------------+
| Log string as error                          | `my_log.log(my_string, Logger.Type.error);`       |
+----------------------------------------------+---------------------------------------------------+
| Log exception                                | `my_log.log(my_exception);`                       |
+----------------------------------------------+---------------------------------------------------+
| Launch File Explorer and select the log file | `my_log.open();`                                  |
+----------------------------------------------+---------------------------------------------------+
