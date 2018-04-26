<html>
<head>
    <title>Classic ASP</title>

</head>
<body>
    <h1>DAL Classic</h1>

    <% Err.Clear %>
    <% On Error Resume Next %>
    <%   
    Dim dbContext
    Dim rs
    Set dbContext = Server.CreateObject("WD.DataAccess.Context.DbContext") 
    response.write "*************SQL**************<br/>"
    dbContext.Constructor "server=172.21.12.60;database=db_Log;uid=dal_user;password=dal_user*123",1 
    response.write dbContext.ConfigSiteLocation
    response.write "<br/>***************************"
    SET rs=dbContext.ICommands.ExecuteRecordSet("SELECT * FROM TEMPEMPLOYEE")
    response.write "<br/>SELECT * FROM TEMPEMPLOYEE<br/>"
     response.write "***************************"
    if Not rs.EOF Then 
        Response.write "<table border='1'>"
         rs.MoveFirst
            Do While Not rs.EOF
            Response.write "<tr>"
            Response.Write "<td>" &  rs("FirstName")& "</td>"
             Response.Write "<td>" &  rs("MiddleName")& "</td>"
             Response.Write "<td >" &  rs("LastName")& "</td>"	
            Response.Write "</tr>"
            rs.MoveNext
            Loop
          Response.write "</table>"
        rs.close
     End if
    
    %>
</body>
</html>
