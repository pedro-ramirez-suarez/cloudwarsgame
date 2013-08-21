
<h1><img src="https://raw.github.com/pedro-ramirez-suarez/cloudwarsgame/master/CloudWars.Game/Images/logo.png"/></h1>
<img src="https://raw.github.com/pedro-ramirez-suarez/cloudwarsgame/master/CloudWars.Game/img/playing.jpg"/>
<h2>Before you run the game</h2>
<p>
    <ul>
        <li>The Needletail Migrations library will create all the tables and populate initial data but you need at least MSSQL Express 2008 R2</li>
        <li>Create a new database</li>
        <li>
            Modify the Default connection string on <code>web.config</code> to match your environment.
        </li>
      <li>
        <a href="http://blogs.msdn.com/b/webdev/archive/2012/08/15/oauth-openid-support-for-webforms-mvc-and-webpages.aspx">Enable and setup Open Auth </a>
      </li>
    </ul>
    <h4>Important</h4>
    <ul>
        <li>The game is not compatible with any version of Internet Explorer.</li>
        <li>Do not use IIS Express, SSE is not properly implemented on it, some events are lost or not sent live</li>
        <li>All the functionality related with account management is the one included with the Asp.Net MVC 4 template</li>
    </ul>
 </p>

  <h2>Built with:</h2>
  <ul>
    <li>Asp.Net MVC 4</li>
    <li>AntiXss</li>
    <li>C#</li>
    <li>Html5</li>
    <li>Javascript</li>
    <li>JQuery</li>
    <li>Bootstrap 3 RC2</li>
    <li>Needletail DataAccess</li>
    <li>Needletail DataAccess Migrations</li>
    <li>Needletail MVC</li>
  </ul>
  
<h2>Why Needletail libraries</h2>
<p>There are a lot of good libraries out there, but sometimes only the creator of the library or people with the "right" problem will 
try new libraries and then appreciate it's features, so my purpose was to create an application that shows what the Needletail libraries can do for you.
</p>


<h2>Architecture and points of interest</h2>
<img src="https://raw.github.com/pedro-ramirez-suarez/cloudwarsgame/master/architecture.jpg"/>
<p>We use Needletail's Micro ORM to access the database, since the library provide fast access to the data, a social game it's an excelent scenario for it, Needletail MVC is used to push/send events on real time to the browser without breaking up your MVC pattern.</p>

<h2>How needletail tools were used</h2>
<h4>Needletail.DataAccess</h4>
<p>The CloudWars.DataAccess library project shows how Needletail.DataAccess was used, you can do the same thing in a few different ways with the library so instead of using the same approach, I use different approaches to get or modify data, a few samples are:</p>
<ul>
    <li>
        <p>Get a challenge stored in the database</p>
        <pre><code>return CloudWarsDB.Challenges.GetSingle( where: new { Id = challengeId } );</code></pre>
    </li>
    <li>
        <p>Create a new challenge</p>
        <pre>
            <code>
                var c = new Challenge { Id = Guid.NewGuid(), Player1 = fromId, Player2 = toId };
                CloudWarsDB.Challenges.Insert(c);
            </code>
        </pre>
    </li>
    <li>
        <p>update a player</p>
        <pre>
            <code>
                CloudWarsDB.Players.UpdateWithWhere(values: values, where: where);
            </code>
        </pre>
    </li>
</ul>

<h4>Needletail.DataAccess.Migratoins</h4>
<p>Since we don't want to manually create the database, we use the Migrations library to automatically create and prepupulate the database for us.</p>
<ul>
    <li>
        <p>Creating/updating the database on Global.asax</p>
        <pre><code>Needletail.DataAccess.Migrations.Migrator.Migrate("DefaultConnection",Server.MapPath("~"));</code></pre>
    </li>    
</ul>


<h4>Needletail.MVC</h4>
<p>The purpose of the game is to interact in real time with your opponent as you are playing, there is a well known alternative for Needletail.MVC which is SignalR, but one thing that I don't like about it, is that it force you to follow some conventions and pattern, since the game
was developed with Asp.Net MVC 4, it does not feel natural to break your pattern to implement some parts of the functionality, so I decided to build a library for that purpose(with some limitations), here is how the library was used on the game</p>
<ul>
    <li>
        Chatting with your opponent
        <pre>
            <code>
                //This code is on the GameController
                [HttpPost]
                public TwoWayResult PlayerChat(string message, string clientId)
                {
                    //sanitize the message
                    message = Encoder.HtmlEncode(message);
                    dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
                    call.game.receiveMessage(message); //game.receiveMessage is a javascript method
                    return new TwoWayResult(call);
                }
            </code>
        </pre>
    </li>
    <li>
        Invite your opponent to start the match
        <pre>
            <code>
                //This code is on the GameController
                [HttpPost]
                public  TwoWayResult InvitePlayer(Guid matchId, string clientId)
                {
                    var player = factory.GetPlayerPresence().GetPlayerNameByClientId(Context.ConnectionId);
            
                    dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
                    call.game.inviteToMatch(matchId, player); //game.inviteToMatch is a javascript method
                    return new TwoWayResult(call);
                }
            </code>
        </pre>
    </li>
</ul>

<h4>Not perfect</h4>
<p>The game is still under development so you may find some bugs, feedback will be appreciated</p>
<p>
You can read more about Needletail libraries <a href="http://pedro-ramirez-suarez.github.io/needletailtools/" target="_blank">here</a>
</p>
