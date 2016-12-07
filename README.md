# Running instructions
The application has been built using VS2015.
It contains a unit test project that can be run to test the correctness of the implementation.
Alternatively you can also execute the web project (Inventory.WebAPI) and invoke the API methods via web browser/Fiddler.
[Shameless plug here, you can use the Fiddler extension I developed a while ago to make things easier] ;-)

http://fiddlertreeviewpanel.codeplex.com/

The WebAPI works by default with JSON, but can also work with XML data (using the corresponding headers in Fiddler for example).

TODO: Swagger is only for net.Core???
TODO: Host it in Azure and have Runscope???


# Design notes:
- I have not coded against DB. I have implemented an in-memory storage solution making it thread-safe via the Singleton pattern so there are no issues given the multi-thread nature of web services.
Basically it's just a very simple internal List<T> with a sample defined Item class that contains the information required. For the sake of the demo and strong-typing the Types are referring to dummy enumeration values.
The solution contains a couple of projects with the logic separated in DAL and WebAPI to make things simpler.

-When running the web application from within VS it gets hosted in IISExpress in a URL such as:

http://localhost:2099/api/Inventory

which you can use with the usual HTTP verbs to test the implementation

Given how WebAPI is stateless, I can't think of a good user interaction when it comes to notifications, so IMHO a dedicated service for that matter should work better.
Issue is that the user could proactively check in some kind of local "logging" system but it's not a good idea, needs to be alerted of changes.

- It's not specified in the requirements whether an item should be removed when it expires (even though users get a notification).
I don't think it should be the inventory's responsibility since it shold be up to the final consumer to check expiration of the pulled item from the inventory.
Now I'm a huge fan of Enterprise Library to handle "logging" so I would probably end up implementing something like that to send messages, write logs or whatever destinations were to be defined.
[Another shameless plug, you can see how also back in the day I created a trace listener to write logs to SharePoint http://entlibextensions.codeplex.com/] ;-)
For the sake of simplicity in this sample however I'll just use regular SMTP server to send the notifications.
- Since I don't have an SMTP server configured, the best way to try out the notifications is to use smtp4dev tool (http://smtp4dev.codeplex.com/)

# Assumptions:
- No dealing with encoding/decoding issues from the requests (i.e.: blank spaces or strange characters).
- No dealing with string constants for error messages.
- HttpStatusCode 422 is not included in the .Net enumeration, so "hardcoded" it there to return a more menaningful response.
- Didn't deal with verification of appsettings correctness or whether the values exist or not. For this demo I'll assume they're all provided.