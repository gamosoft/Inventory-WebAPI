## Running instructions
- The application has been built using VS2015.
- It contains a unit test project that can be run to test the correctness of the implementation.
- Alternatively you can also execute the web project (Inventory.WebAPI) and invoke the API methods via web browser/Fiddler.
- Shameless plug here, you can use the Fiddler extension I developed a while ago to make things easier ;-) [http://fiddlertreeviewpanel.codeplex.com/]
- The WebAPI works by default with JSON, but can also work with XML data (using the corresponding headers in Fiddler for example).
- In the initialization of the app I provide some dummy data to aid in testing through the method "InitializeDummyData" which should be removed prior to deployment of course.

## Documentation (new)
- Just for kicks, I've added the swagger-style documentation that gets generated automatically so you have an additional way of testing the operations.
- It can be accessed in the /swagger folder, such as [http://localhost:2099/swagger]

## Design notes:
- I have not coded against DB. I have implemented an in-memory storage solution making it thread-safe via the Singleton pattern so there are no issues given the multi-thread nature of web services.
- Use a ConcurrentDictionary as internal repository for the items (stored by key, case-insensitive).
- For the sake of the demo and strong-typing the Types are referring to dummy enumeration values.
- The solution contains a couple of projects with the logic separated in DAL and WebAPI to make things simpler.
- When running the web application from within VS it gets hosted in IISExpress in a URL such as:

[http://localhost:2099/api/Inventory]

which you can use with the usual HTTP verbs to test the implementation

- Given how WebAPI is stateless, I can't think of a good user interaction when it comes to notifications, so IMHO a dedicated service for that matter should work better. Issue is that the user could proactively check in some kind of local "logging" system but it's not a good idea, needs to be alerted of changes.
- It's not specified in the requirements whether an item should be removed when it expires (even though users get a notification).
I don't think it should be the inventory's responsibility since it shold be up to the final consumer to check expiration of the pulled item from the inventory.
Now I'm a huge fan of Enterprise Library to handle "logging" so I would probably end up implementing something like that to send messages, write logs or whatever destinations were to be defined.
- Another shameless plug, you can see how also back in the day I created a trace listener to write logs to SharePoint [http://entlibextensions.codeplex.com/] ;-)
For the sake of simplicity in this sample however I'll just use regular SMTP server to send the notifications.
- Since I don't have an SMTP server configured, the best way to try out the notifications is to use smtp4dev tool [http://smtp4dev.codeplex.com/]
- For the expiration part, the way I've decided to do it is via the CacheManager class. Whenever adding an item to the inventory a cache will be created with the specified expiration, so upon eviction a delegate will execute and send the recipients a notification.
- Security has been designed on a Windows Authentication basis and SSL. Since it's a service directed towards a company's inventory, I'd discard other more complex authentication mechanisms such as OAuth (do we really need to log in using a facebook account? ;-D).
Instead, just securing the app using HTTPS and using windows authentication should suffice for this exercise.
Given how this is a solution in Visual Studio running under IISExpress, it's very simple to use, just click the WebAPI project and select these values:
[Anonymous Authentication=Disabled], [SSL Enabled=True], [Windows Authentication=Enabled].
Next uncomment the controller attribute [Authorize] and after building use the https uri that gets generated to manually test the application.
What this does is changing the applicationhost.config file behind the scenes (user dependent so you won't see it commited in GitHub) that dictates how IISExpress works.
- In "real life" hosting it in IIS for example you'd have to install a server certificate and deal with the authentication/authorization using the IIS Manager console.

## Assumptions:
- For unit testing the notification messages, they're dump in c:\tmp folder, so it needs to exist beforehand and make sure nobody else writes to that folder.
- No dealing with encoding/decoding issues from the requests (i.e.: blank spaces or strange characters).
- No dealing with string constants for error messages.
- HttpStatusCode 422 is not included in the .Net enumeration, so "hardcoded" it there to return a more menaningful response.
- Didn't deal with verification of appsettings correctness or whether the values exist or not. For this demo I'll assume they're all provided.
- Don't care much about web.config transformations for production (such as setting debug=false, which is done by default, but others that may be needed...).