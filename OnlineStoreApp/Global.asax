<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["VisitorCount"] = 0;
    }

    void Application_End(object sender, EventArgs e)
    {
        // Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError();
        
        // Get the inner exception (usually more detailed)
        Exception innerException = ex;
        while (innerException.InnerException != null)
        {
            innerException = innerException.InnerException;
        }
        
        // Build the error message
        string errorMessage = "Error Timestamp: " + DateTime.Now.ToString() + Environment.NewLine +
                             "Error Type: " + ex.GetType().FullName + Environment.NewLine +
                             "Message: " + ex.Message + Environment.NewLine +
                             "Inner Message: " + innerException.Message + Environment.NewLine +
                             "Source: " + ex.Source + Environment.NewLine +
                             "Stack Trace: " + ex.StackTrace + Environment.NewLine +
                             "Inner Stack Trace: " + innerException.StackTrace + Environment.NewLine +
                             "Requested URL: " + Request.Url.ToString() + Environment.NewLine +
                             "----------------------------------------" + Environment.NewLine;
        
        // Log the error to a file
        try
        {
            System.IO.File.AppendAllText(Server.MapPath("~/App_Data/ErrorLog.txt"), errorMessage);
        }
        catch
        {
            // If we can't write to the file, at least display the error
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        Session["LastVisit"] = DateTime.Now;
        Application.Lock();
        Application["VisitorCount"] = (int)Application["VisitorCount"] + 1;
        Application.UnLock();
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends
    }
</script>