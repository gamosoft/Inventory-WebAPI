The application has been built using VS2015.
It contains a unit test project that can be run to test the correctness of the implementation.
Alternatively you can also execute the web project (Inventory.WebAPI) and invoke the API methods via web browser/Fiddler.
[Shameless plug here, you can use the Fiddler extension I developed a while ago to make things easier] ;-)
http://fiddlertreeviewpanel.codeplex.com/


TODO: Swagger is only for net.Core???


Design notes:
I have not coded against DB. I have implemented an in-memory storage solution making it thread-safe via the Singleton pattern so there are no issues given the multi-thread nature of web services.
Basically it's just a very simple internal List<T> with a sample defined Item class that contains the information required. For the sake of the demo and strong-typing the Types are referring to dummy enumeration values.
The solution contains a couple of projects with the logic separated in DAL and WebAPI to make things simpler.
When running the web application from within VS it gets hosted in IISExpress in a URL such as:

http://localhost:2099/api/Inventory

which you can use with the usual HTTP verbs to test the implementation

Assumptions:
no dealing with encoding/decoding issues from the requests (i.e.: blank spaces or strange characters)