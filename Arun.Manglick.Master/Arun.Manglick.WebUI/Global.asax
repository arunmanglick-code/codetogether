<%@ Application Language="C#" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.Reflection" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {

        //EventLog log = new EventLog();

        //log.Source = "BeginRequest_ArunManglickWebUI";
        //log.WriteEntry("Web UI", EventLogEntryType.Information);
    }

    void Application_EndRequest(object sender, EventArgs e)
    {
    }
    
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

        HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);

        if (runtime == null)
        {
            return;
        }

        string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
        string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);

        if (!EventLog.SourceExists(".NET Runtime"))
        {
            EventLog.CreateEventSource(".NET Runtime", "Application");
        }

        EventLog log = new EventLog();

        log.Source = ".NET Runtime";
        log.WriteEntry(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}", shutDownMessage, shutDownStack), EventLogEntryType.Error);
    }

    void Application_Error(object sender, EventArgs e)
    {
        if (Context != null && Context.User.IsInRole("Developer"))
        {
            Exception err = Server.GetLastError();
            Response.Clear();

            Response.Write("<h1>" + err.InnerException.Message + "</h1>");
            //Response.Write("<pre>" + err.ToString + "</pre>");

            Server.ClearError();
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        //EventLog log = new EventLog();

        //log.Source = "SessionStart_ArunManglickWebUI";
        //log.WriteEntry("Web UI", EventLogEntryType.Information);


    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>

