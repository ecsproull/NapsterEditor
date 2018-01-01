# NapsterEditor
Napster supports a set of RESTful apis for extending their music service. This project is an example of how to work with their API’s using C#. The library code that is contained in ‘App_Code\NapsterApiHelper.cs’ and associated data classes in ‘Api_Code\JsonClasses’ could be used in any C# project. For this example, I chose ASP.NET MVC simply because I knew very little about it. Yes, I have a learning problem/addiction. 😊 I also knew nothing about REST, so it was double good. If you happen to be an expert in either I’d love to have your feedback.

Initially I set out to solve a problem and that was scrambling very long playlists. One of the menu items in the sample is ‘Playlist Editor’ and that is the functional part of the program. The other options are just for demo purposes. For now, the project is published and running at http://edsview.azurewebsites.net/ 

TODO’s to use the code.
1.) You will need to go to https://developer.napster.com/ and create an account.
2.) Next you need to add an app to your Napster developer account to obtain a API Key (ClientId) and an API Secret (Client Secret). If you read the documentation you will see both names used.
3.) Clone the repo.
4.) Edit ‘App_Code\AccessProperties.ds’ to add you ID and Secret keys.
5.) While you are there you want to update the CallbackUri to match your environment. These are used during the login process, assuming of course, you have an account and will want to use the most awesome part of the example. The playlist editor.
6.) I have set bower.json to download into wwwroot\lib but it seems to only do that on Monday’s and Thursdays. YMMV. If it lands in the root of the project you can just copy it to the correct path. It has to be under wwwroot to be available at runtime.
7.) If you aren’t familiar with ASP.NET MVC you might want to at least read the intro. Else this app will make no sense. https://msdn.microsoft.com/en-us/library/dd381412(v=vs.108).aspx 

JsonClasses.cs
Wow that looks like a lot of work. Obviously, it would be if you did that all by hand but I didn’t, and wouldn’t. In the GetObjectAsync function you can add a couple lines of code and read the Response in a string.

using (TextReader textReader = new StreamReader(responseStream, true))
{
      jsonString = textReader.ReadToEnd();
}

Using the debugger set a breakpoint after the string is created. Copy it to the clipboard. If you are using Visual Studio there is a paste option to past as JSON Classes. If you don’t use VS you can convert it here: http://json2csharp.com/ If you want you can stop there but you can never change the class or the associated JSON. I prefer to decorate them with DataContract and DataMember attributes.

    [DataContract]
    public class NapsterData
    {
        [DataMember(Name = "access_token")]
        public string AccessProperties { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }
    }

This allows me to alter the classes to contain more info that my program needs and not have to exactly match the JSON they receive. You can also change the actual name and that allows me to Pascal case my properties just like the beloved .NET guidelines calls for. Since I was in the meeting where we argued over these guidelines I feel confined to use them. Yes, I’m old and worked many years there. I’ll let you guess where ‘there’ is. 😊
Again, I didn’t add the attributes by hand either. I wrote a quick and dirty app to do it for me. If you read this far and want the app use the email at the bottom and I’ll share it.

BEWARE! with the docorations you will almost never fail JSON conversion. The conversion does what it can and ignores the rest. This can lead to missing data. It is just a design decision / trade off. Each choice has its pitfalls. Choose wisely!

WTF? WebRequest?
With the array of choices, I chose the bottom feeder. If you decide you want to switch this to HttpWebRequest or any other .NET derivative you won’t have to change much. All the others derive from it. http://www.karthikscorner.com/sharepoint/webrequest-vs-httpwebrequest-vs-webclient-vs-httpclient/ 
Keep in mind that WebRequest throws exceptions on error and you won’t see much use of try/catch. There are two top reasons for this. Either the code is perfect, or the developer is lazy. This is a sample and not production code. It is your job to make it perfect. 
Issues? Feel free to send mail to ecs3@po.cwru.edu 
