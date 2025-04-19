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
        // Log the error
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