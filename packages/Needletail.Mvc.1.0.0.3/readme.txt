Needletail.Mvc 

Needletail.Mvc allows you to call javascript code anywhere from your MVC project, 
this is allows you to tell the client(browser) to call some function when an event occurs on the server's code in real time.

Only works with MVC 4 and with browsers that support SSE(sorry, no IE).

NeedletailMvc.js has been added to your Scripts folder, you can change it to fit your needs.
RemoteController has been added to your Controllers folder, take a look to it and add code to handle some evets.

You need to add a reference to NeedletailMvc.js before make any call on the server like this.
<script src="@Url.Content("~/Scripts/NeedletailMvc.js")" type="text/javascript"></script>

Also, you need to initialize the library to point it where the controller that handles the communication is located,
by default is the "RemoteController".
An example of how to initialize the library using the default controller added by the nuget package:
<script language="javascript" type="text/javascript">
    var twoway = '@Url.Action("Remote", "Api", new { })';
    needleTail.initialize(twoway);
</script>

You can find a simple chat application here:
https://github.com/pedro-ramirez-suarez/Needletail.Mvc.Sample